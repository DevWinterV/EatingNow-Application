using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.PushNotification;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.Notification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Notification
{
    public interface INotificationService
    {
        Task<BaseResponse<NotificationResponse>> TakeAllNotifications(NotificationRequest request);
        Task<BaseResponse<bool>> CreateNotification(NotificationRequest request);
        Task<BaseResponse<bool>> UpdateNotification(NotificationRequest request);
        Task<BaseResponse<bool>> SendNotification(NotificationRequest request);
        Task<BaseResponse<NotificationResponse>> TakeNotificationById(int Id);
        Task<BaseResponse<bool>> RemoveNotification(NotificationRequest request);
        Task<BaseResponse<NotificationResponse>> NotificationTakeAll_ByUserId(NotificationRequest request);
    }
    public class NotificationService : INotificationService
    {
        private readonly DataContext _datacontext;
        private readonly IUserService _userService;
        private readonly ILogService _logService;
        private readonly IPushMessageService _pushMessageService;
        public NotificationService(DataContext datacontext, IUserService userService, ILogService logService, IPushMessageService pushMessageService)
        {
            _datacontext = datacontext;
            _userService = userService;
            _logService = logService;
            _pushMessageService = pushMessageService;
        }
        public async Task<BaseResponse<NotificationResponse>> TakeAllNotifications(NotificationRequest request)
        {
            var result = new BaseResponse<NotificationResponse> { };
            try
            {
                var query = _datacontext.FUV_Notifications.AsQueryable().Where(d => d.FUV_NotificationTypes.IsDefault);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Title.Contains(request.Term));
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.CreatedAt);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreatedAt <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<NotificationResponse>();
                foreach (var notify in result.Data)
                {
                    var _user = await _userService.TakeUserByUserName(notify.CreatedBy);
                    notify.CreatedBy = _user.Item.DisplayName;
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNotification(NotificationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id == 0)
                {

                    _datacontext.FUV_Notifications.Add(new FUV_Notifications
                    {
                        Title = request.Title,
                        Description = request.Description,
                        NotificationTypeId = request.NotificationTypeId,
                        NotificationGroupId = request.NotificationGroupId,
                        CreatedAt = DateTime.Now,
                        CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                        IsSend = false,
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
        public async Task<BaseResponse<bool>> UpdateNotification(NotificationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    //Edit
                    var notify = await _datacontext.FUV_Notifications.SingleOrDefaultAsync(x => x.Id == request.Id);
                    if (notify != null)
                    {
                        notify.NotificationTypeId = request.NotificationTypeId;
                        notify.NotificationGroupId = request.NotificationGroupId;
                        notify.Title = request.Title;
                        notify.Description = request.Description;
                        notify.UpdatedAt = DateTime.Now;
                        notify.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                    }
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
        public async Task<BaseResponse<bool>> SendNotification(NotificationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var notify = await _datacontext.FUV_Notifications.SingleOrDefaultAsync(x => x.Id == request.Id);

                if (notify != null)
                {
                    var mobileToken = _datacontext.FUV_MobileDevice.Where(x => x.Active.HasValue && x.Active.Value).ToList();
                    var listDeviceToken = new List<string>();

                    if (mobileToken.Count() > 0)
                    {
                        listDeviceToken = mobileToken.Select(s => s.DeviceToken).ToList();
                        //Notification Customer
                        if (notify.FUV_NotificationGroups.Target == "AllCustomer")
                        {
                            listDeviceToken = new List<string>();
                            foreach (var token in mobileToken)
                            {
                                var user = await _datacontext.Users.FirstOrDefaultAsync(x => x.Id == token.UserId);

                                if (user != null)
                                {
                                    if (token.User.RoleSystem == "KhachHang")
                                    {
                                        listDeviceToken.Add(token.DeviceToken);
                                    }
                                }
                            }
                            await _pushMessageService.Push(notify.Title, notify.Description, listDeviceToken);
                        }
                        //Notification User
                        if (notify.FUV_NotificationGroups.Target == "AllUser")
                        {
                            await _pushMessageService.Push(notify.Title, notify.Description, listDeviceToken);
                        }
                        //Notification Employee
                        if (notify.FUV_NotificationGroups.Target == "AllEmployee" || notify.FUV_NotificationGroups.Name == "Phòng kỹ thuật")
                        {
                            listDeviceToken = new List<string>();
                            foreach (var token in mobileToken)
                            {
                                var user = await _datacontext.Users.FirstOrDefaultAsync(x => x.Id == token.UserId);
                                if (user != null)
                                {
                                    if (user.RoleSystem == "KyThuatVien")
                                    {
                                        listDeviceToken.Add(token.DeviceToken);
                                    }
                                }
                            }
                            await _pushMessageService.Push(notify.Title, notify.Description, listDeviceToken);
                        }
                    }
                }
                notify.IsSend = true;
                notify.SendAt = DateTime.Now;
                notify.UpdatedAt = DateTime.Now;
                notify.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<NotificationResponse>> TakeNotificationById(int Id)
        {
            var result = new BaseResponse<NotificationResponse> { };
            try
            {
                var data = await _datacontext.FUV_Notifications.FindAsync(Id);
                result.Item = data.MapTo<NotificationResponse>();
                

                var _user = await _userService.TakeUserByUserName(result.Item.CreatedBy);
                result.Item.CreatedBy = _user.Item.DisplayName;

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> RemoveNotification(NotificationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Notifications.SingleOrDefaultAsync(x => x.Id == request.Id);
                if (!request.IsSend)
                {
                    _datacontext.FUV_Notifications.Remove(entity);
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

        #region [App]
        public async Task<BaseResponse<NotificationResponse>> NotificationTakeAll_ByUserId(NotificationRequest request)
        {
            var result = new BaseResponse<NotificationResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Notifications.AsQueryable().Where(s => s.IsSend && s.FUV_NotificationGroups.UserIds.Contains(UserId.ToString()));
                query = query.OrderByDescending(d => d.Id).Take(10);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<NotificationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        #endregion
    }
}
