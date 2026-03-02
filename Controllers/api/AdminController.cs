using Dolphin_AI.ApplicationContext;
using Dolphin_AI.Controllers.JWT;
using Dolphin_AI.Helpers;
using Dolphin_AI.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dolphin_AI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly JwtTokenHelper _jwtToken;

        public AdminController(ApplicationDbcontext dbcontext, JwtTokenHelper jwtToken)
        {
            _dbcontext = dbcontext;
            _jwtToken = jwtToken;
            //cmd
        }

        [HttpPost("add")]
        public async Task<IActionResult> postuser(AdminDto adminDto)
        {
            var user = new Admin()
            {
                name = adminDto.name,
                email = adminDto.email,
                password = PasswordCryptoHelper.Encrypt(adminDto.password)
            };
            _dbcontext.Admins.Add(user);
            await _dbcontext.SaveChangesAsync();
            return Ok(new {Status = "Ok" , Result="Save successfully",adminDto.password });
        }

        [HttpPost("Authentication")]
        public async Task<ActionResult<Admin>> postAdmin(Authenticationa authenticationa)
        {
            try
            {
                var user = await _dbcontext.Admins.FirstOrDefaultAsync(o => o.email == authenticationa.email);
                

                if (user != null)
                {
                    var dbpass = PasswordCryptoHelper.Decrypt(user.password);
                    if (dbpass != authenticationa.password)
                    {
                        return Ok(new { Status = "Fails", Result = "Rong password" });
                    }
                    else
                    {
                        _jwtToken.GenerateToken(user.AdminId,user.name, user.email);
                        return Ok(new { Status = "Ok", Result = "Login Successfully." });
                    }
                }
                else
                {
                    return Ok(new { Status = "Fails", Result = "User Not Found" });
                }

            }
            catch (Exception ex)
            {
                var data = await _dbcontext.Admins.ToListAsync();
                return Ok(new { Status = "Fails", Result = ex.Message,data });
            }
        }

        [HttpGet("Profile/{Id?}")]

        public async Task<ActionResult<Admin>> postProfile(int? Id)
        {
            try
            {
                var data = await _dbcontext.Admins.Where(o => o.AdminId == Id)
                .Select(o => new
                {
                    o.name,
                    o.Baner,
                    o.Logo,
                    o.Mapurl,
                    o.email,
                    o.phoneno,
                    o.Adress
                }).ToListAsync();

                if(data.Count != 0)
                {
                    return Ok(new { Status = "Ok", Result = data });
                }
                else
                {
                    return Ok(new { Status = "Failes", Result = "Data Not Found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Satatus = "Ok", Result = ex.Message });
            }
        }
    }
}
