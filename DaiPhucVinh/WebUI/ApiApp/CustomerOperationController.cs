using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Customer;
using DaiPhucVinh.Services.MainServices.Product;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Customer;
using DaiPhucVinh.Shared.Feedback;
using DaiPhucVinh.Shared.Notification;
using DaiPhucVinh.Shared.Task;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace PCheck.WebUI.ApiApp
{
    [RoutePrefix("xapi/customerOperation")]
    public class CustomerOperationController : BaseApiController
    {
        private readonly ICustomerService _customerService;
        public CustomerOperationController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        #region [Bảo trì - bảo dưỡng]
        [HttpPost]
        [Route("takeTaskProductByCustomerCode")]
        public async Task<BaseResponse<TaskMobileResponse>> TakeTaskProductByCustomerCode(TaskRequest request) => await _customerService.TakeTaskProductByCustomerCode(request);
        [HttpPost]
        [Route("updateTaskByCustomerCode")]
        public async Task<BaseResponse<bool>> UpdateTaskByCustomerCode(TaskRequest request) => await _customerService.UpdateTaskByCustomerCode(request);
        #endregion

        #region [Feedback of Customer]
        [HttpPost]
        [Route("customerFeedback")]
        public async Task<BaseResponse<FeedbackResponse>> CustomerFeedback_ByUserId(FeedbackRequest request) => await _customerService.CustomerFeedback_ByUserId(request);
        [HttpPost]
        [Route("customerFeedbackCreate")]
        public async Task<BaseResponse<bool>> CustomerFeedback_Create(FeedbackRequest request) => await _customerService.CustomerFeedback_Create(request);
        [HttpPost]
        [Route("customerFeedbackEdit")]
        public async Task<BaseResponse<bool>> CustomerFeedback_Edit(FeedbackRequest request) => await _customerService.CustomerFeedback_Edit(request);
        [HttpPost]
        [Route("customerFeedbackRemove")]
        public async Task<BaseResponse<bool>> CustomerFeedback_Remove(FeedbackRequest request) => await _customerService.CustomerFeedback_Remove(request);
        #endregion

        #region[Thông tin cá nhân]
        [HttpPost]
        [Route("ProfileTakeDetail")]
        public async Task<BaseResponse<CustomerResponse>> ProfileTakeDetail(CustomerRequest request) => await _customerService.ProfileTakeDetail(request);
        [HttpPost]
        [Route("profileEditImage")]
        public async Task<BaseResponse<bool>> ProfileEditImage()
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
                var request = JsonConvert.DeserializeObject<CustomerRequest>(jsonRequest);
                return await _customerService.ProfileEditImage(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        #endregion

    }
}