using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Catalogue;
using DaiPhucVinh.Shared.Common;
using Falcon.Core;
using Falcon.Web.Core.Log;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace DaiPhucVinh.Services.MainServices.Catalogue
{
    public interface ICatalogueService
    {
        Task<BaseResponse<CatalogueResponse>> TakeAllCatalogues(CatalogueRequest request);
        Task<BaseResponse<CatalogueResponse>> TakeCatalogueById(int Id);
        Task<BaseResponse<bool>> RemoveCatalogue(CatalogueRequest request);
        //Task<BaseResponse<int>> CreateCatalogue(CatalogueRequest request);
        Task<BaseResponse<bool>> CreateCatalogue(HttpPostedFile fileImg, HttpPostedFile filePdf, CatalogueRequest request);
        Task<BaseResponse<bool>> UpdateCatalogue(HttpPostedFile fileImg, HttpPostedFile filePdf, CatalogueRequest request);

        #region Mobile
        Task<BaseResponse<CatalogueDto>> TakeAllCataloguesForMobile(CatalogueRequest request);
        #endregion
    }
    public class CatalogueService : ICatalogueService
    {
        private readonly DataContext _datacontext;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
        private readonly ISetting _settingService;
        private readonly ILogger _logger;
        private readonly ILogService _logService;

        public CatalogueService(DataContext datacontext, IUserService userService, IImageService imageService, ISetting settingService, ILogger logger, ILogService logService)
        {
            _datacontext = datacontext;
            _userService = userService;
            _imageService = imageService;
            _settingService = settingService;
            _logger = logger;
            _logService = logService;
        }
        public async Task<BaseResponse<CatalogueResponse>> TakeAllCatalogues (CatalogueRequest request)
        {
            var result = new BaseResponse<CatalogueResponse> { };
            try
            {
                var query = _datacontext.FUV_Catalogs.AsQueryable().Where(d => !d.Deleted);
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
                result.Data = data.MapTo<CatalogueResponse>();
                foreach (var catalogue in result.Data)
                {
                    var _user = await _userService.TakeUserByUserName(catalogue.CreatedBy);
                    catalogue.CreatedBy = _user.Item.DisplayName;
                }
                result.Success = true;

            }
            catch(Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<CatalogueResponse>> TakeCatalogueById(int Id)
        {
            var result = new BaseResponse<CatalogueResponse> { };
            try
            {
                var data = await _datacontext.FUV_Catalogs.FindAsync(Id);
                result.Item = data.MapTo<CatalogueResponse>();

                List<object> imagesList = new List<object>();
                List<int> imagesListId = new List<int>();

                List<string> pdfFileList = new List<string>();
                List<int> pdfFileListId = new List<int>();

                if (result.Item.Images.HasValue())
                {
                    /*imagesListId = result.Item?.Images.Split(',')?.Select(Int32.Parse)?.ToList();
                    foreach (var listId in imagesListId)
                    {
                        var link = await _datacontext.ImageRecords.SingleOrDefaultAsync(d => d.Id == listId);
                        result.Item.ImageId = link.Id;
                        result.Item.ImageAbsolutePath = link.AbsolutePath;
                    }*/
                    var imgId = Int32.Parse(result.Item.Images);
                    var link = await _datacontext.ImageRecords.FirstOrDefaultAsync(d => d.Id == imgId);
                    if (link != null)
                    {
                        result.Item.ImageId = link.Id;
                        result.Item.ImageAbsolutePath = link.AbsolutePath;
                    }
                }

                if (result.Item.PdfFile.HasValue())
                {
                    var pdfId = Int32.Parse(result.Item.PdfFile);
                    var link = await _datacontext.ImageRecords.FirstOrDefaultAsync(d => d.Id == pdfId);
                    if (link == null)
                    {
                        result.Message = "Không tìm được đường dẫn file";
                        return result;
                    }
                    result.Item.PdfId = link.Id;
                    result.Item.PdfFileName = link.FileName;
                    result.Item.PdfFileAbsolutePath = link.AbsolutePath;
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
        public async Task<BaseResponse<bool>> RemoveCatalogue(CatalogueRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var catalogue = await _datacontext.FUV_Catalogs.SingleOrDefaultAsync(x => x.Id == request.Id);
                catalogue.Deleted = true;
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
        public async Task<BaseResponse<bool>> CreateCatalogue(HttpPostedFile fileImg, HttpPostedFile filePdf, CatalogueRequest request)
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
                        var addImg = await _imageService.InsertImage(ms.GetBuffer(), fileImg.FileName, "Catalogue\\Images", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImageHaveStoreName(addImg.Image.Id, Int32.Parse(setting.Photomaxwidth), "Catalogue\\Images");
                        ImgId = addImg.Image.Id.ToString();
                    }
                }

                if (filePdf != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        filePdf.InputStream.CopyTo(ms);
                        var addPdf = await _imageService.InsertFile(ms.GetBuffer(), filePdf.FileName, "Catalogue\\Files");
                        PdfId = addPdf.Image.Id.ToString();
                    }
                }
                var task = _datacontext.FUV_Catalogs.Add(new FUV_Catalogs
                {
                    Title = request.Title,
                    ShortTitle = request.ShortTitle,
                    Description = request.Description,
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

        public async Task<BaseResponse<bool>> UpdateCatalogue(HttpPostedFile fileImg, HttpPostedFile filePdf, CatalogueRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                var img = _datacontext.ImageRecords.FindAsync(request.Images);


                string ImgId = "";
                if (fileImg != null)
                {
                    //insert image
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fileImg.InputStream.CopyTo(ms);
                        var addImg = await _imageService.InsertImage(ms.GetBuffer(), fileImg.FileName, "Catalogue\\Images", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImageHaveStoreName(addImg.Image.Id, Int32.Parse(setting.Photomaxwidth), "Catalogue\\Images");
                        ImgId = addImg.Image.Id.ToString();
                    }
                }


                string PdfId = "";
                if (filePdf != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        filePdf.InputStream.CopyTo(ms);
                        var addPdf = await _imageService.InsertFile(ms.GetBuffer(), filePdf.FileName, "Catalogue\\Files");
                        PdfId = addPdf.Image.Id.ToString();
                    }

                }

                var catalogue = await _datacontext.FUV_Catalogs.FindAsync(request.Id);
                if (catalogue != null)
                {

                    catalogue.Title = request.Title;
                    catalogue.ShortTitle = request.ShortTitle;
                    catalogue.Description = request.Description;
                    if (ImgId != "" && PdfId != "")
                    {
                        catalogue.Images = ImgId;
                        catalogue.PdfFile = PdfId;
                    }
                    if (ImgId != "")
                    {
                        catalogue.Images = ImgId;
                    }
                    if (PdfId != "")
                    {
                        catalogue.PdfFile = PdfId;
                    }
                    catalogue.UpdatedAt = DateTime.Now;
                    catalogue.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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


        #region[Mobile]
        public async Task<BaseResponse<CatalogueDto>> TakeAllCataloguesForMobile(CatalogueRequest request)
        {
            var result = new BaseResponse<CatalogueDto> { };
            try
            {
                var query = _datacontext.FUV_Catalogs.AsQueryable().Where(d => !d.Deleted);
                if (request.Term.HasValue() && request.Term != "")
                {
                    query = query.Where(d => d.Title.Contains(request.Term));
                }
                var data = await query.OrderByDescending(d => d.Id).ToListAsync();
                result.Data = data.MapTo<CatalogueDto>();
                foreach (var catalogue in result.Data)
                {
                    var image = await _datacontext.ImageRecords.FindAsync(Int32.Parse(catalogue.Images));
                    if (image != null)
                    {
                        catalogue.ImageAbsolutePath = image.AbsolutePath;
                    }

                    var pdf = await _datacontext.ImageRecords.FindAsync(Int32.Parse(catalogue.PdfFile));
                    if (pdf == null)
                    {
                        result.Message = "Không tìm thấy đường dẫn file";
                        return result;
                    }
                    catalogue.PdfFileAbsolutePath = pdf.AbsolutePath;
                    catalogue.PdfFileName = pdf.FileName;
                    /*var product = await _datacontext.WMS_Items.FindAsync(userguides.ItemCode);
                    userguides.Model = product.Model;*/
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
