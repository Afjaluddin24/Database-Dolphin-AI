using Dolphin_AI.ApplicationContext;
using Dolphin_AI.Mode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dolphin_AI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactusController : ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;

        public ContactusController(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpPost("contact")]
        public async Task<ActionResult<Contactus>> postContactus(Contactus contacs)
        {
            try
            {
                if (contacs != null)
                {
                   await _dbcontext.Contactus.AddAsync(contacs);
                   await _dbcontext.SaveChangesAsync();
                   return Ok(new { Status = "Success", Result = "Save Successfully." });
                }
                else
                {
                    return Ok(new { Status = "Failes", Result = "Data is null." });
                }
            }
            catch (Exception ex)
            {
              return Ok(new { Status = "Failes", Result = ex.Message });
            }
        }

        [HttpGet("DisplayContactus")]

        public async Task<ActionResult<List<Contactus>>> GetContactus()
        {
            try
            {
                var data = await _dbcontext.Contactus.ToArrayAsync();
                if (data != null)
                {
                    return Ok(new { Status = "Ok", Result = data });
                }
                else
                {
                    return Ok(new { Status = "Failes", Result = "Data not Found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { Status = "Failes", Result = ex.Message });
            }
        }
    }
}
