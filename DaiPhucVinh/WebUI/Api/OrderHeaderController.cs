using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.OrderHeader;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.OrderLineResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/order-header")]
    public class OrderHeaderController : BaseApiController
    {
        private readonly IOrderHeaderService _orderHeaderService;
        public OrderHeaderController(IOrderHeaderService orderHeaderService)
        {
            _orderHeaderService = orderHeaderService;
        }
        [HttpGet]
        [Route("TakeOrderHeaderByStoreId")]
        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(int UserId) => await _orderHeaderService.TakeOrderHeaderByStoreId(UserId);

  }
}