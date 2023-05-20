using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Notification
{
    public interface INotificationTypeService
    {
        Task<BaseResponse<NotificationTypeResponse>> TakeAllNotificationTypes(NotificationTypeRequest request);
        Task<BaseResponse<bool>> CreateOfUpdateNotificationType(NotificationTypeRequest request);
        Task<BaseResponse<bool>> RemoveNotificationType(NotificationTypeRequest request);
    }
    public class NotificationTypeService : INotificationTypeService
    {
        private readonly ILogService _logService;
        private readonly DataContext _datacontext;
        public NotificationTypeService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<NotificationTypeResponse>> TakeAllNotificationTypes(NotificationTypeRequest request)
        {
            var result = new BaseResponse<NotificationTypeResponse> { };
            try
            {
                var query = _datacontext.FUV_NotificationTypes.AsQueryable().Where(d => d.IsDefault);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.OrderByDescending(d => d.Id).ToListAsync();
                result.Data = data.MapTo<NotificationTypeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateOfUpdateNotificationType(NotificationTypeRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    var notificationType = await _datacontext.FUV_NotificationTypes.SingleOrDefaultAsync(x => x.Id == request.Id);
                    if (notificationType != null)
                    {
                        notificationType.Name = request.Name;
                    }
                }
                else
                {
                    _datacontext.FUV_NotificationTypes.Add(new FUV_NotificationTypes
                    {
                        Name = request.Name,
                        IsDefault = true
                    });
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
        public async Task<BaseResponse<bool>> RemoveNotificationType(NotificationTypeRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_NotificationTypes.SingleOrDefaultAsync(x => x.Id == request.Id);
                if (entity != null)
                {
                    _datacontext.FUV_NotificationTypes.Remove(entity);
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
    }
}
