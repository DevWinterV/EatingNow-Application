﻿
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
    public interface ITaskTypeService
    {
        Task<BaseResponse<TaskTypeResponse>> TakeAllTaskTypes(TaskTypeRequest request);
    }
    public class TaskTypeService : ITaskTypeService
    {
        private readonly ILogService _logService;
        private readonly DataContext _datacontext;
        public TaskTypeService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<TaskTypeResponse>> TakeAllTaskTypes(TaskTypeRequest request)
        {
            var result = new BaseResponse<TaskTypeResponse> { };
            try
            {
                var query = _datacontext.FUV_TaskTypes.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.OrderByDescending(d => d.Id).ToListAsync();
                result.Data = data.MapTo<TaskTypeResponse>();
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