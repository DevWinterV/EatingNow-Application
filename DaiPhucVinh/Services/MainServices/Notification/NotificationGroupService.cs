using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
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
    public interface INotificationGroupService
    {
        Task<BaseResponse<NotificationGroupResponse>> TakeAllNotificationGroups(NotificationGroupRequest request);
        Task<BaseResponse<bool>> CreateNotificationGroup(NotificationGroupRequest request);
        Task<BaseResponse<bool>> UpdateNotificationGroup(NotificationGroupRequest request, string EmployeeCode);
        Task<BaseResponse<bool>> AddNameNotificationGroup(NotificationGroupRequest request);
        Task<BaseResponse<NotificationGroupResponse>> TakeNotificationGroupById(int Id);
        Task<BaseResponse<bool>> RemoveNotificationGroup(NotificationGroupRequest request);
        Task<BaseResponse<bool>> DeletedEmployeeNotificationRequest(NotificationGroupRequest request, string EmployeeCode);
    }
    public class NotificationGroupService : INotificationGroupService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public NotificationGroupService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<NotificationGroupResponse>> TakeAllNotificationGroups(NotificationGroupRequest request)
        {
            var result = new BaseResponse<NotificationGroupResponse> { };
            try
            {
                var query = _datacontext.FUV_NotificationGroups.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<NotificationGroupResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<NotificationGroupResponse>> TakeNotificationGroupById(int Id)
        {
            var result = new BaseResponse<NotificationGroupResponse> { };
            try
            {
                var data = await _datacontext.FUV_NotificationGroups.FindAsync(Id);
                result.Item = data.MapTo<NotificationGroupResponse>();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNotificationGroup(NotificationGroupRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id == 0)
                {
                    var targetName = "Orther";
                    var userTarget = "";
                    List<string> userIdList = new List<string> { };
                    foreach (var userItem in request.dataItemCheck)
                    {
                        userIdList.Add(userItem.EmployeeCode);
                    }
                    userTarget = string.Join(",", userIdList);
                    _datacontext.FUV_NotificationGroups.Add(new FUV_NotificationGroups
                    {
                        Name = request.Name,
                        Target = targetName,
                        UserIds = userTarget,
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
        public async Task<BaseResponse<bool>> UpdateNotificationGroup(NotificationGroupRequest request, string EmployeeCode)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    //Edit
                    var notificationGroup = await _datacontext.FUV_NotificationGroups.SingleOrDefaultAsync(x => x.Id == request.Id);
                    if (notificationGroup != null)
                    {
                        if (request.UserIds.IsNullOrEmpty())
                        {
                            notificationGroup.UserIds += EmployeeCode;

                        }
                        else
                        {
                            notificationGroup.UserIds += "," + EmployeeCode;
                        }
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
        public async Task<BaseResponse<bool>> AddNameNotificationGroup(NotificationGroupRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    var targetName = "Orther";
                    //Edit
                    var notificationGroup = await _datacontext.FUV_NotificationGroups.SingleOrDefaultAsync(x => x.Id == request.Id);
                    if (notificationGroup != null)
                    {
                        notificationGroup.Name = request.Name;
                        notificationGroup.Target = targetName;
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
        public async Task<BaseResponse<bool>> RemoveNotificationGroup(NotificationGroupRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_NotificationGroups.SingleOrDefaultAsync(x => x.Id == request.Id);
                if (entity != null)
                {
                    _datacontext.FUV_NotificationGroups.Remove(entity);
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
        public async Task<BaseResponse<bool>> DeletedEmployeeNotificationRequest(NotificationGroupRequest request, string EmployeeCode)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var query = await _datacontext.FUV_NotificationGroups.SingleOrDefaultAsync(n => n.Id == request.Id);
                string EmployeeId = "";
                List<string> EmployeeListId = new List<string>();
                List<string> ArrId = new List<string>();
                EmployeeListId = query.UserIds.Split(',')?.ToList();
                foreach (var listId in EmployeeListId)
                {
                    if (listId != EmployeeCode)
                    {
                        ArrId.Add(listId);
                    }
                    EmployeeId = string.Join(",", ArrId);
                }
                query.UserIds = EmployeeId;
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
