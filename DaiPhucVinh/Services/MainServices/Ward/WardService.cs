using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Province;
using DaiPhucVinh.Shared.Ward;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.District
{
    public interface IWardService
    {
        Task<BaseResponse<WardResponse>> TakeAllWard(WardRequest request);
        Task<BaseResponse<bool>> CreateNewWard(WardRequest request);
        Task<BaseResponse<bool>> UpdateNewWard(WardRequest request);
        Task<BaseResponse<bool>> DeleteWard(WardRequest request);
        Task<BaseResponse<WardResponse>> SearchWard(string nameProvince);
        Task<BaseResponse<WardResponse>> TakeWardById(int Id);
    }
    public class WardService : IWardService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public WardService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<WardResponse>> TakeAllWard(WardRequest request)
        {
            var result = new BaseResponse<WardResponse> { };
            try
            {
                var query = _datacontext.EN_Ward.AsQueryable();
                query = query.OrderBy(d => d.Name);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<WardResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<WardResponse>> TakeWardById(int Id)
        {
            var result = new BaseResponse<WardResponse> { };
            try
            {
                var data = await _datacontext.EN_Ward.FindAsync(Id);
                result.Item = data.MapTo<WardResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNewWard(WardRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.WardId == 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var ward = new EN_Ward
                    {
                        Name = request.Name,
                        DistrictId = request.ItemDistrictCode,
                    };
                    _datacontext.EN_Ward.Add(ward);
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> UpdateNewWard(WardRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.WardId > 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var district = await _datacontext.EN_Ward.FindAsync(request.WardId);
                    {
                        district.Name = request.Name;
                    };
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> DeleteWard(WardRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var district = await _datacontext.EN_Ward.FirstOrDefaultAsync(x => x.WardId == request.WardId);
                _datacontext.EN_Ward.Remove(district);
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<WardResponse>> SearchWard(string wardName)
        {
            var result = new BaseResponse<WardResponse> { };
            try
            {
                var query = await _datacontext.EN_Ward.SingleOrDefaultAsync(x => x.Name == wardName);
                result.Item = query.MapTo<WardResponse>();
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
