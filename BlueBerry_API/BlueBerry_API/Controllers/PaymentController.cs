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
         private  ApiResponse _response;

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
                if (string.IsNullOrEmpty(userId))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Incorrect UserIDF!");
                    return BadRequest(_response);
                }
                var shoppingCard = await _db.ShoppingCards
                    .Include(a => a.CardItems)
                    .ThenInclude(a=>a.MenuItem)
                    .FirstOrDefaultAsync(a => a.UserId == userId);

                if (shoppingCard == null || shoppingCard.CardItems == null || !shoppingCard.CardItems.Any())
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Error in Payment,Couldn't Find any ShoppingCard or CartItem !!!");
                    return BadRequest(_response);
                }

                #region CreatePayment

                StripeConfiguration.ApiKey = _configuration["Stripe:ApiKey"];
                double cardTotal = shoppingCard.CardItems.Sum(a => a.Quantity * a.MenuItem.Price);


                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (int)(cardTotal * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>()
                    {
                        "card"

                    }

                };
                var service = new PaymentIntentService();
                var response = await service.CreateAsync(options);
                shoppingCard.StripPaymentIntentId = response.PaymentMethodId;
                shoppingCard.ClientSecret = response.Id;
                 _response.Result = shoppingCard;
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
