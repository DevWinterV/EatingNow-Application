using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Shared.ChatRoom;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Employee;
using Falcon.Web.Core.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.ChatRoom
{
    public interface IChatRoomService
    {
        Task<BaseResponse<ChatRoomResponse>> TakeAllChatRoom_ByEmployeeCode(ChatRoomRequest request);
        Task<BaseResponse<EmployeeResponse>> TakeAllUserAddToChatRoom(ChatRoomRequest request);
        Task<BaseResponse<bool>> CreateChatRoom(ChatRoomRequest request);
        Task<BaseResponse<EmployeeResponse>> TakeAllUserInRoomChat(ChatRoomRequest request);
        Task<BaseResponse<bool>> OutRoomChat(ChatRoomRequest request);

        Task<BaseResponse<ChatDescriptionResponse>> TakeAllChatByRoomId(ChatDescriptionRequest request);
        Task<BaseResponse<bool>> CreateChatDescription(ChatDescriptionRequest request);
    }
    public class ChatRoomService : IChatRoomService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly IImageRecordService _imageRecordService;
        public ChatRoomService(DataContext datacontext, IImageRecordService imageRecordServic, ILogService logService)
        {
            _datacontext = datacontext;
            _imageRecordService = imageRecordServic;
            _logService = logService;
        }

        #region ChatRoom
        public async Task<BaseResponse<ChatRoomResponse>> TakeAllChatRoom_ByEmployeeCode(ChatRoomRequest request)
        {
            var result = new BaseResponse<ChatRoomResponse> { };
            try
            {
                var UserName = TokenHelper.CurrentIdentity().UserName;
                var query = from roomMember in _datacontext.FUV_ChatRoomMembers
                            let emp = _datacontext.WMS_Employees.FirstOrDefault(x => x.UserLogin == UserName)
                            let roomCount = _datacontext.FUV_ChatRoomMembers.Count(d => d.RoomId == roomMember.RoomId)
                            let isItMe = roomMember.WMS_Employee.UserLogin == UserName
                            let userNotOwner = _datacontext.FUV_ChatRoomMembers.FirstOrDefault(d => d.RoomId == roomMember.RoomId && d.WMS_Employee.UserLogin != UserName)
                            where roomMember.EmployeeCode == emp.EmployeeCode
                            select new ChatRoomResponse
                            {
                                Id = roomMember.RoomId,
                                UserOwnerId = roomMember.FUV_ChatRoom.UserOwnerId,
                                Uuid = roomMember.FUV_ChatRoom.Uuid,
                                Name = roomCount == 2 ?
                                        isItMe ? 
                                            userNotOwner.WMS_Employee.FullName 
                                            : emp.FullName 
                                        : roomMember.FUV_ChatRoom.Name
                            };
                result.Data = query.ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<EmployeeResponse>> TakeAllUserAddToChatRoom(ChatRoomRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                if (request.Id == 0) {
                    var userName = TokenHelper.CurrentIdentity().UserName;
                    var query = _datacontext.WMS_Employees.AsQueryable().Where(x => x.UserLogin != userName);
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<EmployeeResponse>();
                    foreach (var user in result.Data)
                    {
                        if (user.ImageRecordId.HasValue())
                        {
                            var IdImage = user?.ImageRecordId;
                            user.Avarta = await _imageRecordService.GetImageList(IdImage);
                        }
                    }
                    result.Success = true;
                }
                else
                {
                    var query = _datacontext.FUV_ChatRoomMembers.AsQueryable().Where(x => x.RoomId == request.Id);
                    var data = await query.ToListAsync();
                    var employee = _datacontext.WMS_Employees.AsQueryable();
                    foreach (var emlployeeCode in data)
                    {
                        employee = employee.Where(x => x.EmployeeCode != emlployeeCode.EmployeeCode);
                    }
                    var dataEM = await employee.ToListAsync();
                    result.Data = dataEM.MapTo<EmployeeResponse>();
                    result.Success = true;
                }
                
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateChatRoom(ChatRoomRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if(request.Id == 0)
                {
                    if (request.listUser.Count > 0)
                    {
                        List<string> listEmployeeCode = request.listUser.Select(x => x.EmployeeCode).ToList();
                        var userName = TokenHelper.CurrentIdentity().UserName;
                        var employee = await _datacontext.WMS_Employees.FirstOrDefaultAsync(x => x.UserLogin == userName);
                        if (employee != null)
                        {
                            listEmployeeCode.Add(employee.EmployeeCode);
                        }
                        var newRoom = new FUV_ChatRoom
                        {
                            Uuid = Guid.NewGuid().ToString(),
                            Name = request.Name,
                            UserOwnerId = TokenHelper.CurrentIdentity().UserId,
                            CreatedAt = DateTime.Now,
                            CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        };
                        _datacontext.FUV_ChatRooms.Add(newRoom);
                        await _datacontext.SaveChangesAsync();
                        foreach (var employeeCode in listEmployeeCode)
                        {
                            var newMember = new FUV_ChatRoomMember
                            {
                                RoomId = newRoom.Id,
                                EmployeeCode = employeeCode,
                            };
                            _datacontext.FUV_ChatRoomMembers.Add(newMember);
                            await _datacontext.SaveChangesAsync();
                        }
                        result.Success = true;
                    }
                }
                else
                {
                    List<string> listEmployeeCode = request.listUser.Select(x => x.EmployeeCode).ToList();
                    foreach (var employeeCode in listEmployeeCode)
                    {
                        var newMember = new FUV_ChatRoomMember
                        {
                            RoomId = request.Id,
                            EmployeeCode = employeeCode,
                        };
                        _datacontext.FUV_ChatRoomMembers.Add(newMember);
                        await _datacontext.SaveChangesAsync();
                    }
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<EmployeeResponse>> TakeAllUserInRoomChat(ChatRoomRequest request)
        {
            var result = new BaseResponse<EmployeeResponse> { };
            try
            {
                var query = _datacontext.FUV_ChatRoomMembers.AsQueryable().Where(d => d.RoomId == request.Id);
                var data = await query.OrderBy(x => x.Id).ToListAsync();
                result.Data = data.MapTo<EmployeeResponse>();
                foreach (var user in result.Data)
                {
                    var employee = _datacontext.WMS_Employees.FirstOrDefault(d => d.EmployeeCode == user.EmployeeCode);
                    if (employee != null)
                    {
                        user.FullName = employee.FullName;
                        var IdImage = employee.ImageRecordId.ToString();
                        user.Avarta = await _imageRecordService.GetImageList(IdImage);
                    }
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<bool>> OutRoomChat(ChatRoomRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserLogin = TokenHelper.CurrentIdentity().UserName;
                var employee = await _datacontext.WMS_Employees.FirstOrDefaultAsync(x => x.UserLogin == UserLogin);
                if (employee != null)
                {
                    var member = await _datacontext.FUV_ChatRoomMembers.FirstOrDefaultAsync(x => x.RoomId == request.Id && x.EmployeeCode == employee.EmployeeCode);
                    if (member != null)
                    {
                        _datacontext.FUV_ChatRoomMembers.Remove(member);
                    }
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        #endregion

        #region ChatDecription
        public async Task<BaseResponse<ChatDescriptionResponse>> TakeAllChatByRoomId(ChatDescriptionRequest request)
        {
            var result = new BaseResponse<ChatDescriptionResponse> { };
            try
            {
                var query = _datacontext.FUV_ChatDescriptions.AsQueryable().Where(d => d.RoomId == request.RoomId);
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.OrderBy(x => x.Id).ToListAsync();
                result.Data = data.MapTo<ChatDescriptionResponse>();
                foreach(var user in result.Data)
                {
                    var employee = _datacontext.WMS_Employees.FirstOrDefault(d => d.UserLogin == user.UserName && d.ImageRecordId != null);
                    if (employee != null)
                    {
                        var IdImage = employee.ImageRecordId.ToString();
                        user.Avarta = await _imageRecordService.GetImageList(IdImage);
                    }
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateChatDescription(ChatDescriptionRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var newChat = new FUV_ChatDescriptions
                {
                    RoomId = request.RoomId,
                    UserId = TokenHelper.CurrentIdentity().UserId,
                    ContentText = request.ContentText,
                    CreatedAt = DateTime.Now,
                };
                _datacontext.FUV_ChatDescriptions.Add(newChat);
                var room = _datacontext.FUV_ChatRooms.Find(request.RoomId);
                room.UpdatedAt = DateTime.Now;
                room.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        #endregion

    }
}
