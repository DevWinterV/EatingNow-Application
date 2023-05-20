using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Feedback;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Feedback
{
    public interface IFeedbackService
    {
        Task<BaseResponse<FeedbackResponse>> TakeAllFeedback(FeedbackRequest request, string userType);
        Task<BaseResponse<FeedbackResponse>> TakeFeedbackById(int Id);
        Task<BaseResponse<bool>> ApprovedFeedback(FeedbackRequest request);
        Task<BaseResponse<FeedbackResponse>> FeedbackTakeAll_ByUserId(FeedbackRequest request);
        Task<BaseResponse<bool>> FeedbackCreate(FeedbackRequest request);
        Task<BaseResponse<bool>> FeedbackEdit(FeedbackRequest request);
        Task<BaseResponse<bool>> FeedbackRemove(FeedbackRequest request);
    }
    public class FeedbackService : IFeedbackService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        public FeedbackService(DataContext datacontext, IUserService userService, ILogService logService)
        {
            _datacontext = datacontext;
            _userService = userService;
            _logService = logService;
        }
        public async Task<BaseResponse<FeedbackResponse>> TakeAllFeedback(FeedbackRequest request, string userType)
        {
            var result = new BaseResponse<FeedbackResponse> { };
            try
            {
           
                var query = _datacontext.FUV_Feedbacks.AsQueryable().Where(d => d.User.Roles == userType && !d.Deleted);

                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Title.Contains(request.Term) || d.User.DisplayName.Contains(request.Term));
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
                query = query.OrderByDescending(d => d.CreatedAt).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<FeedbackResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> ApprovedFeedback(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var feedback = await _datacontext.FUV_Feedbacks.FindAsync(request.Id);
                if (feedback != null)
                {
                    feedback.Approved = request.Approved;
                    if (request.Approved)
                    {
                        feedback.ApprovedBy = TokenHelper.CurrentIdentity().UserName;
                    }
                    feedback.ApprovedDescription = request.ApprovedDescription;
                    feedback.UpdatedAt = DateTime.Now;
                    feedback.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<FeedbackResponse>> TakeFeedbackById(int Id)
        {
            var result = new BaseResponse<FeedbackResponse> { };
            try
            {
                var data = await _datacontext.FUV_Feedbacks.FindAsync(Id);
                result.Item = data.MapTo<FeedbackResponse>();

                if (result.Item.ApprovedBy.HasValue())
                {
                    var _user = await _userService.TakeUserByUserName(result.Item.ApprovedBy);
                    result.Item.ApprovedBy = _user.Item.DisplayName;
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

        #region [App]
        public async Task<BaseResponse<FeedbackResponse>> FeedbackTakeAll_ByUserId(FeedbackRequest request)
        {
            var result = new BaseResponse<FeedbackResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Feedbacks.AsQueryable().Where(s => !s.Deleted && s.UserId == UserId);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.UpdatedAt);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.UpdatedAt <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.CreatedAt).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<FeedbackResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> FeedbackCreate(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var dayoffs = _datacontext.FUV_Feedbacks.Add(new FUV_Feedbacks
                {
                    Title = request.Title,
                    Description = request.Description,
                    UserId = TokenHelper.CurrentIdentity().UserId,
                    Approved = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = TokenHelper.CurrentIdentity().UserName,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                    Deleted = false,
                });
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
        public async Task<BaseResponse<bool>> FeedbackEdit(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Feedbacks.FindAsync(request.Id);
                entity.Title = request.Title;
                entity.Description = request.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<bool>> FeedbackRemove(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Feedbacks.FindAsync(request.Id);
                entity.Deleted = true;
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
        #endregion
    }
}