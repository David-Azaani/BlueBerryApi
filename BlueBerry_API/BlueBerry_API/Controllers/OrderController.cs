using System.Net;
using AutoMapper;
using BlueBerry_API.Data;
using BlueBerry_API.Model;
using BlueBerry_API.Model.Dto;
using BlueBerry_API.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry_API.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private ApiResponse _response;
        public OrderController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ApiResponse();
        }



        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
        {

            try
            {
                var orderHeaders = _db.OrderHeaders
                    .Include(a => a.OrderDetails)
                    .ThenInclude(a => a.MenuItem).OrderByDescending(a => a.OrderHeaderId);
                if (!string.IsNullOrEmpty(userId))
                {
                    _response.Result = orderHeaders.Where(a => a.ApplicationUserId == userId);
                }
                else
                {
                    _response.Result = orderHeaders;
                }

                _response.IsSuccess = true;
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
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetOrders(int id)
        {

            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Id is NotValid");
                    return BadRequest(_response);
                }
                var orderHeaders = _db.OrderHeaders.Include(a => a.OrderDetails)
                    .ThenInclude(a => a.MenuItem).Where(a => a.OrderHeaderId == id);
                #region MustBeChecked!
                if (!orderHeaders.Any())
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Id is NotValid or There is No Order!");
                    return NotFound(_response);
                }

                #endregion

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = orderHeaders;
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



        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDto)
        {
            try
            {
                var order = _mapper.Map<OrderHeader>(orderHeaderDto);

                order.Status = string.IsNullOrEmpty(order.Status) ? SD.Status_Pending : order.Status;

                _db.Add(order);
                await _db.SaveChangesAsync();
                foreach (var item in orderHeaderDto.OrderDetailsDTO)
                {
                    var orderDetail = _mapper.Map<OrderDetails>(item);
                    orderDetail.OrderHeaderId = order.OrderHeaderId;
                    _db.Add(orderDetail);
                }
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = order;
                order.OrderDetails = null;
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



        [HttpPut("{id:int}")]

        public async Task<ActionResult<ApiResponse>> UpdateOrderHeader(int id, [FromBody] OrderHeaderUpdateDTO orderHeaderUpdateDto)
        {
            try
            {
                if (orderHeaderUpdateDto == null || id != orderHeaderUpdateDto.OrderHeaderId)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                var orderHeaders = await _db.OrderHeaders.FirstOrDefaultAsync(a => a.OrderHeaderId == id);
                if (orderHeaders == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages.Add("Order Doesn't Exist");
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.PickUpName))
                {
                    orderHeaders.PickUpName = orderHeaderUpdateDto.PickUpName;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.PickUpEmail))
                {
                    orderHeaders.PickUpEmail = orderHeaderUpdateDto.PickUpEmail;

                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.PickUpPhoneNumber))
                {
                    orderHeaders.PickUpPhoneNumber = orderHeaderUpdateDto.PickUpPhoneNumber;

                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.Status))
                {
                    orderHeaders.Status = orderHeaderUpdateDto.Status;
                }
                if (!string.IsNullOrEmpty(orderHeaderUpdateDto.StripePaymentIntentID))
                {
                    orderHeaders.StripePaymentIntentID = orderHeaderUpdateDto.StripePaymentIntentID;
                }

                await _db.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
               
                return BadRequest(_response);















                // var newOrderHeaderDto = _mapper.Map<OrderHeaderUpdateDTO, OrderHeader>(orderHeaderUpdateDto, orderHeaders);

                // _db.SaveChangesAsync();
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
