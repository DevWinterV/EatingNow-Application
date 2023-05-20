
using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Post;
using Falcon.Web.Core.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Post
{
    public interface IPostService
    {
        Task<BaseResponse<PostResponse>> TakeAllPosts(PostRequest request);
        Task<BaseResponse<PostResponse>> TakeAllNews(PostRequest request);
        Task<BaseResponse<bool>> CreatePost(PostRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> UpdatePost( PostRequest request, HttpPostedFile file);
        Task<BaseResponse<bool>> RemovePost(PostRequest request);
        Task<BaseResponse<PostResponse>> TakePostById(int Id);
		Task<BaseResponse<PostResponse>> TakeImagePostById(int Id);
		
		#region Mobile
        Task<BaseResponse<PostMobileResponse>> TakeAllPostsForMobile(PostMobileRequest request);
        #endregion
    }
    public class PostService : IPostService
    {
        private readonly DataContext _datacontext;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly ISetting _settingService;
        private readonly ILogService _logService;
        public PostService(DataContext datacontext, IUserService userService, IImageService imageService, ISetting settingService, ILogService logService)
        {
            _datacontext = datacontext;

            _userService = userService;
            _imageService = imageService;
            _settingService = settingService;
            _logService = logService;
        }
        public async Task<BaseResponse<PostResponse>> TakeAllPosts(PostRequest request)
        {
            var result = new BaseResponse<PostResponse> { };
            try
            {
                var query = _datacontext.FUV_Posts.AsQueryable().Where(d => !d.Deleted);
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
                result.Data = data.MapTo<PostResponse>();
                foreach (var post in result.Data)
                {
                    var _user = await _userService.TakeUserByUserName(post.CreatedBy);
                    post.CreatedBy = _user.Item.DisplayName;
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
        public async Task<BaseResponse<PostResponse>> TakeAllNews(PostRequest request)
        {
            var result = new BaseResponse<PostResponse> { };
            try
            {
                var query = _datacontext.FUV_Posts.AsQueryable().Where(r => !r.Deleted);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Title.Contains(request.Term));
                }
                
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.CreatedAt).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<PostResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreatePost(PostRequest request, HttpPostedFile file)
         {
            var result = new BaseResponse<bool> { };
            try
            {
                var post = new FUV_Posts
                {
                    Title = request.Title,
                    ShortTitle = request.ShortTitle,
                    Description = request.Description,
                    CreatedAt = DateTime.Now,
                    CreatedBy = TokenHelper.CurrentIdentity().UserName,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                    Deleted = false,
                };
                if (file != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        var Images = await _imageService.InsertImage(ms.GetBuffer(), file.FileName, "DaiPhucVinh\\Image", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                        post.ImageId = Images.Image.Id;
                    }
                }
                      
                _datacontext.FUV_Posts.Add(post);
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
        public async Task<BaseResponse<bool>> UpdatePost( PostRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var query = await _datacontext.FUV_Posts.SingleOrDefaultAsync(n => n.Id == request.Id);
                 query.Title = request.Title;
                query.ShortTitle = request.ShortTitle;
                query.Description = request.Description;
                query.UpdatedAt = DateTime.Now;
                query.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                if (file != null)
                {
                    HttpPostedFile file_Img = null;
                    if (_imageService.CheckImageFileType(file.FileName))
                    {
                        file_Img = file;
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file_Img.InputStream.CopyTo(ms);
                        var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                        query.ImageId = Images.Image.Id;
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
        public async Task<BaseResponse<bool>> RemovePost(PostRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var post = await _datacontext.FUV_Posts.SingleOrDefaultAsync(x => x.Id == request.Id);
                post.Deleted = true;
                post.UpdatedBy = TokenHelper.CurrentIdentity().DisplayName;
                post.UpdatedAt = DateTime.Now;
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
        public async Task<BaseResponse<PostResponse>> TakePostById(int Id)
        {
            var result = new BaseResponse<PostResponse> { };
            try
            {
                var data = await _datacontext.FUV_Posts.FindAsync(Id);
                result.Item = data.MapTo<PostResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
		public async Task<BaseResponse<PostResponse>> TakeImagePostById(int Id)
        {
            var result = new BaseResponse<PostResponse> { };
            try
            {

                var data = await _datacontext.FUV_Posts.FindAsync(Id);
                result.Item = data.MapTo<PostResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
		#region Mobile

        #region Take Post For Mobile
        public async Task<BaseResponse<PostMobileResponse>> TakeAllPostsForMobile(PostMobileRequest request)
        {
            var result = new BaseResponse<PostMobileResponse> { };
            try
            {
                var query = _datacontext.FUV_Posts.AsQueryable().Where(d => !d.Deleted);

                if (request.Term.HasValue() && request.Term != "")
                {
                    query = query.Where(d => d.Title.Contains(request.Term));
                }

                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);

                var data = await query.Select(s => new PostMobileResponse
                {
                    Id = s.Id,
                    Title = s.Title,
                    ShortTitle = s.ShortTitle,
                    Description = s.Description,
                    ImagePath = _datacontext.ImageRecords.FirstOrDefault(x => x.Id == s.ImageId).AbsolutePath,
                    UpdatedBy = _datacontext.Users.FirstOrDefault(d => d.UserName.ToLower().Equals(s.UpdatedBy.ToLower())).DisplayName,
                    UpdatedAt = s.UpdatedAt.Value.Day.ToString() + "/" + s.UpdatedAt.Value.Month.ToString() + "/" + s.UpdatedAt.Value.Year.ToString()
                }).ToListAsync();

                result.Data = data;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        #endregion

        #endregion
    }

}