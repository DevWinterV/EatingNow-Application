using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Shared.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Common
{
    public interface ICommonService
    {
        Task<string> AutoGencode(string tableName);
        Task<BaseResponse<MobileDeviceDto>> SaveDeviceToken(MobileTokenRequest request);
    }

    public  class CommonService : ICommonService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public CommonService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<string> AutoGencode(string tableName)
        {
            var config = await _datacontext.WMS_AutoGenCodeConfigs.FirstOrDefaultAsync(d => d.TableName == tableName);
            if (config == null) return string.Empty;
            var now = DateTime.Today;
            string newCode = string.Empty;
            newCode = $"{config.Prefix}-";
            if (config.IsDay.HasValue && config.IsDay.Value)
                newCode += now.Day.ToString("00");
            if (config.IsMonth.HasValue && config.IsMonth.Value)
                newCode += now.Month.ToString("00");
            if (config.IsYear.HasValue && config.IsYear.Value)
                newCode += $"{now:yy}-";

            newCode += (config.CurrentCode + 1).ToString().PadLeft(config.AutonumberLenght ?? 0, '0');
            //update config
            config.CurrentCode++;
            await _datacontext.SaveChangesAsync();
            return newCode;
        }
        #region Save Token Device
        public async Task<BaseResponse<MobileDeviceDto>> SaveDeviceToken(MobileTokenRequest request)
        {
            var result = new BaseResponse<MobileDeviceDto> { Success = false };
            try
            {
                var queryToken = _datacontext.FUV_MobileDevice.AsQueryable();

                if (request == null)
                {
                    result.Message = "Không tìm thấy điều kiện";
                    return result;
                }

                if (request.UserId > 0)
                {
                    var mobileToken = await queryToken.FirstOrDefaultAsync(x => x.UserId == request.UserId);

                    if (mobileToken == null)
                    {
                        var dataToken = new FUV_MobileDevices
                        {
                            UserId = request.UserId,
                            PhoneOs = request.PhoneOs,
                            DeviceToken = request.DeviceToken,
                            LastActive = DateTime.Now,
                            Active = true,
                        };
                        _datacontext.FUV_MobileDevice.Add(dataToken);
                        await _datacontext.SaveChangesAsync();
                    }
                    else
                    {
                        mobileToken.DeviceToken = request.DeviceToken;
                        mobileToken.PhoneOs = request.PhoneOs;
                        mobileToken.UserId = request.UserId;
                        mobileToken.LastActive = DateTime.Now;
                        await _datacontext.SaveChangesAsync();
                    }
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                result.Message = ex.ToString();
#else
                result.Message = ex.Message;
#endif
                _logService.InsertLog(ex);
            }
            return result;
        }
        #endregion
    }
}
