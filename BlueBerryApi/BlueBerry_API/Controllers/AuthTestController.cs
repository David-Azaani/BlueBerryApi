using BlueBerry_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry_API.Controllers
{
    [Route("api/AuthTest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> GetSomething()
        {


            return "You are Authenticated";
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<string>> GetSomething(int someValue)
        {

            //authorization -> authenticated + Some access / Roles
            return "You are Authenticated with Role";

        }

    }
}
