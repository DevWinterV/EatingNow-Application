using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.LoaiDanhGia;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.LoaiDanhGia
{
    public interface ILoaiDanhGiaService
    {
        Task<BaseResponse<LoaiDanhGiaResponse>> TakeAllLoaiDanhGia(LoaiDanhGiaRequest request);
    }
    public class LoaiDanhGiaService : ILoaiDanhGiaService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public LoaiDanhGiaService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<LoaiDanhGiaResponse>> TakeAllLoaiDanhGia(LoaiDanhGiaRequest request)
        {
            var result = new BaseResponse<LoaiDanhGiaResponse> { };
            try
            {
                var query = _datacontext.WMS_LoaiDanhGias.AsQueryable();
                query = query.OrderBy(d => d.Name);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<LoaiDanhGiaResponse>();
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
