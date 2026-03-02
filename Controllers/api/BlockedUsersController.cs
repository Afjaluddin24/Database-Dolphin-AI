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
    public class BlockedUsersController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;

        public BlockedUsersController(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("BlockUser/{UserId}/{Chat?}")]
        public async Task<ActionResult> BlockUser(int UserId,string? Chat)
        {
            try
            {
                var userChats = await _dbcontext.Chats
                    .Where(c => c.UserId == UserId)
                    .ToListAsync();

                if (!userChats.Any())
                {
                    return Ok(new { Status = "Faile", Result = "No chats found" });
                }

                bool hasAdultContent = userChats.Any(chat =>
                    ContentFilterHelper.ContainsAdultContent(chat.question, chat.answer));

                if (!hasAdultContent)
                {
                    return Ok(new { Status = "Ok", Result = "User chats are clean" });
                }

                var existingBlock = await _dbcontext.BlockedUsers
                    .FirstOrDefaultAsync(b => b.UserId == UserId);

                // If already blocked and not expired
                if (existingBlock != null && existingBlock.ExpireAt > DateTime.UtcNow)
                {
                    return Ok(new
                    {
                        Status = "Faile",
                        Result = $"User already blocked until {existingBlock.ExpireAt}"
                    });
                }

                // Remove expired block
                if (existingBlock != null && existingBlock.ExpireAt <= DateTime.UtcNow)
                {
                    _dbcontext.BlockedUsers.Remove(existingBlock);
                    await _dbcontext.SaveChangesAsync();
                }

                // ✅ Only ExpireAt use કરો
                var block = new BlockedUsers
                {
                    UserId = UserId,
                    ExpireAt = DateTime.UtcNow.AddMinutes(60)
                };

                await _dbcontext.BlockedUsers.AddAsync(block);
                await _dbcontext.SaveChangesAsync();

                return Ok(new
                {
                    Status = "Ok",
                    Result = "User blocked for 60 minutes"
                });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Faile", Result = ex.Message });
            }
        }

    }
}
