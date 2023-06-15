using System.Net;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry_API.Controllers
{
    [Route("api/ShoppingCard")]
    [ApiController]
    public class ShoppingCardController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;

        public ShoppingCardController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updatedQuantity)
        {
            var shoppingCard = await _db.ShoppingCards.Include(a => a.CardItems).FirstOrDefaultAsync(a => a.UserId == userId);
            var menuItem = await _db.MenuItems.FirstOrDefaultAsync(a => a.Id == menuItemId);

            if (menuItem == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (shoppingCard == null && updatedQuantity > 0)
            {

                ShoppingCard newCard = new()
                {
                    UserId = userId
                };

                await _db.AddAsync(newCard);
                await _db.SaveChangesAsync();

                CardItem newCardItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updatedQuantity,
                    ShoppingCardId = newCard.Id,
                    MenuItem = null


                };
                await _db.AddAsync(newCardItem);
                await _db.SaveChangesAsync();

            }
            else
            {
                var cardItemInCart = shoppingCard.CardItems.FirstOrDefault(a => a.MenuItemId == menuItemId);
                // EveryUser has one shopping card

                if (cardItemInCart == null)
                {
                    CardItem newCardItem = new()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updatedQuantity,
                        ShoppingCardId = shoppingCard.Id,
                        MenuItem = null


                    };
                    await _db.AddAsync(newCardItem);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    int newQuantity = cardItemInCart.Quantity + updatedQuantity;

                    if (newQuantity == 0 || newQuantity <= 0)
                    {
                        _db.CardItems.Remove(cardItemInCart);
                        if (shoppingCard.CardItems.Count() == 1)
                        {
                            _db.ShoppingCards.Remove(shoppingCard);
                        }

                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        cardItemInCart.Quantity = newQuantity;
                        await _db.SaveChangesAsync();

                    }
                }



            }

            return _response;


        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCard(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("UserId is Empty!!");
                    return BadRequest(_response);

                }
                var shoppingCard = await _db.ShoppingCards
                    .Include(a => a.CardItems)
                    .ThenInclude(a => a.MenuItem)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                if (shoppingCard != null)
                {
                    //_response.IsSuccess = false;
                    //_response.StatusCode = HttpStatusCode.BadRequest;
                    //_response.ErrorMessages.Add("ShoppingCard Is not Existed!");
                    //return BadRequest(_response);





                    if (shoppingCard.CardItems != null && shoppingCard.CardItems.Count > 0)
                    {
                        // shoppingCard.CartTotalItems = shoppingCard.CardItems.Select(a => a.Quantity).Sum();
                        shoppingCard.CartTotalItems = shoppingCard.CardItems.Sum(a => a.Quantity);
                        shoppingCard.CartTotal = shoppingCard.CardItems.Sum(a => a.Quantity * a.MenuItem.Price);
                    }
                }
                //else
                //{
                //    shoppingCard.CartTotalItems = 0;
                //    shoppingCard.CartTotal = 0;
                //}

                _response.IsSuccess = true;
                _response.Result = shoppingCard;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);








            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add(e.Message);
            }

            return _response;
        }

    }
}
