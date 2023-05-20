using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.UnitOfMeasure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.UnitOfMeasure
{
    public interface IUnitOfMeasureService
    {
        Task<BaseResponse<UnitOfMeasureResponse>> TakeAllUnitOfMeasure(UnitOfMeasureRequest request);
        Task<BaseResponse<UnitOfMeasureResponse>> TakeUnitOfMeasure(string Code);
    }
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public UnitOfMeasureService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<UnitOfMeasureResponse>> TakeAllUnitOfMeasure(UnitOfMeasureRequest request)
        {
            var result = new BaseResponse<UnitOfMeasureResponse> { };
            try
            {
                var query = _datacontext.WMS_UnitOfMeasures.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<UnitOfMeasureResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<UnitOfMeasureResponse>> TakeUnitOfMeasure(string Code)
        {
            var result = new BaseResponse<UnitOfMeasureResponse> { };
            try
            {
                var query = await _datacontext.WMS_Items.FirstOrDefaultAsync(x => x.Code == Code);
                result.Item = query.MapTo<UnitOfMeasureResponse>();
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
