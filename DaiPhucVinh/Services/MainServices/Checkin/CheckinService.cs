using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.Checkin;
using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Checkin
{
    public interface ICheckinService
    {
        Task<BaseResponse<CheckinResponse>> TakeAlls(CheckinRequest request);
        

    }
    public class CheckinService : ICheckinService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly IImageService _imageService;
        private readonly ISetting _settingService;
        public CheckinService(DataContext datacontext, ILogService logService, IImageService imageService, ISetting settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _imageService = imageService;
            _settingService = settingService;
        }
        public async Task<BaseResponse<CheckinResponse>> TakeAlls(CheckinRequest request)
        {
            var result = new BaseResponse<CheckinResponse> { };
            try
            {
                var query = _datacontext.FUV_Checkins.AsQueryable().Where(r => !r.Deleted);
              
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.TimeCheckin).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<CheckinResponse>();
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
