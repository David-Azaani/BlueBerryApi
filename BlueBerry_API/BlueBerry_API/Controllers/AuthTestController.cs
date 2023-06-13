using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry_API.Controllers
{
    [Route("api/AuthTest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<string>> GetSomething()
        {


            return "You are Authenticated";
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<string>> GetSomething(int someValue)
        {

            //authorization -> authenticated + Some access / Roles
            return "You are Authenticated with Role";

        }

    }
}
