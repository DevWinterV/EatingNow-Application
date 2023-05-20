using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.ImageRecords;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.ImageRecords
{
    public interface IImageRecordService
    {
        Task<BaseResponse<ImageRecordsResponse>> TakeAllImages(ImageRecordsRequest request);
        Task<BaseResponse<bool>> CreateImage(HttpPostedFile file);
        Task<List<string>> GetImageList(string ImageId);
    }
    public class ImageRecordService : IImageRecordService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly IImageService _imageService;
        private readonly ISetting _settingService;

        public ImageRecordService(DataContext datacontext, ILogService logService, IImageService imageService, ISetting settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _imageService = imageService;
            _settingService = settingService;
        }
        public async Task<BaseResponse<ImageRecordsResponse>> TakeAllImages(ImageRecordsRequest request)
        {
            var result = new BaseResponse<ImageRecordsResponse> { };
            try
            {
                var query = _datacontext.ImageRecords.AsQueryable().Where(d => !d.Deleted && d.IsWeb == true && (d.FileName.EndsWith(".gif") || d.FileName.EndsWith(".jpg") || d.FileName.EndsWith(".jpeg") || d.FileName.EndsWith(".png")));
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.FileName.Contains(request.Term));
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<ImageRecordsResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateImage(HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (file != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        var Images = await _imageService.InsertImage(ms.GetBuffer(), file.FileName, "DaiPhucVinh\\Image", true);
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));

                        _datacontext.FUV_WorkLogs.Add(new FUV_WorkLog
                        {
                            UserId = TokenHelper.CurrentIdentity().UserId,
                            Description = TokenHelper.CurrentIdentity().UserName + " đã thêm hình ảnh " + Images.Image.FileName,
                            CreatedAt = DateTime.Now,
                        });
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
        public async Task<List<string>> GetImageList(string ImageId)
        {
            List<string> result = new List<string> { };
            List<int> ImageIdList = new List<int>();
            try
            {
                ImageIdList = ImageId.Split(',')?.Select(Int32.Parse)?.ToList();
                foreach (var listId in ImageIdList)
                {
                    var link = await _datacontext.ImageRecords.SingleOrDefaultAsync(d => d.Id == listId);
                    if(link != null)
                    {
                        result.Add(link.AbsolutePath);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                _logService.InsertLog(ex);
            }
            return result;
        }
    }
}
