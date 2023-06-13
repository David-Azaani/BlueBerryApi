using System.Net;
using AutoMapper;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using BlueBerry_API.Model.Dto;
using BlueBerry_API.Services.IService;
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
        private readonly IMapper _mapper;
        private readonly IFileUpload _fileUpload;
        private readonly ApiResponse _response;

        public MenuItemController(ApplicationDbContext db, IMapper mapper, IFileUpload fileUpload)
        {
            _db = db;
            _mapper = mapper;
            _fileUpload = fileUpload;
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
        [HttpGet("{id:int}", Name = "GetMenuItem")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            try
            {
                if (id == 0)
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

        [HttpPost]
        //we use fromform isntead of frombidy because we want to send image

        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (menuItemCreateDto.File.Length == 0 || menuItemCreateDto.File == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages = new List<string>()
                        {
                            "image File is  Essential"

                        };
                        return BadRequest(_response);
                    }
                    var result = _mapper.Map<MenuItem>(menuItemCreateDto);
                    result.Image = await _fileUpload.UploadFile(menuItemCreateDto.File);
                    // string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDto.File.FileName)}";
                    _db.Add(result);
                    await _db.SaveChangesAsync();
                    _response.Result = result;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { id = result.Id }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _response;
        }



        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemUpdateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (menuItemUpdateDto == null || menuItemUpdateDto.Id != id)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;

                        return BadRequest(_response);
                    }

                    var menuItemFromDb = await _db.MenuItems.FindAsync(id);
                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;

                        return BadRequest(_response);
                    }

                    var newUpdatedto = _mapper.Map<MenuItemUpdateDTO, MenuItem>(menuItemUpdateDto, menuItemFromDb);

                    // var updateDto = _mapper.Map<MenuItem>(menuItemUpdateDto);

                    if (menuItemUpdateDto.File != null && menuItemUpdateDto.File.Length > 0)
                    {
                        _fileUpload.DeleteFile(menuItemFromDb.Image.Split('/').Last());
                        newUpdatedto.Image = await _fileUpload.UploadFile(menuItemUpdateDto.File);
                    }

                    _db.Update(newUpdatedto);
                    await _db.SaveChangesAsync();
                    _response.Result = newUpdatedto;
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return _response;
        }

    }
}
