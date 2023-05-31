using System.Net;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry_API.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;

        public MenuItemController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            try
            {
                _response.Result = await _db.MenuItems.ToListAsync();
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.Result = e.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_response);

            }

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            try
            {
                if (id==0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                
                var result = await _db.MenuItems.FirstOrDefaultAsync(a => a.Id == id);
                if (result == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = result;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.Result = e.Message;
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_response);

            }

        }



    }
}
