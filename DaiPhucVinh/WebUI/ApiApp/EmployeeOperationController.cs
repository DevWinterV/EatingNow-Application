using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Attendances;
using DaiPhucVinh.Services.MainServices.ChatRoom;
using DaiPhucVinh.Services.MainServices.Checkin;
using DaiPhucVinh.Services.MainServices.Dayoffs;
using DaiPhucVinh.Services.MainServices.Employee;
using DaiPhucVinh.Services.MainServices.Feedback;
using DaiPhucVinh.Services.MainServices.Notification;
using DaiPhucVinh.Services.MainServices.Report;
using DaiPhucVinh.Services.MainServices.Tasks;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.ChatRoom;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Dayoffs;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.Feedback;
using DaiPhucVinh.Shared.Notification;
using DaiPhucVinh.Shared.Report;
using DaiPhucVinh.Shared.Task;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DaiPhucVinh.ApiApp
{
    [RoutePrefix("xapi/employeeOperation")]
    public class EmployeeOperationController : SecureApiController
    {
        private readonly IDayoffsService _dayoffsService;
        private readonly IFeedbackService _feedbackService;
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;
        private readonly IEmployeeService _employeeService;
        private readonly IAttendancesService _attendancesService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IReportService _reportService;
        public EmployeeOperationController(
            IDayoffsService dayoffsService, 
            IFeedbackService feedbackService, 
            ITaskService taskService, 
            INotificationService notificationService,
            IEmployeeService employeeService,
            IAttendancesService attendancesService,
            IChatRoomService chatRoomService,
            IReportService reportService
            )
        {
            _dayoffsService = dayoffsService;
            _feedbackService = feedbackService;
            _taskService = taskService;
            _notificationService = notificationService;
            _employeeService = employeeService;
            _attendancesService = attendancesService;
            _chatRoomService = chatRoomService;
            _reportService = reportService;
        }

        #region[Chấm công]
        [HttpPost]
        [Route("DailyCheck")]
        public async Task<BaseResponse<bool>> DailyCheck() => await _attendancesService.DailyCheck();
        [HttpPost]
        [Route("AttendancesCheckInTakeDetail")]
        public async Task<BaseResponse<AttendancesResponse>> AttendancesCheckInTakeDetail([FromBody] AttendancesRequest request) => await _attendancesService.AttendancesCheckInTakeDetail(request);

        #region Chấm công vào
        [HttpPost]
        [Route("AttendancesCheckIn")]
        public async Task<BaseResponse<bool>> AttendancesCheckIn()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;
            HttpPostedFile img1 = httpRequest.Files.Count > 1 ? httpRequest.Files[1] : null;
            HttpPostedFile img2 = httpRequest.Files.Count > 2 ? httpRequest.Files[2] : null;
            List<HttpPostedFile> imgs = new List<HttpPostedFile>
            {
                img,
                img1,
                img2
            };
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<AttendancesRequest>(jsonRequest);
                return await _attendancesService.AttendancesCheckIn(request, imgs);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        #endregion

        #region Chấm công ra
        [HttpPost]
        [Route("AttendancesCheckOut")]
        public async Task<BaseResponse<bool>> AttendancesCheckOut()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;
            HttpPostedFile img1 = httpRequest.Files.Count > 1 ? httpRequest.Files[1] : null;
            HttpPostedFile img2 = httpRequest.Files.Count > 2 ? httpRequest.Files[2] : null;
            List<HttpPostedFile> imgs = new List<HttpPostedFile>
            {
                img,
                img1,
                img2
            };
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<AttendancesRequest>(jsonRequest);
                return await _attendancesService.AttendancesCheckOut(request, imgs);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        #endregion

        #region Thống kê chấm công
        [HttpPost]
        [Route("timekeepingTakeDetail")]
        public async Task<BaseResponse<AttendancesResponse>> TimeKeepingTakeDetail(AttendancesRequest request) => await _attendancesService.TimeKeepingTakeAll_ByUserLogin(request);
        [HttpPost]
        [Route("TakeAttendancesByUserId")]
        public async Task<BaseResponse<AttendancesResponse>> TakeAllAttendancesByUserId(AttendancesRequest request) => await _attendancesService.TakeAllAttendancesByUserId(request);
        [HttpPost]
        [Route("EditNoteAttendances")]
        public async Task<BaseResponse<bool>> EditNoteAttendances(AttendancesRequest request) => await _attendancesService.EditNoteAttendances(request);
        #endregion

        #endregion

        #region [Thống kê công việc]
        [HttpPost]
        [Route("ReportTakeAlls")]
        public async Task<ReportResponse> ReportTakeAlls([FromBody] ReportRequest request) => await _reportService.ReportTakeAlls(request);

        #endregion

        #region [Nghỉ phép]
        [HttpPost]
        [Route("dayoffTakeAlls")]
        public async Task<BaseResponse<DayoffsResponse>> DayoffTakeAlls(DayoffsRequest request) => await _dayoffsService.TakeAllDayOff_ByUserId(request);
        [HttpPost]
        [Route("dayoffCreate")]
        public async Task<BaseResponse<bool>> DayoffCreate(DayoffsRequest request) => await _dayoffsService.CreateDayOff(request);
        [HttpPost]
        [Route("dayoffEdit")]
        public async Task<BaseResponse<bool>> DayoffEdit(DayoffsRequest request) => await _dayoffsService.EditDayOff(request);
        [HttpPost]
        [Route("dayoffRemove")]
        public async Task<BaseResponse<bool>> DayoffRemove(DayoffsRequest request) => await _dayoffsService.RemoveDayOff(request);
        #endregion

        #region [Đề xuất ý kiến]
        [HttpPost]
        [Route("feedbackTakeAlls")]
        public async Task<BaseResponse<FeedbackResponse>> FeedbackTakeAlls([FromBody] FeedbackRequest request) => await _feedbackService.FeedbackTakeAll_ByUserId(request);
        [HttpPost]
        [Route("feedbackCreate")]
        public async Task<BaseResponse<bool>> FeedbackCreate([FromBody] FeedbackRequest request) => await _feedbackService.FeedbackCreate(request);
        [HttpPost]
        [Route("feedbackEdit")]
        public async Task<BaseResponse<bool>> FeedbackEdit([FromBody] FeedbackRequest request) => await _feedbackService.FeedbackEdit(request);
        [HttpPost]
        [Route("feedbackRemove")]
        public async Task<BaseResponse<bool>> FeedbackRemove([FromBody] FeedbackRequest request) => await _feedbackService.FeedbackRemove(request);


        #endregion

        #region[Thông tin cá nhân]
        [HttpPost]
        [Route("ProfileTakeDetail")]
        public async Task<BaseResponse<EmployeeResponse>> ProfileTakeDetail(EmployeeRequest request) => await _employeeService.ProfileTakeDetail_ByUserLogin(request);
        [HttpPost]
        [Route("ProfileEditImage")]
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
                var request = JsonConvert.DeserializeObject<EmployeeRequest>(jsonRequest);
                return await _employeeService.ProfileEdit_ByUserLogin(request, img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        #endregion

        #region[Công việc]
        [HttpPost]
        [Route("taskTakeAll")]
        public async Task<BaseResponse<TaskMobileResponse>> TakeAllTask([FromBody] TaskRequest request) => await _taskService.TakeAllTask_ByUserId(request);
        [HttpPost]
        [Route("taskTakeDetail")]
        public async Task<BaseResponse<TaskResponse>> TakeTaskDetail([FromBody] TaskRequest request) => await _taskService.TakeTaskDetail_ById(request);
        [HttpPost]
        [Route("taskEditCheckIn")]
        public async Task<BaseResponse<bool>> taskEditCheckIn()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;
            HttpPostedFile img1 = httpRequest.Files.Count > 1 ? httpRequest.Files[1] : null;
            HttpPostedFile img2 = httpRequest.Files.Count > 2 ? httpRequest.Files[2] : null;
            List<HttpPostedFile> imgs = new List<HttpPostedFile>
            {
                img,
                img1,
                img2
            };
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<TaskRequest>(jsonRequest);
                return await _taskService.TaskCheckIn_ById(request, imgs);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("taskEditDetail_Before")]
        public async Task<BaseResponse<bool>> taskEditDetail_Before()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;
            HttpPostedFile img1 = httpRequest.Files.Count > 1 ? httpRequest.Files[1] : null;
            HttpPostedFile img2 = httpRequest.Files.Count > 2 ? httpRequest.Files[2] : null;
            HttpPostedFile img3 = httpRequest.Files.Count > 3 ? httpRequest.Files[3] : null;
            HttpPostedFile img4 = httpRequest.Files.Count > 4 ? httpRequest.Files[4] : null;
            HttpPostedFile img5 = httpRequest.Files.Count > 5 ? httpRequest.Files[5] : null;
            List<HttpPostedFile> imgs = new List<HttpPostedFile>
            {
                img,
                img1,
                img2,
                img3,
                img4,
                img5,
            };
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<TaskRequest>(jsonRequest);
                return await _taskService.taskEditDetail_Before(request, imgs);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("taskEditDetail_After")]
        public async Task<BaseResponse<bool>> taskEditDetail_After()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;
            HttpPostedFile img1 = httpRequest.Files.Count > 1 ? httpRequest.Files[1] : null;
            HttpPostedFile img2 = httpRequest.Files.Count > 2 ? httpRequest.Files[2] : null;
            HttpPostedFile img3 = httpRequest.Files.Count > 3 ? httpRequest.Files[3] : null;
            HttpPostedFile img4 = httpRequest.Files.Count > 4 ? httpRequest.Files[4] : null;
            HttpPostedFile img5 = httpRequest.Files.Count > 5 ? httpRequest.Files[5] : null;
            List<HttpPostedFile> imgs = new List<HttpPostedFile>
            {
                img,
                img1,
                img2,
                img3,
                img4,
                img5,
            };
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<TaskRequest>(jsonRequest);
                return await _taskService.taskEditDetail_After(request, imgs);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("taskEditDetail_Document")]
        public async Task<BaseResponse<bool>> taskEditDetail_Document()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = httpRequest.Files.Count > 0 ? httpRequest.Files[0] : null;
            HttpPostedFile img1 = httpRequest.Files.Count > 1 ? httpRequest.Files[1] : null;
            HttpPostedFile img2 = httpRequest.Files.Count > 2 ? httpRequest.Files[2] : null;
            List<HttpPostedFile> imgs = new List<HttpPostedFile>
            {
                img,
                img1,
                img2,
            };
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<TaskRequest>(jsonRequest);
                return await _taskService.taskEditDetail_Document(request, imgs);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }

        #endregion

        #region [Chat]

        #region ChatRoom
        [HttpPost]
        [Route("TakeAllChatRooms")]
        public async Task<BaseResponse<ChatRoomResponse>> TakeAllRooms([FromBody] ChatRoomRequest request) => await _chatRoomService.TakeAllChatRoom_ByEmployeeCode(request);
        [HttpPost]
        [Route("TakeAllUserAddToChatRoom")]
        public async Task<BaseResponse<EmployeeResponse>> TakeAllUserEmployeeAddToChatRoom([FromBody] ChatRoomRequest request) => await _chatRoomService.TakeAllUserAddToChatRoom(request);
        [HttpPost]
        [Route("CreateChatRoom")]
        public async Task<BaseResponse<bool>> CreateChatRoom([FromBody] ChatRoomRequest request) => await _chatRoomService.CreateChatRoom(request);
        [HttpPost]
        [Route("TakeAllUserInRoomChat")]
        public async Task<BaseResponse<EmployeeResponse>> TakeAllUserInRoomChat([FromBody] ChatRoomRequest request) => await _chatRoomService.TakeAllUserInRoomChat(request);
        [HttpPost]
        [Route("OutRoomChat")]
        public async Task<BaseResponse<bool>> OutRoomChat([FromBody] ChatRoomRequest request) => await _chatRoomService.OutRoomChat(request);
        #endregion

        #region ChatDecription
        [HttpPost]
        [Route("TakeAllChatByRoomId")]
        public async Task<BaseResponse<ChatDescriptionResponse>> TakeAllChatByRoomId([FromBody] ChatDescriptionRequest request) => await _chatRoomService.TakeAllChatByRoomId(request);
        [HttpPost]
        [Route("CreateChatDescription")]
        public async Task<BaseResponse<bool>> CreateChatDescription([FromBody] ChatDescriptionRequest request) => await _chatRoomService.CreateChatDescription(request);
        #endregion

        #endregion

        #region[Thông báo]
        [HttpPost]
        [Route("NotificationTakeAll")]
        public async Task<BaseResponse<NotificationResponse>> NotificationTakeAll(NotificationRequest request) => await _notificationService.NotificationTakeAll_ByUserId(request);

        #endregion

    }
}    