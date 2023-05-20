using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.UserGuides;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.UserGuides
{
    public interface IUserGuidesService
    {
        Task<BaseResponse<UserGuidesResponse>> TakeAllUserGuides(UserGuidesRequest request);
        Task<BaseResponse<UserGuidesResponse>> TakeUserGuidesById(int Id);
        Task<BaseResponse<bool>> CreateUserGuides(HttpPostedFile fileImg, HttpPostedFile filePdf, UserGuidesRequest request);
        Task<BaseResponse<bool>> UpdateUserGuides(HttpPostedFile fileImg, HttpPostedFile filePdf, UserGuidesRequest request);
        Task<BaseResponse<bool>> RemoveUserGuides(UserGuidesRequest request);

        #region Mobile
        Task<BaseResponse<UserGuidesResponse>> TakeAllUserGuidesForMobile(UserGuidesRequest request);
        #endregion
    }
    public class UserGuidesService : IUserGuidesService
    {
        private readonly DataContext _datacontext;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly ISetting _settingService;
        private readonly ILogService _logService;
        public UserGuidesService(DataContext datacontext, IUserService userService, IImageService imageService, ISetting settingService, ILogService logService)
        {
            _datacontext = datacontext;
            _userService = userService;
            _imageService = imageService;
            _settingService = settingService;
            _logService = logService;
        }
        public async Task<BaseResponse<UserGuidesResponse>> TakeAllUserGuides(UserGuidesRequest request)
        {
            var result = new BaseResponse<UserGuidesResponse> { };
            try
            {
                var query = _datacontext.FUV_UserGuides.AsQueryable().Where(d => !d.Deleted);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Title.Contains(request.Term) ||
                                                d.WMS_Item.Name.Contains(request.Term));
                }
                if (request.ItemCode.HasValue())
                {
                    query = query.Where(d => d.ItemCode == request.ItemCode);
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
                result.Data = data.MapTo<UserGuidesResponse>();
                foreach (var userguides in result.Data)
                {
                    var _user = await _userService.TakeUserByUserName(userguides.CreatedBy);
                    userguides.CreatedBy = _user.Item.DisplayName;
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

        public async Task<BaseResponse<UserGuidesResponse>> TakeUserGuidesById(int Id)
        {
            var result = new BaseResponse<UserGuidesResponse> { };
            try
            {
                var data = await _datacontext.FUV_UserGuides.FindAsync(Id);
                result.Item = data.MapTo<UserGuidesResponse>();

                List<string> imagesList = new List<string>();
                List<int> imagesListId = new List<int>();

                List<string> pdfFileList = new List<string>();
                List<int> pdfFileListId = new List<int>();

                if (result.Item.Images.HasValue())
                {
                    var imgId = Int32.Parse(result.Item.Images);
                    var link = await _datacontext.ImageRecords.SingleOrDefaultAsync(d => d.Id == imgId);

                    if (link != null)
                    {
                        result.Item.ImageAbsolutePath = link.AbsolutePath;
                        result.Item.ImageId = link.Id;
                    }
                }

                if (result.Item.PdfFile.HasValue())
                {
                    var pdfId = Int32.Parse(result.Item.PdfFile);
                    var link = await _datacontext.ImageRecords.SingleOrDefaultAsync(d => d.Id == pdfId);
                    if (link != null)
                    {
                        result.Item.PdfId = link.Id;
                        result.Item.PdfFileName = link.FileName;
                        result.Item.PdfFileAbsolutePath = link.AbsolutePath;
                    }
                    
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

        public async Task<BaseResponse<bool>> CreateUserGuides(HttpPostedFile fileImg, HttpPostedFile filePdf, UserGuidesRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                string PdfId = "", ImgId = "";
                if (fileImg != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fileImg.InputStream.CopyTo(ms);
                        var addImg = await _imageService.InsertImage(ms.GetBuffer(), fileImg.FileName, "UserGuides\\Images", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImageHaveStoreName(addImg.Image.Id, Int32.Parse(setting.Photomaxwidth), "UserGuides\\Images");
                        ImgId = addImg.Image.Id.ToString();
                    }
                }

                if (filePdf != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        filePdf.InputStream.CopyTo(ms);
                        var addPdf = await _imageService.InsertFile(ms.GetBuffer(), filePdf.FileName, "UserGuides\\Files");
                        PdfId = addPdf.Image.Id.ToString();
                    }
                }
                var task = _datacontext.FUV_UserGuides.Add(new FUV_UserGuides
                {
                    Title = request.Title,
                    ShortTitle = request.ShortTitle,
                    Description = request.Description,
                    ItemCode = request.ItemCode,
                    Images = ImgId,
                    PdfFile = PdfId,
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
                result.Message = ex.Message;
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateUserGuides(HttpPostedFile fileImg, HttpPostedFile filePdf, UserGuidesRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                var img = _datacontext.ImageRecords.FindAsync(request.Images);


                string ImgId = "";
                if (fileImg != null)
                {
                    if (request.Images.HasValue())
                    {
                        //override image
                        using (MemoryStream ms = new MemoryStream())
                        {
                            fileImg.InputStream.CopyTo(ms);
                            _imageService.WriteOverrideImage(Int32.Parse(request.Images), ms.GetBuffer(), "UserGuides\\Images");
                            var setting = _settingService.LoadSetting<MetadataSettings>();
                            await _imageService.ResizeImageHaveStoreName(Int32.Parse(request.Images), Int32.Parse(setting.Photomaxwidth), "UserGuides\\Images");
                        }
                    }
                    else
                    {
                        //insert image
                        using (MemoryStream ms = new MemoryStream())
                        {
                            fileImg.InputStream.CopyTo(ms);
                            var addImg = await _imageService.InsertImage(ms.GetBuffer(), fileImg.FileName, "UserGuides\\Images", true);
                            var setting = _settingService.LoadSetting<MetadataSettings>();
                            await _imageService.ResizeImage(addImg.Image.Id, Int32.Parse(setting.Photomaxwidth));
                            ImgId = addImg.Image.Id.ToString();
                        }
                    }

                }


                string PdfId = "";
                if (filePdf != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        filePdf.InputStream.CopyTo(ms);
                        var addPdf = await _imageService.InsertFile(ms.GetBuffer(), filePdf.FileName, "UserGuides\\Files");
                        PdfId = addPdf.Image.Id.ToString();
                    }
                }

                var userguides = await _datacontext.FUV_UserGuides.FindAsync(request.Id);
                if (userguides != null)
                {

                    userguides.Title = request.Title;
                    userguides.ShortTitle = request.ShortTitle;
                    userguides.Description = request.Description;
                    userguides.ItemCode = request.ItemCode;
                    if (ImgId != "" && PdfId != "")
                    {
                        userguides.Images = ImgId;
                        userguides.PdfFile = PdfId;
                    }
                    if (ImgId != "")
                    {
                        userguides.Images = ImgId;
                    }
                    if (PdfId != "")
                    {
                        userguides.PdfFile = PdfId;
                    }
                    userguides.UpdatedAt = DateTime.Now;
                    userguides.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
                    await _datacontext.SaveChangesAsync();
                    result.Success = true;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> RemoveUserGuides(UserGuidesRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var userguides = await _datacontext.FUV_UserGuides.FindAsync(request.Id);
                userguides.Deleted = true;
                userguides.UpdatedAt = DateTime.Now;
                userguides.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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


        #region Mobile
        public async Task<BaseResponse<UserGuidesResponse>> TakeAllUserGuidesForMobile(UserGuidesRequest request)
        {
            var result = new BaseResponse<UserGuidesResponse> { };
            try
            {
                var query = _datacontext.FUV_UserGuides.AsQueryable().Where(d => !d.Deleted);
                if (request.Term.HasValue() && request.Term != "")
                {
                    query = query.Where(d => d.Title.Contains(request.Term) ||
                                                d.WMS_Item.Name.Contains(request.Term));
                }
                query = query.OrderByDescending(d => d.Id).Take(5);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<UserGuidesResponse>();
                foreach (var userguides in result.Data)
                {
                    if (userguides.Images != null)
                    {
                        var image = await _datacontext.ImageRecords.FindAsync(Int32.Parse(userguides.Images));
                        if (image != null)
                        {
                            userguides.ImageAbsolutePath = image.AbsolutePath;
                        }
                    }
                    if (userguides.PdfFile == null)
                    {
                        result.Message = "Không tìm thấy file";
                        result.Success = false;
                        return result;
                    }
                    var pdf = await _datacontext.ImageRecords.FindAsync(Int32.Parse(userguides.PdfFile));
                    if (pdf != null)
                    {
                        userguides.PdfFileAbsolutePath = pdf.AbsolutePath;
                        userguides.PdfFileName = pdf.FileName;
                    }

                    var product = await _datacontext.WMS_Items.FindAsync(userguides.ItemCode);
                    userguides.Model = product.Model;
                }
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
