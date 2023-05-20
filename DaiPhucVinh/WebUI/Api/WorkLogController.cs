using DaiPhucVinh.Api;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.MainServices.WorkLog;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.WorkLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/worklog")]
    public class WorkLogController : SecureApiController
    {
        private readonly IWorkLogService _workLogService;
        private readonly ILogService _logService;
        public WorkLogController(IWorkLogService workLogService, ILogService logService)
        {
            _workLogService = workLogService;
            _logService = logService;
        }

        [HttpPost]
        [Route("TakeAllWorkLog")]
        public async Task<BaseResponse<WorkLogResponse>> TakeAllWorkLog([FromBody] WorkLogRequest request) => await _workLogService.TakeAllWorkLog(request);
    }
}