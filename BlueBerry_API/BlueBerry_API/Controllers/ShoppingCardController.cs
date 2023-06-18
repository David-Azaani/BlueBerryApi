using System.Net;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry_API.Controllers
{
    [Route("api/ShoppingCart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private  ApiResponse _response;

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateItemInCart(string userId, int menuItemId, int updatedQuantity)
        {
            var shoppingCart = await _db.ShoppingCarts.Include(a => a.CartItems).FirstOrDefaultAsync(a => a.UserId == userId);
            var menuItem = await _db.MenuItems.FirstOrDefaultAsync(a => a.Id == menuItemId);

            if (menuItem == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (shoppingCart == null && updatedQuantity > 0)
            {

                ShoppingCart newCart = new()
                {
                    UserId = userId
                };

                await _db.AddAsync(newCart);
                await _db.SaveChangesAsync();

                CartItem newCartItem = new()
                {
                    MenuItemId = menuItemId,
                    Quantity = updatedQuantity,
                    ShoppingCartId = newCart.Id,
                    MenuItem = null


                };
                await _db.AddAsync(newCartItem);
                await _db.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                var CartItemInCart = shoppingCart.CartItems.FirstOrDefault(a => a.MenuItemId == menuItemId);
                // EveryUser has one shopping Cart

                if (CartItemInCart == null)
                {
                    CartItem newCartItem = new()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updatedQuantity,
                        ShoppingCartId = shoppingCart.Id,
                        MenuItem = null


                    };
                    await _db.AddAsync(newCartItem);
                    await _db.SaveChangesAsync();
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    int newQuantity = CartItemInCart.Quantity + updatedQuantity;

                    if (newQuantity == 0 || newQuantity <= 0)
                    {
                        _db.CartItems.Remove(CartItemInCart);
                        if (shoppingCart.CartItems.Count() == 1)
                        {
                            _db.ShoppingCarts.Remove(shoppingCart);
                        }
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        CartItemInCart.Quantity = newQuantity;
                        await _db.SaveChangesAsync();
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;

                    }
                }



            }

            return _response;


        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetShoppingCart(string userId)
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
                var shoppingCart = await _db.ShoppingCarts
                    .Include(a => a.CartItems)
                    .ThenInclude(a => a.MenuItem)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                if (shoppingCart != null)
                {
                    //_response.IsSuccess = false;
                    //_response.StatusCode = HttpStatusCode.BadRequest;
                    //_response.ErrorMessages.Add("ShoppingCart Is not Existed!");
                    //return BadRequest(_response);





                    if (shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
                    {
                        // shoppingCart.CartTotalItems = shoppingCart.CartItems.Select(a => a.Quantity).Sum();
                        shoppingCart.CartTotalItems = shoppingCart.CartItems.Sum(a => a.Quantity);
                        shoppingCart.CartTotal = shoppingCart.CartItems.Sum(a => a.Quantity * a.MenuItem.Price);
                    }
                }
                //else
                //{
                //    shoppingCart.CartTotalItems = 0;
                //    shoppingCart.CartTotal = 0;
                //}

                _response.IsSuccess = true;
                _response.Result = shoppingCart;
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
