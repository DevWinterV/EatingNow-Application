using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Province;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Province
{
    public interface IProvinceService
    {
        Task<BaseResponse<ProvinceResponse>> TakeAllProvince(ProvinceRequest request);
        Task<BaseResponse<bool>> CreateNewProvince(ProvinceRequest request);
        Task<BaseResponse<bool>> UpdateNewProvince(ProvinceRequest request);
        Task<BaseResponse<bool>> DeleteProvince(ProvinceRequest request);
        Task<BaseResponse<ProvinceResponse>> SearchProvince(string nameProvince);
        Task<BaseResponse<ProvinceResponse>> TakeProvinceById(int Id);
    }
    public class ProvinceService : IProvinceService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public ProvinceService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<ProvinceResponse>> TakeAllProvince(ProvinceRequest request)
        {
            var result = new BaseResponse<ProvinceResponse> { };
            try
            {
                var query = _datacontext.EN_Province.AsQueryable();
                result.DataCount = await query.CountAsync();
                if (request.PageSize != 0)
                {
                    query = query.OrderByDescending(d => d.ProvinceId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                }
                var data = await query.ToListAsync();
                result.Data = data.MapTo<ProvinceResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ProvinceResponse>> TakeProvinceById(int Id)
        {
            var result = new BaseResponse<ProvinceResponse> { };
            try
            {
                var data = await _datacontext.EN_Province.FindAsync(Id);
                result.Item = data.MapTo<ProvinceResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNewProvince(ProvinceRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.ProvinceId == 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var province = new EN_Province
                    {
                        Name = request.Name,
                    };
                    _datacontext.EN_Province.Add(province);
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

        public async Task<BaseResponse<bool>> UpdateNewProvince(ProvinceRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.ProvinceId > 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var province = await _datacontext.EN_Province.FindAsync(request.ProvinceId);
                    {
                        province.Name = request.Name;
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
        public async Task<BaseResponse<bool>> DeleteProvince(ProvinceRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var province = await _datacontext.EN_Province.FirstOrDefaultAsync(x => x.ProvinceId == request.ProvinceId);
                _datacontext.EN_Province.Remove(province);
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
        public async Task<BaseResponse<ProvinceResponse>> SearchProvince(string provinceName)
        {
            var result = new BaseResponse<ProvinceResponse> { };
            try
            {
                var query = await _datacontext.EN_Province.SingleOrDefaultAsync(x => x.Name == provinceName);
                result.Item = query.MapTo<ProvinceResponse>();
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
