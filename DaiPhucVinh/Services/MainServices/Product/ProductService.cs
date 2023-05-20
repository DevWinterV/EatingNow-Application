using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.CategoryItem;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.ItemImages;
using DaiPhucVinh.Shared.Notification;
using DaiPhucVinh.Shared.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Product
{
    public interface IProductService
    {
        #region Web Admin
        Task<BaseResponse<ProductResponse>> TakeAlls(ProductRequest request);
        Task<BaseResponse<ProductItemGroupResponse>> TakeAllItemGroup(ProductRequest request);
        Task<BaseResponse<ProductImageResponse>> TakeAllImageByItemCode(ProductRequest request);
        Task<BaseResponse<bool>> UploadImages(HttpPostedFile file, string ItemCode);
        Task<BaseResponse<bool>> DeleteImages(ProductImageRequest request);
        Task<BaseResponse<bool>> UpdateItem(ProductRequest request);
        Task<BaseResponse<ProductResponse>> TakeProductDetailById(ProductRequest request);
        Task<BaseResponse<ProductDto>> TakeProductDetailByCode(ProductRequest request);
        Task<BaseResponse<ProductResponse>> TakeProductById(string Code);
        Task<BaseResponse<CategoryItemResponse>> TakeItemCategory(string Code);
        Task<BaseResponse<bool>> ImagesSync();
        Task<BaseResponse<ItemImageResponse>> CheckMainImage(ProductImageRequest request);
        Task<BaseResponse<ItemImageResponse>> HidenImage(ProductImageRequest request);
        //Task<BaseResponse<ProductResponse>> TakeProductByUserId(ProductRequest request);
        #endregion

        #region Mobile
        Task<BaseResponse<ProductDto>> TakeProductByGroupCode(ProductRequest request);
        Task<BaseResponse<ProductDto>> SearchProducts(ProductRequest request);
        Task<BaseResponse<ProductDto>> NewProducts(ProductRequest request);
        Task<BaseResponse<NotificationResponse>> TakeNotifications(NotificationRequest request);
        Task<BaseResponse<bool>> UpdateStatusNotifications(NotificationRequest request);
        #endregion
    }
    public class ProductService : IProductService
    {
        private readonly DataContext _datacontext;
        private readonly ISetting _settingService;
        private readonly IImageService _imageService;
        private readonly ILogService _logService;
        public ProductService(DataContext datacontext, IImageService imageService, ISetting settingService, ILogService logService)
        {
            _datacontext = datacontext;
            _imageService = imageService;
            _settingService = settingService;
            _logService = logService;
        }
        #region Web Admin
        public async Task<BaseResponse<ProductResponse>> TakeAlls(ProductRequest request)
        {
            var result = new BaseResponse<ProductResponse> { };
            try
            {
                var query = _datacontext.WMS_Items.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term) ||
                    d.Model.Contains(request.Term));
                }
                if (request.ItemGroup_Code.HasValue())
                {
                    query = query.Where(d => d.ItemGroup_Code == request.ItemGroup_Code);
                }
                if (request.ItemCategoryCode.HasValue())
                {
                    query = query.Where(d => d.ItemCategoryCode == request.ItemCategoryCode);
                }
                if (request.dataItemCode != null && request.dataItemCode.Any())
                {
                    query = query.Where(x => !request.dataItemCode.Contains(x.Code));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                var resultList = data.MapTo<ProductResponse>();
                foreach(var item in resultList)
                {
                    var flag = false;
                    var imageList = _datacontext.WMS_Item_Images.AsQueryable().Where(d => d.ItemCode == item.Code).ToList();
                    if (imageList.Count > 0)
                    {
                        /*var mainList = await _datacontext.WMS_Item_Images.FirstOrDefaultAsync(d => d.ItemCode == item.Code && d.IsMain == true);
                        if(mainList != null)
                        {
                            item.AbsolutePath = mainList.Image;
                        }
                        else
                        {
                            item.AbsolutePath = imageList[0].Image;
                        }*/
                        item.AbsolutePath = imageList[0].Image;
                        if (imageList != null)
                        {
                            foreach (var image in imageList)
                            {
                                if (!flag)
                                {
                                    if (image.IsMain == true)
                                    {
                                        item.AbsolutePath = image.Image;
                                        flag = true;
                                    }
                                }

                            }
                        }
                    }
                }
                result.Data = resultList;
                result.Success = true;

                
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ProductItemGroupResponse>> TakeAllItemGroup(ProductRequest request)
        {
            var result = new BaseResponse<ProductItemGroupResponse> { };
            try
            {
                var query = _datacontext.WMS_ItemGroups.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<ProductItemGroupResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ProductImageResponse>> TakeAllImageByItemCode(ProductRequest request)
        {
            var result = new BaseResponse<ProductImageResponse> { };
            try
            {
                var query = _datacontext.WMS_Item_Images.AsQueryable();
                if (request.ItemCode.HasValue())
                {
                    query = query.Where(d => d.ItemCode == request.ItemCode);
                }
                var data = await query.ToListAsync();
                result.Data = data.MapTo<ProductImageResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UploadImages(HttpPostedFile file, string ItemCode)
        {
            var result = new BaseResponse<bool>();
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        _datacontext.WMS_Item_Images.Add(new WMS_Item_Image
                        {
                            ItemCode = ItemCode,
                            ImageName = file.FileName,
                            Image = ms.GetBuffer(),
                        });
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã thêm hình ảnh " + file.FileName,
                            CreatedAt = DateTime.Now,
                        });
                        await _datacontext.SaveChangesAsync();
                        result.Success = true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> DeleteImages(ProductImageRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                var entity = await _datacontext.WMS_Item_Images.SingleOrDefaultAsync(x => x.Id == request.Id);
                _datacontext.WMS_Item_Images.Remove(entity);
                _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                {
                    UserId = TokenHelper.CurrentIdentity().UserId,
                    Description = TokenHelper.CurrentIdentity().UserName + " đã xóa hình ảnh " + entity.ImageName,
                    CreatedAt = DateTime.Now,
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
        public async Task<BaseResponse<bool>> UpdateItem(ProductRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var item = await _datacontext.WMS_Items.FirstOrDefaultAsync(x => x.Code == request.Code);
                if(item != null)
                {
                    #region
                    if (item.Code2 != request.Code2)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " +  item.Name + "(" + item.Code + ")" +  " từ mã sản phẩm "  + item.Code2 + " thành " + request.Code2,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.Model != request.Model)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ Model " +  item.Model + " thành " +  request.Model,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.Name != request.Name)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " thành " + request.Name,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.Specifications != request.Specifications)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ thông số kỹ thuật " + item.Specifications + " thành " + request.Specifications,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.Description != request.Description)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ diển giải " + item.Description + " thành " + request.Description,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.CountryOfOriginCode != request.CountryOfOriginCode)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ xuất xứ " + item.CountryOfOriginCode + " thành " + request.CountryOfOriginCode,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.ItemCategoryCode != request.ItemCategoryCode)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ nhóm sản phẩm " + item.WMS_ItemCategory?.Name + " thành " + request.ItemCategoryName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.LinkVideo != request.LinkVideo)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ link video " + item.LinkVideo + " thành " + request.LinkVideo,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.UnitOfMeasure != request.UnitOfMeasure)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ đơn vị " + item.WMS_UnitOfMeasure?.Name + " thành " + request.UnitOfMeasureName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.UnitPrice != request.UnitPrice)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ giá bán sỉ " + item.UnitPrice + " thành " + request.UnitPrice,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    if (item.UnitCost_Whole != request.UnitCost_Whole)
                    {
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã cập nhật sản phẩm " + item.Name + "(" + item.Code + ")" + " từ giá bán lẻ " + item.UnitCost_Whole + " thành " + request.UnitCost_Whole,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    #endregion
                    item.Code = request.Code;
                    item.Code2 = request.Code2;
                    item.Model = request.Model;
                    item.Name = request.Name;
                    item.Specifications = request.Specifications;
                    item.Description = request.Description;
                    item.CountryOfOriginCode = request.CountryOfOriginCode;
                    item.ItemCategoryCode = request.ItemCategoryCode;
                    item.LinkVideo = request.LinkVideo;
                    item.UnitOfMeasure = request.UnitOfMeasure;
                    item.UnitPrice = request.UnitPrice;
                    item.UnitCost_Whole = request.UnitCost_Whole;
                    item.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
                    item.LastUpdateDate = DateTime.Now;
                    item.IsChecked = false;
                    if(request.IsDelete != null)
                    {
                        item.IsDelete = request.IsDelete;
                    }
                }

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
        public async Task<BaseResponse<ProductResponse>> TakeProductById(string Code)
        {
            var result = new BaseResponse<ProductResponse> { };
            try
            {
                //var query = await _datacontext.WMS_Items.AsNoTracking().Where(code => code.Code == Code).ToListAsync();
                //result.Data = query.MapTo<ProductResponse>();
                //result.Success = true;
                var data = await _datacontext.WMS_Items.FindAsync(Code);
                result.Item = data.MapTo<ProductResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<CategoryItemResponse>> TakeItemCategory(string Code)
        {
            var result = new BaseResponse<CategoryItemResponse> { };
            try
            {
                var query =  await _datacontext.WMS_Items.FirstOrDefaultAsync(x => x.Code == Code);
                result.Item = query.MapTo<CategoryItemResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ProductResponse>> TakeProductDetailById(ProductRequest request)
        {
            var result = new BaseResponse<ProductResponse> { };
            try
            {
                var query = await _datacontext.WMS_Items.FindAsync(request.ItemCode);
                result.Item =  query.MapTo<ProductResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ProductDto>> TakeProductDetailByCode(ProductRequest request)
        {
            var result = new BaseResponse<ProductDto> { };
            try
            {
                var query = _datacontext.WMS_Items.AsQueryable();
                var queryImageRecord = _datacontext.ImageRecords.AsQueryable();

                if (!string.IsNullOrEmpty(request.ItemCode))
                {
                    var data = await query.FirstOrDefaultAsync(d => d.Code == request.ItemCode
                                                    && d.IsDelete.HasValue && !d.IsDelete.Value);
                    if (data == null)
                    {
                        result.Success = false;
                        return result;
                    }
                    var mapdata = data.MapTo<ProductDto>();
                    List<ImageRecord> lstImg = new List<ImageRecord>();
                    if (mapdata.ImageRecordId != null)
                    {
                        var listImageId = data.ImageRecordId.Split(',').Where(x => int.TryParse(x, out _))
                                                                    .Select(int.Parse)
                                                                    .ToList();
                        foreach (var id in listImageId)
                        {
                            var dataId = queryImageRecord.FirstOrDefault(x => x.Id == id);
                            lstImg.Add(dataId);
                        }
                    }
                    mapdata.ListImage = lstImg;

                    result.CustomData = mapdata;
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
        public async Task<BaseResponse<bool>> ImagesSync()
        {
            var result = new BaseResponse<bool>();
            try
            {
                var query = _datacontext.WMS_Item_Images.AsQueryable();

                query = query.Where(x => x.WMS_Item.IsDelete.HasValue && !x.WMS_Item.IsDelete.Value
                                                               && string.IsNullOrEmpty(x.WMS_Item.ImageRecordId));

                var groupBy = query.GroupBy(g => g.ItemCode);

                var dataGroupby = groupBy.Select(s => new
                {
                    ItemCode = s.Key,
                    ListImage = s.Select(x => new ProductImageResponse 
                    { 
                        Id = x.Id,
                        ImageName = x.ImageName,
                        Image = x.Image,
                        Description = x.Description,
                        IsMain = x.IsMain
                    }).ToList()
                }).ToList();
                //var data = query.MapTo<ProductImageResponse>().ToList();
                foreach (var d in dataGroupby)
                {
                    List<int> ListImageId = new List<int>();

                    foreach (var item in d.ListImage)
                    {

                        using (MemoryStream ms = new MemoryStream())
                        {
                            var setting = _settingService.LoadSetting<MetadataSettings>();
                            if (item.IsMain.HasValue)
                            {
                                var Images = await _imageService.InsertImageWithServerSetting(item.Image, item.ImageName, "DaiPhucVinh\\Image", setting.CdnServer, item.IsMain.Value);

                                if (Images.Image != null)
                                {
                                    await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                    ListImageId.Add(Images.Image.Id);
                                }
                            }
                            else
                            {
                                var Images = await _imageService.InsertImageWithServerSetting(item.Image, item.ImageName, "DaiPhucVinh\\Image", setting.CdnServer);

                                if (Images.Image != null)
                                {
                                    await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                    ListImageId.Add(Images.Image.Id);
                                }
                            }
                        }
                    }
                    if (ListImageId.Count > 0)
                    {
                        string listRecord = string.Join(",", ListImageId);
                        var dataItems = _datacontext.WMS_Items.FirstOrDefault(x => x.Code.Equals(d.ItemCode));
                        dataItems.ImageRecordId = listRecord;
                    }
                    _datacontext.SaveChanges();
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ItemImageResponse>> CheckMainImage(ProductImageRequest request)
        {
            var result = new BaseResponse<ItemImageResponse> { };
            try
            {
                var data = await _datacontext.WMS_Item_Images.FindAsync(request.Id);
                if(data != null)
                {
                    if (data.IsMain == null)
                    {
                        data.IsMain = true;
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã chọn ảnh đại diện " + data.ImageName,
                            CreatedAt = DateTime.Now,
                        });
                    } else if (data.IsMain == true ) {
                        data.IsMain = false;
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã bỏ chọn ảnh đại diện " + data.ImageName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    else
                    {
                        data.IsMain = true;
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã chọn ảnh đại diện " + data.ImageName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                }
          
                await _datacontext.SaveChangesAsync();
                result.Success = true;
                result.Item = data.MapTo<ItemImageResponse>();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<ItemImageResponse>> HidenImage(ProductImageRequest request)
        {
            var result = new BaseResponse<ItemImageResponse> { };
            try
            {
                var data = await _datacontext.WMS_Item_Images.FindAsync(request.Id);
                if (data != null)
                {
                    if (data.IsHiden == null)
                    {
                        data.IsHiden = true;
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã ẩn hình ảnh " + data.ImageName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    else if (data.IsHiden == true)
                    {
                        data.IsHiden = false;
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã bỏ ẩn hình ảnh " + data.ImageName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                    else
                    {
                        data.IsHiden = true;
                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã ẩn hình ảnh " + data.ImageName,
                            CreatedAt = DateTime.Now,
                        });
                    }
                }
                await _datacontext.SaveChangesAsync();
                result.Success = true;
                result.Item = data.MapTo<ItemImageResponse>();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        #endregion

        #region [APP]

        #region Product By GroupCode
        public async Task<BaseResponse<ProductDto>> TakeProductByGroupCode(ProductRequest request)
        {
            var result = new BaseResponse<ProductDto> { };
            try
            {
                var query = _datacontext.WMS_Items.AsQueryable();
                var queryImageRecord = _datacontext.ImageRecords.AsQueryable();
                if (request.ItemCatalogueCode.HasValue())
                {
                    query = query.Where(d => d.ItemCategoryCode == request.ItemCatalogueCode
                                                        && d.IsDelete.HasValue && !d.IsDelete.Value);
                }
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.ToLower().Contains(request.Term.ToLower()) 
                                                || d.Model.ToLower().Contains(request.Term.ToLower())
                                                && d.IsDelete.HasValue && !d.IsDelete.Value);
                }
                var data = await query.OrderByDescending(o => o.Id)
                                            .Skip(request.Page * request.PageSize).Take(request.PageSize).ToListAsync();
                var mapdata = data.MapTo<ProductDto>();

                foreach (var item in mapdata)
                {
                    if (item.ImageRecordId != null)
                    {
                        var listImageId = item.ImageRecordId.Split(',').Where(x => int.TryParse(x, out _))
                                                                    .Select(int.Parse)
                                                                    .ToArray();
                        if (listImageId.Count() > 0)
                        {
                            foreach (var id in listImageId)
                            {
                                var mainImage = queryImageRecord.FirstOrDefault(x => x.Id == id
                                                                            && x.IsMain.HasValue && x.IsMain.Value);
                                if (mainImage != null)
                                {
                                    item.MainImagePath = mainImage.AbsolutePath;
                                }
                                else
                                {
                                    int imgId = listImageId[0];
                                    item.MainImagePath = queryImageRecord.FirstOrDefault(x => x.Id == imgId).AbsolutePath;
                                }
                            }
                        }
                    }
                    var product = _datacontext.WMS_InventoryItems.Where(x => x.ItemCode == item.Code);
                    var slTonKho = product.Sum(x => x.Qty);
                    item.Inventory = slTonKho;
                }
                result.Data = mapdata;
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

        #region Search Product
        public async Task<BaseResponse<ProductDto>> SearchProducts(ProductRequest request)
        {
            var result = new BaseResponse<ProductDto> { };
            try
            {
                var query = _datacontext.WMS_Items.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term) || d.Model.Contains(request.Term));
                }
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                var resultList = data.MapTo<ProductDto>();
                foreach (var item in resultList)
                {
                    item.MainImagePath = MainImagePath(item.ImageRecordId);
                }
                result.Data = resultList;
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

        #region List New Product
        public async Task<BaseResponse<ProductDto>> NewProducts(ProductRequest request)
        {
            var result = new BaseResponse<ProductDto> { };
            try
            {
                var query = _datacontext.WMS_Items.AsQueryable();

                query = query.Where(x => x.IsDelete.HasValue && !x.IsDelete.Value
                                          && x.IsNew.HasValue && x.IsNew.Value);

                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name.Contains(request.Term) || d.Model.Contains(request.Term));
                }
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                var resultList = data.MapTo<ProductDto>();
                foreach (var item in resultList)
                {
                    item.MainImagePath = MainImagePath(item.ImageRecordId);
                }
                result.Data = resultList;
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

        #region Notification
        public async Task<BaseResponse<NotificationResponse>> TakeNotifications(NotificationRequest request)
        {
            var result = new BaseResponse<NotificationResponse> { };

            var queryNotification = _datacontext.FUV_Notifications.AsQueryable();
            try
            {
                if (request == null)
                {
                    result.Message = "Điều kiện rỗng";
                    return result;
                }
                if (request.NotificationGroupId > 0)
                {
                    queryNotification = queryNotification.Where(x => x.NotificationGroupId == request.NotificationGroupId
                                                                    && x.IsSend);
                }
                var dataNotification = await queryNotification.ToListAsync();
                if (dataNotification == null)
                {
                    result.Message = "Không tìm thấy dữ liệu";
                    return result;
                }
                result.DataCount = dataNotification.Count();
                result.Data = dataNotification.MapTo<NotificationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateStatusNotifications(NotificationRequest request)
        {
            var result = new BaseResponse<bool> { };

            var queryNotification = _datacontext.FUV_Notifications.AsQueryable();
            try
            {
                if (request == null)
                {
                    result.Message = "Điều kiện rỗng";
                    return result;
                }
                if (request.IsSee.HasValue)
                {
                    if (request.Id > 0)
                    {
                        var dataNotification = await queryNotification.FirstOrDefaultAsync(x => x.Id == request.Id);

                        if (dataNotification == null)
                        {
                            result.Message = "Không tìm thấy dữ liệu";
                            return result;
                        }

                        dataNotification.IsSee = request.IsSee.Value;
                        _datacontext.SaveChanges();

                        result.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        #endregion
        private string MainImagePath(string listImageId)
        {
            string MainImage = "";
            var queryImageRecord = _datacontext.ImageRecords.AsQueryable();
            if (!string.IsNullOrEmpty(listImageId))
            {
                var listId = listImageId.Split(',').Where(x => int.TryParse(x, out _))
                                                                    .Select(int.Parse)
                                                                    .ToArray();
                if (listId.Count() > 0)
                {
                    foreach (var id in listId)
                    {
                        MainImage = "";
                        var mainImage = queryImageRecord.FirstOrDefault(x => x.Id == id
                                                                    && x.IsMain.HasValue && x.IsMain.Value
                                                                    && !x.Deleted);
                        if (mainImage != null)
                        {
                            MainImage = mainImage.AbsolutePath;
                            break;
                        }
                        else
                        {
                            int imgId = listId[0];
                            MainImage = queryImageRecord.FirstOrDefault(x => x.Id == imgId).AbsolutePath;
                        }
                    }
                }
            }
            return MainImage;
        }
        #endregion
    }
}
