using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.ImageRecords;
using DaiPhucVinh.Shared.ProjectImages;
using Falcon.Core;
using Falcon.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace DaiPhucVinh.Services.MainServices.ProjectImages
{
    public interface IProjectImagesService
    {
        Task<BaseResponse<ProjectImagesResponse>> TakeAllProjectImages(ProjectImageRequest request);
        Task<BaseResponse<ProjectImagesResponse>> Deleted(ProjectImageRequest request);
        Task<BaseResponse<int>> CreateProjectImage(ProjectImageRequest request);
        Task<BaseResponse<bool>> UpdateProjectImage(ProjectImageRequest request);
        Task<BaseResponse<bool>> UpdateFileAttachProjectImageRequest(int id,HttpPostedFile fileImg);
        Task<BaseResponse<bool>> AddNewFileAttachProjectImageRequest(ProjectImageRequest request, HttpRequest httpRequest);
        Task<BaseResponse<bool>> FileAttachProjectImageRequest(ProjectImageRequest request, HttpRequest httpRequest);
        Task<BaseResponse<bool>> DeletedAllFileAttachProjectImageRequest(ProjectImageRequest request);
        Task<BaseResponse<bool>> DeletedFileAttachProjectImageRequest(ProjectImageRequest request, int Idimg);
        Task<BaseResponse<ProjectImagesResponse>> TakeProjectImageById(int Id);

        #region Mobile
        Task<BaseResponse<ProjectImagesResponse>> TakeAllProjectImagesForMobile(ProjectImageRequest request);
        #endregion
    }
    public class ProjectImagesService : IProjectImagesService
    {
        private readonly DataContext _datacontext;
        private readonly IImageService _imageservice;
        private readonly ISetting _settingService;
        private readonly IUserService _userService;
        private readonly ILogService _logService;
        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        public ProjectImagesService(DataContext datacontext, IImageService imageservice, ISetting settingService, IUserService userService, ILogService logService)
        {
            _datacontext = datacontext;
            _imageservice = imageservice;
            _settingService = settingService;
            _userService = userService;
            _logService = logService;
        }

        #region Web Admin
        public async Task<BaseResponse<ProjectImagesResponse>> TakeAllProjectImages(ProjectImageRequest request)
        {
            var result = new BaseResponse<ProjectImagesResponse> { };
            try
            {
                var query = _datacontext.fUV_ProjectImages.AsQueryable().Where(d => !d.Deleted);
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
                result.Data = data.MapTo<ProjectImagesResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ProjectImagesResponse>> Deleted(ProjectImageRequest request)
        {
            var result = new BaseResponse<ProjectImagesResponse> { };
            try
            {
                var query = await _datacontext.fUV_ProjectImages.FindAsync(request.Id);
                query.Deleted = true;
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
        public async Task<BaseResponse<int>> CreateProjectImage( ProjectImageRequest request)
        {
            var result = new BaseResponse<int> { };
            try
            {
                var entity = new FUV_ProjectImages
                {
                    Title = request.Title,
                    ShortTitle = request.ShortTitle,
                    Description = request.Description,
                    Images = string.Empty,
                    CreatedAt = DateTime.Now,
                    CreatedBy = TokenHelper.CurrentIdentity().UserName,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                    Deleted = false,
                };
                _datacontext.fUV_ProjectImages.Add(entity);
                await _datacontext.SaveChangesAsync();
                result.Success = true;
                result.Item = entity.Id;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateProjectImage(ProjectImageRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                    var query = await _datacontext.fUV_ProjectImages.SingleOrDefaultAsync(n => n.Id == request.Id);
                    query.Title = request.Title;
                    query.ShortTitle = request.ShortTitle;
                    query.Description = request.Description;
                    query.UpdatedAt = DateTime.Now;
                    query.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                    query.Deleted = false;
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
        public async Task<BaseResponse<bool>> AddNewFileAttachProjectImageRequest(ProjectImageRequest request, HttpRequest httpRequest)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                string ImgId = "";
                List<string> imagesList = new List<string>();
                List<int> imagesListId = new List<int>();
                foreach (string file in httpRequest.Files)
                {
                    HttpPostedFile file_post = null;
                    file_post = httpRequest.Files[file];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file_post.InputStream.CopyTo(ms);
                        var Images = await _imageservice.InsertImage(ms.GetBuffer(), file_post.FileName, "DaiPhucVinh\\Image", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageservice.ResizeImage(Images.Image.Id, Int32.Parse(setting.Photomaxwidth));
                        imagesListId.Add(Images.Image.Id);
                    }
                    ImgId = string.Join(",", imagesListId);
                }
                var query = await _datacontext.fUV_ProjectImages.SingleOrDefaultAsync(n => n.Id == request.Id);
                query.Images += "," + ImgId;
                query.UpdatedAt = DateTime.Now;
                query.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<bool>> FileAttachProjectImageRequest(ProjectImageRequest request, HttpRequest httpRequest)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                string ImgId = "";
                List<string> imagesList = new List<string>();
                List<int> imagesListId = new List<int>();
                foreach (string file in httpRequest.Files)
                {
                    HttpPostedFile file_Img = null;
                    if (_imageservice.CheckImageFileType(httpRequest.Files[file].FileName))
                    {
                        file_Img = httpRequest.Files[file];
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        file_Img.InputStream.CopyTo(ms);
                        var Images = await _imageservice.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageservice.ResizeImage(Images.Image.Id, Int32.Parse(setting.Photomaxwidth));
                        imagesListId.Add(Images.Image.Id);
                    }
                    ImgId = string.Join(",", imagesListId);
                }
                var query = await _datacontext.fUV_ProjectImages.SingleOrDefaultAsync(n => n.Id == request.Id);
                query.Images =  ImgId;
                query.UpdatedAt = DateTime.Now;
                query.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                await _datacontext.SaveChangesAsync();
                result.Success = true;
             }catch(Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateFileAttachProjectImageRequest(int id, HttpPostedFile fileImg)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                string ImgId = "";
                List<string> imagesList = new List<string>();
                List<int> imagesListId = new List<int>();
                if (fileImg != null)
                {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            fileImg.InputStream.CopyTo(ms);
                              _imageservice.WriteOverrideImage(id, ms.GetBuffer(), "ProjectImage\\Images");

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
        public async Task<BaseResponse<bool>> DeletedFileAttachProjectImageRequest(ProjectImageRequest request,int Idimg)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var query = await _datacontext.fUV_ProjectImages.SingleOrDefaultAsync(n => n.Id == request.Id);
                string ImgId = "";
                List<int> imagesListId = new List<int>();
                List<int> ArrId = new List<int>();
                imagesListId = query.Images.Split(',')?.Select(Int32.Parse)?.ToList();
                foreach (var listId in imagesListId)
                {
                    if(listId != Idimg)
                    {
                        ArrId.Add(listId);
                    }
                    ImgId = string.Join(",", ArrId);
                }
                query.Images = ImgId;
                query.UpdatedAt = DateTime.Now;
                query.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<bool>> DeletedAllFileAttachProjectImageRequest(ProjectImageRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var query = await _datacontext.fUV_ProjectImages.SingleOrDefaultAsync(n => n.Id == request.Id);
                query.Images = string.Empty;
                query.UpdatedAt = DateTime.Now;
                query.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<ProjectImagesResponse>> TakeProjectImageById(int Id)
        {
            var result = new BaseResponse<ProjectImagesResponse> { };
            try
            {
                var data = await _datacontext.fUV_ProjectImages.FindAsync(Id);
                result.Item = data.MapTo<ProjectImagesResponse>();

                List<object> imagesList = new List<object>();
                List<int> imagesListId = new List<int>();
                if (result.Item.Images.HasValue())
                {
                    imagesListId = result.Item?.Images.Split(',')?.Select(Int32.Parse)?.ToList();
                    foreach (var listId in imagesListId)
                    {
                        var link = await _datacontext.ImageRecords.SingleOrDefaultAsync(d => d.Id == listId);
                        imagesList.Add(new {
                            id = link.Id,
                            url = link.AbsolutePath
                        });
                    }
                    result.Item.ImageList = imagesList;
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
        public bool CheckImageFileType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (ext == null)
                return false;
            switch (ext.ToLower())
            {
                case ".gif":
                    return true;
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }
        }
        private string LocalMapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
        #endregion

        #region Mobile
        public async Task<BaseResponse<ProjectImagesResponse>> TakeAllProjectImagesForMobile(ProjectImageRequest request)
        {
            var result = new BaseResponse<ProjectImagesResponse> { };
            try
            {
                var query = _datacontext.fUV_ProjectImages.AsQueryable().Where(d => !d.Deleted);
                if (request.Term.HasValue() && request.Term != "")
                {
                    query = query.Where(d => d.Title.Contains(request.Term));
                }

                var data = await query.OrderByDescending(d => d.Id).Select(s => new ProjectImagesResponse
                {
                    Title = s.Title,
                    Description = s.Description,
                    Images = s.Images,
                    UpdatedBy =  _datacontext.Users.FirstOrDefault(d => d.UserName.ToLower().Equals(s.UpdatedBy.ToLower())).DisplayName,
                    UpdatedAt = s.UpdatedAt.Value.Day.ToString() + "/" + s.UpdatedAt.Value.Month.ToString() + "/" + s.UpdatedAt.Value.Year.ToString()
                }).ToListAsync();
                //result.Data = data.MapTo<ProjectImagesResponse>();
                foreach (var post in data)
                {
                    List<object> imagesList = new List<object>();
                    List<int> imagesListId = new List<int>();
                    if (post.Images.HasValue() && post.Images != "")
                    {
                        imagesListId = post.Images.Split(',')?.Select(Int32.Parse)?.ToList();
                        foreach (var listId in imagesListId)
                        {
                            var link = await _datacontext.ImageRecords.FirstOrDefaultAsync(d => d.Id == listId);
                            if (link != null)
                            {
                                imagesList.Add(new
                                {
                                    id = link.Id,
                                    url = link.AbsolutePath
                                });
                            }
                            
                        }
                        if (imagesList.Count > 0)
                        {
                            post.ImageList = imagesList;
                        }
                    }
                }
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
    }
}
