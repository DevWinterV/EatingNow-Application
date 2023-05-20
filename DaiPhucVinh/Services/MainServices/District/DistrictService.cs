using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.District;
using DaiPhucVinh.Shared.Province;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.District
{
    public interface IDistrictService
    {
        Task<BaseResponse<DistrictResponse>> TakeAllDistrict(DistrictRequest request);
        Task<BaseResponse<bool>> CreateNewDistrict(DistrictRequest request);
        Task<BaseResponse<bool>> UpdateNewDistrict(DistrictRequest request);
        Task<BaseResponse<bool>> DeleteDistrict(DistrictRequest request);
        Task<BaseResponse<DistrictResponse>> SearchDistrict(string nameProvince);
        Task<BaseResponse<DistrictResponse>> TakeDistrictById(int Id);
    }
    public class DistrictService : IDistrictService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public DistrictService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<DistrictResponse>> TakeAllDistrict(DistrictRequest request)
        {
            var result = new BaseResponse<DistrictResponse> { };
            try
            {
                var query = _datacontext.EN_District.AsQueryable();
                query = query.OrderBy(d => d.Name);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<DistrictResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<DistrictResponse>> TakeDistrictById(int Id)
        {
            var result = new BaseResponse<DistrictResponse> { };
            try
            {
                var data = await _datacontext.EN_District.FindAsync(Id);
                result.Item = data.MapTo<DistrictResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNewDistrict(DistrictRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.DistrictId == 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var district = new EN_District
                    {
                        Name = request.Name,
                        ProvinceId = request.ItemProvinceCode,
                    };
                    _datacontext.EN_District.Add(district);
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

        public async Task<BaseResponse<bool>> UpdateNewDistrict(DistrictRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.DistrictId > 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var district = await _datacontext.EN_District.FindAsync(request.DistrictId);
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
        public async Task<BaseResponse<bool>> DeleteDistrict(DistrictRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var district = await _datacontext.EN_District.FirstOrDefaultAsync(x => x.DistrictId == request.DistrictId);
                _datacontext.EN_District.Remove(district);
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
        public async Task<BaseResponse<DistrictResponse>> SearchDistrict(string districtName)
        {
            var result = new BaseResponse<DistrictResponse> { };
            try
            {
                var query = await _datacontext.EN_District.SingleOrDefaultAsync(x => x.Name == districtName);
                result.Item = query.MapTo<DistrictResponse>();
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
