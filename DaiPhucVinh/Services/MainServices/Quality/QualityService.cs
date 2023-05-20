using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Quality;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Quality
{
    public interface IQualityService
    {
        Task<BaseResponse<QualityResponse>> TakeAllQuality(QualityRequest request);
    }
    public class QualityService : IQualityService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public QualityService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<QualityResponse>> TakeAllQuality(QualityRequest request)
        {
            var result = new BaseResponse<QualityResponse> { };
            try
            {
                var query = _datacontext.FSW_ChatLuongs.AsQueryable();
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<QualityResponse>();
                result.Success = true;
            }
            catch(Exception ex)
            {
                result.Message = ex.ToString();
               _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<QualityResponse>> TakeQualityByItemCode(QualityRequest request)
        {
            var result = new BaseResponse<QualityResponse> { };
            try
            {
                var query = _datacontext.FSW_ChatLuongs.AsQueryable();
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<QualityResponse>();
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
