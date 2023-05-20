using DaiPhucVinh.Api;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.MainServices.Attendances;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.Common;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/attendances")]
    public class AttendancesController : SecureApiController
    {
        private readonly IAttendancesService _attendancesService;
        private readonly ILogService _logService;
        public AttendancesController(IAttendancesService attendancesService, ILogService logService)
        {
            _attendancesService = attendancesService;
            _logService = logService;
        }

        [HttpPost]
        [Route("TakeAlls")]
        public async Task<BaseResponse<AttendancesResponse>> TakeAlls([FromBody] AttendancesRequest request) => await _attendancesService.TakeAlls(request );
        [HttpPost]
        [Route("NoteReply")]
        public async Task<BaseResponse<bool>> NoteReply([FromBody] AttendancesRequest request) => await _attendancesService.NoteReply(request);
        [HttpGet]
        [Route("TakeAttendanceById")]
        public async Task<BaseResponse<AttendancesResponse>> TakeAttendanceById(int Id) => await _attendancesService.TakeAttendanceById(Id);
        #region Export excel
        [HttpPost]
        [Route("ExportAttendances")]
        public async Task<BaseResponse<string>> ExportAttendances([FromBody] AttendancesRequest request)
        {
            var result = new BaseResponse<string> { Success = false };
            try
            {
                string templateName = System.Web.HttpContext.Current.Server.MapPath("~/Exports/ExcelTemplate/ExcelTemplate_DanhSachQuanLyChamCong.xlsx");
                result.Item = await _attendancesService.ExportAttendances(templateName, request);
                result.Success = true;
            }
            catch (Exception exx)
            {
                result.Message = exx.Message;
                _logService.InsertLog(exx);
            }
            return result;
        }
        #endregion
    }
}