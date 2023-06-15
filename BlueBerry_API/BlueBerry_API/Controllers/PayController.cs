using BlueBerry_API.Data;
using BlueBerry_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {




        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private ApiResponse _response;

        public PayController(ApplicationDbContext db
        , IConfiguration configuration, ApiResponse response)
        {
            _db = db;
            _configuration = configuration;
            _response = new ApiResponse();
        }

        [HttpPost]

        public async Task<ActionResult<ApiResponse>> MakeItPay(string userId)
        {

            return Ok(userId);
        }
    }
}
