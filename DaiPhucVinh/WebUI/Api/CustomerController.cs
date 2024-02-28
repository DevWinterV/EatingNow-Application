using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.EN_CustomerService;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.CustomerNotification;
using DaiPhucVinh.Shared.OrderHeader;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.Payment;
using DaiPhucVinh.Shared.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/customer")]
    public class CustomerController : BaseApiController
    {
        private readonly IENCustomerService _customerService;
        public CustomerController(IENCustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpPost]
        [Route("CheckCustomer")]
        public async Task<BaseResponse<EN_CustomerResponse>> CheckCustomer([FromBody] EN_CustomerRequest request) => await _customerService.CheckCustomer(request);

        [HttpPost]
        [Route("CheckCustomerEmail")]
        public async Task<BaseResponse<EN_CustomerResponse>> CheckCustomerEmail([FromBody] EN_CustomerRequest request) => await _customerService.CheckCustomerEmail(request);

        [HttpPost]
        [Route("CreateOrderCustomer")]
        public async Task<BaseResponse<bool>> CreateOrderCustomer([FromBody] EN_CustomerRequest request) => await _customerService.CreateOrderCustomer(request);

        [HttpPost]
        [Route("UpdateToken")]
        public async Task<BaseResponse<bool>> UpdateToken([FromBody] EN_CustomerRequest request) => await _customerService.UpdateToken(request);

        [HttpPost]
        [Route("UpdateInfoCustomer")]
        public async Task<BaseResponse<bool>> UpdateInfoCustomer()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = null;
            //file
            if (httpRequest.Files.Count > 0)
            {
                img = httpRequest.Files[0];
            }
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];

                var request = JsonConvert.DeserializeObject<EN_CustomerRequest>(jsonRequest);
                return await _customerService.UpdateInfoCustomer(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [Authorize]
        [HttpPost]
        [Route("TakeAllCustomer")]
        public async Task<BaseResponse<EN_CustomerResponse>> TakeAllCustomer([FromBody] EN_CustomerRequest request) => await _customerService.TakeAllCustomer(request);
        [Authorize]

        [HttpPost]
        [Route("TakeAllCustomerByProvinceId")]
        public async Task<BaseResponse<EN_CustomerResponse>> TakeAllCustomerByProvinceId([FromBody] EN_CustomerRequest request) => await _customerService.TakeAllCustomerByProvinceId(request);
        [HttpPost]
        [Route("CreateCustomerAddress")]
        public async Task<BaseResponse<bool>> CreateCustomerAddress([FromBody] EN_CustomerAddressRequest request) => await _customerService.CreateCustomerAddress(request);

        [HttpPost]
        [Route("DeleteAddress")]
        public async Task<BaseResponse<bool>> DeleteAddress([FromBody] EN_CustomerAddressRequest request) => await _customerService.DeleteAddress(request);

        [HttpPost]
        [Route("RemoveOrderLine")]
        public async Task<BaseResponse<bool>> RemoveOrderLine([FromBody] OrderLineRequest request) => await _customerService.RemoveOrderLine(request);

        [HttpPost]
        [Route("RemoveOrderHeader")]
        public async Task<BaseResponse<bool>> RemoveOrderHeader([FromBody] OrderLineRequest request) => await _customerService.RemoveOrderHeader(request);

        [HttpPost]
        [Route("CheckCustomerAddress")]
        public async Task<BaseResponse<EN_CustomerAddressResponse>> CheckCustomerAddress([FromBody] EN_CustomerRequest request) => await _customerService.CheckCustomerAddress(request);

        [HttpPost]
        [Route("TakeAllCustomerAddressById")]
        public async Task<BaseResponse<EN_CustomerAddressResponse>> TakeAllCustomerAddressById([FromBody] EN_CustomerRequest request) => await _customerService.TakeAllCustomerAddressById(request);
        [HttpPost]
        [Route("TakeOrderByCustomer")]
        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderByCustomer([FromBody] EN_CustomerRequest request) => await _customerService.TakeOrderByCustomer(request);

        [HttpPost]
        [Route("GetAllNotificationCustomer")]
        public async Task<BaseResponse<EN_CustomerNotificationResponse>> GetAllNotificationCustomer([FromBody] EN_CustomerNotificationRequest request) => await _customerService.GetAllNotificationCustomer(request);

        [HttpPost]
        [Route("CreateNotificationCustomer")]
        public async Task<BaseResponse<bool>> CreateNotificationCustomer([FromBody] EN_CustomerNotificationRequest request) => await _customerService.CreateNotificationCustomer(request);
        [HttpPost]
        [Route("DeleteAllNotification")]
        public async Task<BaseResponse<bool>> DeleteAllNotification([FromBody] EN_CustomerNotificationRequest request) => await _customerService.DeleteAllNotification(request);
        [HttpPost]
        [Route("SetIsReadAllNotification")]
        public async Task<BaseResponse<bool>> SetIsReadAllNotification([FromBody] EN_CustomerNotificationRequest request) => await _customerService.SetIsReadAllNotification(request);

        [HttpPost]
        [Route("PaymentConfirm")]
        public async Task<BaseResponse<bool>> PaymentConfirm([FromBody] PaymentConfirmRequest request) => await _customerService.PaymentConfirm(request);

        [Authorize]
        [HttpPost]
        [Route("RemoveCustomer")]
        public async Task<BaseResponse<bool>> RemoveCustomer(EN_CustomerRequest request) => await _customerService.RemoveCustomer(request);
    }
}