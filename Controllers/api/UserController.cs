using Dolphin_AI.ApplicationContext;
using Dolphin_AI.Controllers.JWT;
using Dolphin_AI.Emailservice;
using Dolphin_AI.Helpers;
using Dolphin_AI.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_AI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;
        private readonly JwtTokenHelper _jwttoken;
        private readonly EmailService _senemail;
        public UserController(ApplicationDbcontext dbcontext, JwtTokenHelper jwttoken, EmailService senemail)
        {
            _dbcontext = dbcontext;
            _jwttoken = jwttoken;
            _senemail = senemail;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> UserCreate(UserDto userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.email) ||
                    string.IsNullOrEmpty(userDto.password))
                {
                    return BadRequest(new
                    {
                        Status = "Fails",
                        Result = "Required fields missing."
                    });
                }

                if (await _dbcontext.Users.AnyAsync(o => o.email == userDto.email))
                {
                    return Ok(new { Status = "Fails", Result = "Email already exists." });
                }

                if (await _dbcontext.Users.AnyAsync(o => o.phoneno == userDto.phoneno))
                {
                    return Ok(new { Status = "Fails", Result = "Phone number already exists." });
                }

                var user = new User()
                {
                    username = userDto.username,
                    email = userDto.email,
                    phoneno = userDto.phoneno,
                    Gender = userDto.Gender,
                    password = PasswordCryptoHelper.Encrypt(userDto.password),
                    city = userDto.city,
                };
                //await _senemail.SendWelcomeEmailAsync(userDto.email, userDto.username);
                await _dbcontext.Users.AddAsync(user);
                await _dbcontext.SaveChangesAsync();

                return Ok(new { Status = "Ok", Result = "Registration Successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = "Faile",
                    Result = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPost("Authentication")]
        public async Task<ActionResult<User>> postUser(Authenticationu authenticationu)
        {
            try
            {
                var user = await _dbcontext.Users.FirstOrDefaultAsync(o => o.email == authenticationu.email);

                if (user != null)
                {
                    var dbpass = PasswordCryptoHelper.Decrypt(user.password);
                    if (dbpass != authenticationu.password)
                    {
                        return Ok(new { Status = "Faile", Result = "Rong Password" });
                    }
                    else
                    {
                        var Gtoken = _jwttoken.GenerateToken(user.Userid, user.username, user.email);
                        return Ok(new { Status = "Ok", Token = Gtoken, Result = "Login Successfully." });
                    }
                }
                else
                {
                    return Ok(new { Status = "Faile", Result = "User not Found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Faile", Result = ex });
            }
        }

        [HttpGet("Profil/{Id?}")]
        public async Task<ActionResult<List<User>>> profile(int? Id)
        {
            try
            {
                var data = await _dbcontext.Users.
                           Where(o => o.Userid == Id).
                           Select(o => new
                           {
                               o.Gender,
                               o.city,
                               o.username,
                               o.email,
                               o.phoneno,
                           }).FirstOrDefaultAsync();

                if (data != null)
                {
                    return Ok(new { Status = "Ok", Result = data });
                }
                else
                {
                    return Ok(new { Status = "Faile", Result = "Data not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Faile", Result = ex.Message });
            }
        }

        [HttpGet("Userlist")]
        public async Task<ActionResult<List<User>>> getUsers()
        {
            try
            {
                var data = await _dbcontext.Users.Select(o => new
                {
                    o.Userid,
                    o.username,
                    o.email,
                    o.phoneno,
                    o.city,
                    o.created_at
                }).ToArrayAsync();

                if (data != null)
                {
                    return Ok(new { Status = "Ok", Result = data });
                }
                else
                {
                    return Ok(new { Status = "Faile", Result = "Data not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Faile", Result = ex.Message });
            }
        }

        [HttpPost("UpdateProfile")]
        public async Task<ActionResult<User>> poseUpdate(UpdateDto userDto)
        {
            try
            {
                if (userDto.Userid == 0)
                {
                    return Ok(new { Status = "Faile", Result = "UserId is required." });
                }
                else
                {
                    var User = new User()
                    {
                        Userid = userDto.Userid,
                        username = userDto.username,
                        city = userDto.city,
                        email = userDto.email,
                        phoneno = userDto.phoneno,
                        Gender = userDto.Gender
                    };

                    if (userDto != null)
                    {
                        _dbcontext.Users.Update(User);
                        await _dbcontext.SaveChangesAsync();
                        return Ok(new { Status = "Ok", Result = "Profile Update Successfully." });
                    }
                    else
                    {
                        return Ok(new { Status = "Faile", Result = "User not found" });
                    }
                }


            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Faile", Result = ex.Message });
            }

        }

        [HttpPost("remove{Id?}")]

        public async Task<ActionResult> remove(int? Id)
        {
            try
            {
                var data = await _dbcontext.Users.FirstOrDefaultAsync(o => o.Userid == Id);
                if (data != null)
                {
                    _dbcontext.Users.Remove(data);
                    await _dbcontext.SaveChangesAsync();
                    return Ok(new { Status = "Ok", Result = "User Remove Successfully." });
                }
                else
                {
                    return Ok(new { Status = "Faile", Result = "User not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Faile", Result = ex.Message });
            }
        }
    }
}
