using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.EN_CustomerService;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
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

    }
}