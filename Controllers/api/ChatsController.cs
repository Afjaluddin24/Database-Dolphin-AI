using Dolphin_AI.ApplicationContext;
using Dolphin_AI.Helpers;
using Dolphin_AI.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_AI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly GeminiService _gemini;

        public ChatsController(ApplicationDbcontext dbcontext, GeminiService gemini)
        {
            _dbcontext = dbcontext;
            _gemini = gemini;
        }

        [HttpPost("QuestionAnswer")]
        public async Task<IActionResult> postChats(ChatsDto chatsDto)
        {
            try
            {
                if (chatsDto == null || string.IsNullOrEmpty(chatsDto.question))
                {
                    return Ok(new { Status = "Faile", Result = "Question is required" });
                }

                // 🔴 1️⃣ Check if user already blocked
                var existingBlock = await _dbcontext.BlockedUsers
                    .FirstOrDefaultAsync(b => b.UserId == chatsDto.UserId);

                if (existingBlock != null)
                {
                    if (existingBlock.ExpireAt > DateTime.UtcNow)
                    {
                        return Ok(new
                        {
                            Status = "Faile",
                            Result = $"You are blocked until {existingBlock.ExpireAt}"
                        });
                    }
                    else
                    {
                        // Auto remove expired block
                        _dbcontext.BlockedUsers.Remove(existingBlock);
                        await _dbcontext.SaveChangesAsync();
                    }
                }

                // 🔴 2️⃣ Check adult content BEFORE AI call
                if (ContentFilterHelper.ContainsAdultContent(chatsDto.question))
                {
                    var block = new BlockedUsers
                    {
                        UserId = chatsDto.UserId,
                        ExpireAt = DateTime.UtcNow.AddMinutes(60)
                    };

                    await _dbcontext.BlockedUsers.AddAsync(block);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(new
                    {
                        Status = "Faile",
                        Result = "Inappropriate content detected. You are blocked for 60 minutes."
                    });
                }

                // 🟢 3️⃣ Ask Gemini
                var ansGemini = await _gemini.AskAI(chatsDto.question);

                if (string.IsNullOrEmpty(ansGemini))
                {
                    return Ok(new { Status = "Faile", Result = "AI response failed" });
                }

                // 🟢 4️⃣ Save chat
                var chat = new Chats
                {
                    UserId = chatsDto.UserId,
                    question = chatsDto.question,
                    answer = ansGemini
                };

                await _dbcontext.Chats.AddAsync(chat);
                await _dbcontext.SaveChangesAsync();

                return Ok(new { Status = "Ok", Result = ansGemini });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Fails", Result = ex.Message });
            }
        }
    }
}
