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
            return Ok(new { Status = "Ok", Result = "Save successfully", adminDto.password });
        }

        [HttpPost("Authentication")]
        public async Task<ActionResult<Admin>> postAdmin(Authenticationa authenticationa)
        {
            try
            {
                var user = await _dbcontext.Admins.FirstOrDefaultAsync(o => o.name == authenticationa.name);


                if (user != null)
                {
                    var dbpass = PasswordCryptoHelper.Decrypt(user.password);
                    if (dbpass != authenticationa.password)
                    {
                        return Ok(new { Status = "Fails", Result = "Rong password" });
                    }
                    else
                    {
                        var Tocken = _jwtToken.GenerateToken(user.AdminId, user.name, user.email);
                        return Ok(new { Status = "Ok", Toc = Tocken, Result = "Login Successfully." });
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
                return Ok(new { Status = "Fails", Result = ex.Message, data });
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
                    o.Adress,
                    o.fullname
                }).FirstOrDefaultAsync();

                if (data == null)
                {
                    return Ok(new { Status = "Failes", Result = "Data Not Found" });
                }
                else
                {
                    return Ok(new { Status = "Ok", Result = data });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Satatus = "Ok", Result = ex.Message });
            }
        }


        [HttpPost("UpdateProfiles")]
        public async Task<ActionResult<Admin>> UpdateProfile(AdminUpdateDto adminDto)
        {
            try
            {
                if (adminDto != null)
                {
                    string logoFileName = "";
                    string bannerFileName = "";

                    if (!string.IsNullOrEmpty(adminDto.Logo))
                    {
                        var logoBytes = Convert.FromBase64String(adminDto.Logo);

                        logoFileName = DateTime.Now.Ticks + ".jpg";

                        var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", logoFileName);

                        await System.IO.File.WriteAllBytesAsync(logoPath, logoBytes);
                    }

                    if (!string.IsNullOrEmpty(adminDto.Baner))
                    {
                        var bannerBytes = Convert.FromBase64String(adminDto.Baner);

                        bannerFileName = DateTime.Now.Ticks + ".jpg";

                        var bannerPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", bannerFileName);

                        await System.IO.File.WriteAllBytesAsync(bannerPath, bannerBytes);
                    }

                    var Admin = new Admin()
                    {
                        AdminId = adminDto.AdminId,
                        name = adminDto.name,
                        fullname = adminDto.fullname,
                        email = adminDto.email,
                        phoneno = adminDto.phoneno,
                        Adress = adminDto.Adress,
                        Logo = logoFileName,
                        Baner = bannerFileName,
                        Mapurl = adminDto.Mapurl
                    };

                    _dbcontext.Admins.Update(Admin);
                    await _dbcontext.SaveChangesAsync();

                    return Ok(new { Status = "Ok", Result = "Update Successfully" });
                }
                else
                {
                    return Ok(new { Status = "Fails", Result = "Data Not Found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Satatus = "Ok", Result = ex.Message });
            }
        }

        [HttpDelete("Delete/{Id?}")]

        public async Task<ActionResult<Admin>> DeleteAdmin(int? Id)
        {
            try
            {
                var data = await _dbcontext.Admins.Where(o => o.AdminId == Id).FirstOrDefaultAsync();
                if (data != null)
                {
                    _dbcontext.Admins.Remove(data);
                    await _dbcontext.SaveChangesAsync();
                    return Ok(new { Status = "Ok", Result = "Delete Successfully" });
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
