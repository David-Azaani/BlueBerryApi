using System.Net;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;


namespace BlueBerry_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        private ApiResponse _response;

        public PaymentController(ApplicationDbContext db
        , IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {


            try
            {
                //if (string.IsNullOrEmpty(userId))
                //{
                //    _response.IsSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.ErrorMessages.Add("Incorrect UserIDF!");
                //    return BadRequest(_response);
                //}
                ShoppingCart shoppingCart;

                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new ();
                }
                else
                {
                    shoppingCart = await _db.ShoppingCarts
                     .Include(a => a.CartItems)
                     .ThenInclude(a => a.MenuItem)
                     .FirstOrDefaultAsync(a => a.UserId == userId);
                }



                if (shoppingCart == null || shoppingCart.CartItems == null || !shoppingCart.CartItems.Any())
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Error in Payment,Couldn't Find any ShoppingCart or CartItem !!!");
                    return BadRequest(_response);
                }

                #region CreatePayment

                StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
                //double CartTotal = shoppingCart.CartItems.Sum(a => a.Quantity * a.MenuItem.Price);
                 shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (int)(shoppingCart.CartTotal * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>()
                    {

                        "card"

                    }

                };
                var service = new PaymentIntentService();
                var response = await service.CreateAsync(options);
                shoppingCart.StripPaymentIntentId = response.Id;
                shoppingCart.ClientSecret = response.ClientSecret;
                _response.Result = shoppingCart;


             



            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Error in PaymentService");
                _response.ErrorMessages.Add(e.Message);
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);


            #endregion


        }
    }
}
