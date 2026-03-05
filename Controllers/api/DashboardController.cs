using Dolphin_AI.ApplicationContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dolphin_AI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;

        public DashboardController(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpGet("Dashboard")]
        public async Task<ActionResult> getAdminaashboard()
        {
            try
            {
<<<<<<< HEAD
               // var users = await _dbcontext.Users.ToListAsync();

                //var email = users.First().email;
              //  var countUser = await _dbcontext.Users
                                   // .Where(o => o.email == email)
                                  //  .CountAsync();

var countUser = await _dbcontext.Users.CountAsync();       
=======
                var countUser = await _dbcontext.Users.CountAsync();

                //var usename = users.First().username;
                //var countUser = await _dbcontext.Users
                //                    .Where(o => o.username == usename)
                //                    .CountAsync();
>>>>>>> 1a6b0ff (update files)

                //var contactus = await _dbcontext.Contactus.ToListAsync();
                //var fullname = contactus.First().fullname;
                var countContactus = await _dbcontext.Contactus.CountAsync();

                return Ok(new { Status = "Ok", User=countUser, Contact=countContactus });
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Fails", Result = ex.Message });
            }
        }
    }
}
