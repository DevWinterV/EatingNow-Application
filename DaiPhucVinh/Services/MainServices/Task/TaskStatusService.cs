
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Post;
using DaiPhucVinh.Shared.Task;
using DaiPhucVinh.Shared.Video;
using Falcon.Web.Core.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Tasks
{
    public interface ITaskStatusService
    {
        Task<BaseResponse<TaskStatusResponse>> TakeAllTaskStatus(TaskStatusRequest request);
    }
    public class TaskStatusService : ITaskStatusService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public TaskStatusService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<TaskStatusResponse>> TakeAllTaskStatus(TaskStatusRequest request)
        {
            var result = new BaseResponse<TaskStatusResponse> { };
            try
            {
                var query = _datacontext.FUV_TaskStatus.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.OrderBy(d => d.Id).ToListAsync();
                result.Data = data.MapTo<TaskStatusResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
    }
}