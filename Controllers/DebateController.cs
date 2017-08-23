using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVCAPI.Controllers
{
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   // [Authorize]
    [Route("api/Debate")]
    public class DebateController : Controller
    {
        public DebateController()
        {

        }
        [HttpGet]
        public IActionResult Get()
        {

            List<string> clm = new List<string>();
            foreach (var i in this.User.Claims)
                clm.Add(i.ToString());


            // return Ok(new[] { "Finally","It", "is", "Working" });
            return Ok(clm);
        }
    }
}