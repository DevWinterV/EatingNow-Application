using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.EN_CustomerService;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
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
        [Route("CreateOrderCustomer")]
        public async Task<BaseResponse<bool>> CreateOrderCustomer([FromBody] EN_CustomerRequest request) => await _customerService.CreateOrderCustomer(request);
    }
}