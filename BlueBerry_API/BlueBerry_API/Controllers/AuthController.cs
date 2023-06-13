using System.Net;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using BlueBerry_API.Model.Dto;
using BlueBerry_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BlueBerry_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string _secretKey;

        private ApiResponse _response;
        public AuthController(ApplicationDbContext db,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _response = new ApiResponse();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {

            ApplicationUser user =
                _db.ApplicationUsers.FirstOrDefault(a => a.UserName.ToLower() == model.Username.ToLower());
            if (user != null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("User already Exists");

                return BadRequest(_response);
            }

            ApplicationUser newUser = new()
            {
                UserName = model.Username,
                Email = model.Username,
                NormalizedEmail = model.Username.ToUpper(),
                Name = model.Name ?? model.Username
            };
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    // await _roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult()
                    if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = SD.Role_Admin,
                            NormalizedName = SD.Role_Admin.ToUpper()
                        }); await _roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = SD.Role_Customer,
                            NormalizedName = SD.Role_Customer.ToUpper()
                        });
                    }

                    if (model.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    }

                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.AddRange(result.Errors.Select(a => a.Description));
                return BadRequest(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add(e.Message);
            }
            //_response.IsSuccess = false;
            //_response.StatusCode = HttpStatusCode.BadRequest;
            //_response.ErrorMessages.AddRange(result.Errors.Select(a => a.Description));
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            try
            {
                ApplicationUser user =
                    _db.ApplicationUsers.FirstOrDefault(a => a.UserName.ToLower() == model.Username.ToLower());
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("User is not Existed");

                    return BadRequest(_response);
                }

                bool isValidPassweord = await _userManager.CheckPasswordAsync(user, model.Password);

                if (!isValidPassweord)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("User/Password is Invalid");
                    _response.Result = new LoginResponseDTO();
                    return BadRequest(_response);
                }
                // Generate Jwt Token

                LoginResponseDTO loginResponseDto = new()
                {
                    Email = user.Email,
                    Token = "test",
                };

                if (loginResponseDto.Email == null || string.IsNullOrEmpty(loginResponseDto.Token))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("User/Password is Invalid");
                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = loginResponseDto;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.ErrorMessages.Add(e.Message);
            }
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;

            return BadRequest(_response);


        }

    }
}
