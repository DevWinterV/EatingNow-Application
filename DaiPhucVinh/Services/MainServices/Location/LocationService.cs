using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Location;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Location
{
    public interface ILocationService
    {
        Task<BaseResponse<LocationResponse>> TakeAlls(LocationRequest request);
        Task<BaseResponse<LocationResponse>> SearchLocation(string LocationCode);
    }
    public class LocationService : ILocationService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public LocationService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<LocationResponse>> TakeAlls(LocationRequest request)
        {
            var result = new BaseResponse<LocationResponse> { };
            try
            {
                var query = _datacontext.WMS_Locations.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term) || d.Address.Contains(request.Term));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<LocationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<LocationResponse>> SearchLocation(string LocationCode)
        {
            var result = new BaseResponse<LocationResponse> { };
            try
            {
                var query = await _datacontext.WMS_Locations.SingleOrDefaultAsync(x => x.Code == LocationCode);
                result.Item = query.MapTo<LocationResponse>();
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
