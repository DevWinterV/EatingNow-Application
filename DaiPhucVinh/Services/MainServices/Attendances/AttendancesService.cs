using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Excel;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.User;
using Falcon.Core;
using Falcon.Web.Core.Log;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Attendances
{
    public interface IAttendancesService
    {
        Task<BaseResponse<AttendancesResponse>> TakeAlls(AttendancesRequest request);
        Task<BaseResponse<bool>> NoteReply(AttendancesRequest request);
        Task<string> ExportAttendances(string templateName, AttendancesRequest request);
        Task<BaseResponse<bool>> DailyCheck();
        Task<List<string>> GetImageList(string ImageId);

        Task<BaseResponse<AttendancesResponse>> TimeKeepingTakeAll_ByUserLogin(AttendancesRequest request);
        Task<BaseResponse<AttendancesResponse>> AttendancesCheckInTakeDetail(AttendancesRequest request);
        Task<BaseResponse<bool>> AttendancesCheckIn(AttendancesRequest request, List<HttpPostedFile> file);
        Task<BaseResponse<bool>> AttendancesCheckOut(AttendancesRequest request, List<HttpPostedFile> file);
        Task<BaseResponse<AttendancesResponse>> TakeAllAttendancesByUserId(AttendancesRequest request);
        Task<BaseResponse<bool>> EditNoteAttendances(AttendancesRequest request);
        Task<BaseResponse<AttendancesResponse>> TakeAttendanceById(int Id);
    }
    public class AttendancesService : IAttendancesService
    {
        private readonly DataContext _datacontext;
        private readonly IExportService _exportService;
        private readonly ISetting _settingService;
        private readonly ILogger _logger;
        private readonly ILogService _logService;
        private readonly IImageService _imageService;
        private readonly IImageRecordService _imageRecordService;
        public AttendancesService(DataContext datacontext, IExportService exportService, ISetting settingService, ILogger logger, ILogService logService, IImageService imageService, IImageRecordService imageRecordService)
        {
            _datacontext = datacontext;
            _exportService = exportService;
            _settingService = settingService;
            _logger = logger;
            _logService = logService;
            _imageService = imageService;
            _imageRecordService = imageRecordService;
        }
        public async Task<BaseResponse<AttendancesResponse>> TakeAlls (AttendancesRequest request )
        {
            var result = new BaseResponse<AttendancesResponse> { };
            try
            {
                var setting = _settingService.LoadSetting<MetadataSettings>();
                var startTime = TimeSpan.ParseExact(setting?.StartTime, "hh\\:mm", CultureInfo.InvariantCulture);
                var endTime = TimeSpan.ParseExact(setting?.EndTime, "hh\\:mm", CultureInfo.InvariantCulture);

                var query = _datacontext.FUV_Attendances.AsQueryable();

                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.User.DisplayName.Contains(request.Term));
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.WorkDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.WorkDate <= _ToDt);
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.StatusCode.HasValue())
                {
                    if (request.StatusCode == "Success")
                    {
                        //hoàn thành
                        query = query.Where(d => (d.CheckInTime.Value.Hour < startTime.Hours || (d.CheckInTime.Value.Hour == startTime.Hours && d.CheckInTime.Value.Minute <= startTime.Minutes)) && (d.CheckOutTime.Value.Hour > endTime.Hours || (d.CheckOutTime.Value.Hour == endTime.Hours && d.CheckOutTime.Value.Minute >= endTime.Minutes)) && !string.IsNullOrEmpty(d.CheckInData) && !string.IsNullOrEmpty(d.CheckOutData));
                    }
                    if (request.StatusCode == "BeLate")
                    {
                        //đi trễ
                        query = query.Where(d => (d.CheckInTime.Value.Hour > startTime.Hours || (d.CheckInTime.Value.Hour == startTime.Hours && d.CheckInTime.Value.Minute > startTime.Minutes)) && !string.IsNullOrEmpty(d.CheckInData) && !string.IsNullOrEmpty(d.CheckOutData));
                    }
                    if (request.StatusCode == "LeaveEarly")
                    {
                        //về sớm
                        query = query.Where(d => (d.CheckOutTime.Value.Hour < endTime.Hours || (d.CheckInTime.Value.Hour == endTime.Hours && d.CheckInTime.Value.Minute <= endTime.Minutes)) && !string.IsNullOrEmpty(d.CheckInData) && !string.IsNullOrEmpty(d.CheckOutData));
                    }
                    if (request.StatusCode == "NoTimeKeeping")
                    {
                        //không chấm công vào hoặc ra
                        query = query.Where(d => !d.CheckInTime.HasValue && !d.CheckOutTime.HasValue && string.IsNullOrEmpty(d.CheckInData) && string.IsNullOrEmpty(d.CheckOutData));
                    }
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.WorkDate).Skip(request.Page * request.PageSize).Take(request.PageSize);
               
                var data = await query.ToListAsync();
                result.Data = data.MapTo<AttendancesResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> NoteReply(AttendancesRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var attendances = await _datacontext.FUV_Attendances.FindAsync(request.Id);
                if (attendances != null)
                {
                    attendances.Accept = request.Accept;
                    if (request.Accept)
                    {
                        attendances.ReplyBy = TokenHelper.CurrentIdentity().UserName;
                    }
                    attendances.NoteReply = request.NoteReply;
                    attendances.UpdatedAt = DateTime.Now;
                    attendances.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<AttendancesResponse>> TakeExport(AttendancesRequest request)
        {
            var result = new BaseResponse<AttendancesResponse> { };
            try
            {
                var query = _datacontext.FUV_Attendances.AsQueryable();
                var setting = _settingService.LoadSetting<MetadataSettings>();
                var startTime = TimeSpan.ParseExact(setting.StartTime, "hh\\:mm", CultureInfo.InvariantCulture);
                var endTime = TimeSpan.ParseExact(setting.EndTime, "hh\\:mm", CultureInfo.InvariantCulture);

                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.User.DisplayName.Contains(request.Term));
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.WorkDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.WorkDate <= _ToDt);
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.StatusCode.HasValue())
                {
                    if (request.StatusCode == "Success")
                    {
                        //hoàn thành
                        query = query.Where(d => (d.CheckInTime.Value.Hour < startTime.Hours || (d.CheckInTime.Value.Hour == startTime.Hours && d.CheckInTime.Value.Minute <= startTime.Minutes)) && (d.CheckOutTime.Value.Hour > endTime.Hours || (d.CheckOutTime.Value.Hour == endTime.Hours && d.CheckOutTime.Value.Minute >= endTime.Minutes)) && !string.IsNullOrEmpty(d.CheckInData) && !string.IsNullOrEmpty(d.CheckOutData));
                    }
                    if (request.StatusCode == "BeLate")
                    {
                        //đi trễ
                        query = query.Where(d => (d.CheckInTime.Value.Hour > startTime.Hours || (d.CheckInTime.Value.Hour == startTime.Hours && d.CheckInTime.Value.Minute > startTime.Minutes)) && !string.IsNullOrEmpty(d.CheckInData) && !string.IsNullOrEmpty(d.CheckOutData));
                    }
                    if (request.StatusCode == "LeaveEarly")
                    {
                        //về sớm
                        query = query.Where(d => (d.CheckOutTime.Value.Hour < endTime.Hours || (d.CheckInTime.Value.Hour == endTime.Hours && d.CheckInTime.Value.Minute <= endTime.Minutes)) && !string.IsNullOrEmpty(d.CheckInData) && !string.IsNullOrEmpty(d.CheckOutData));
                    }
                    if (request.StatusCode == "NoTimeKeeping")
                    {
                        //không chấm công vào hoặc ra
                        query = query.Where(d => !d.CheckInTime.HasValue && !d.CheckOutTime.HasValue && string.IsNullOrEmpty(d.CheckInData) && string.IsNullOrEmpty(d.CheckOutData));
                    }
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.WorkDate);
                request.Page = 0;
                request.PageSize = int.MaxValue;
                var data = await query.ToListAsync();
                result.Data = data.MapTo<AttendancesResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<string> ExportAttendances(string templateName, AttendancesRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            var datas = await TakeExport(request);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _exportService.ExportToXlsxAttendances(stream, templateName, datas, request);
                bytes = stream.ToArray();
            }
            string nameFileExport = $"DanhSachQLChamCong_{DateTime.Now.Ticks}.xlsx";
            string downloadFolder = "/download/" + "QLChamCong";
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            File.WriteAllBytes(Path.Combine(exportFilePath), bytes);
            return downloadFolder + "/" + nameFileExport;
        }
        public async Task<BaseResponse<bool>> DailyCheck()
        {
            var result = new BaseResponse<bool> { };
            try
            {
                // _logger.Info("Start: Automatically check attendance every day: " + DateTime.Now.ToString("HH:mm:ss"));
                var query1 = await _datacontext.Users.Join(_datacontext.WMS_Employees, d => d.UserName, p => p.UserLogin, (d, p) => new { D = d, P = p }).Where(x => x.D.Roles == "Employee" && x.D.Active && !x.D.Deleted).ToListAsync();
                foreach (var user in query1)
                {
                    var query = await _datacontext.FUV_Attendances.FirstOrDefaultAsync(d => d.UserId == user.D.Id && d.WorkDate == DateTime.Today);
                    if (query == null)
                    {
                        var attemployee = new FUV_Attendances
                        {
                            UserId = user.D.Id,
                            LocationCode = user.P.LocationCode,
                            WorkDate = DateTime.Today,
                            CreatedAt = DateTime.Now,
                        };
                        _datacontext.FUV_Attendances.Add(attemployee);
                        await _datacontext.SaveChangesAsync();
                    }
                }
                //  _logger.Info("End: Automatically check attendance every day: " + DateTime.Now.ToString("HH:mm:ss"));
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

            ImageIdList = ImageId.Split(',')?.Select(Int32.Parse)?.ToList();
            foreach (var listId in ImageIdList)
            {
                var link = await _datacontext.ImageRecords.SingleOrDefaultAsync(d => d.Id == listId);
                result.Add(link.AbsolutePath);
            }
            return result;
        }
        public async Task<BaseResponse<AttendancesResponse>> TakeAttendanceById(int Id)
        {
            var result = new BaseResponse<AttendancesResponse> { };
            try
            {
                var data = await _datacontext.FUV_Attendances.FindAsync(Id);
                result.Item = data.MapTo<AttendancesResponse>();
                if (result.Item.LocationImagesCheckin.HasValue())
                {
                    var IdList = result.Item?.LocationImagesCheckin;
                    result.Item.ListImageCheckIn = await _imageRecordService.GetImageList(IdList);
                }
                if (result.Item.LocationImagesCheckout.HasValue())
                {
                    var IdList = result.Item?.LocationImagesCheckout;
                    result.Item.ListImageCheckOut = await _imageRecordService.GetImageList(IdList);
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

        #region[APP]
        public async Task<BaseResponse<AttendancesResponse>> TimeKeepingTakeAll_ByUserLogin(AttendancesRequest request)
        {
            var result = new BaseResponse<AttendancesResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Attendances.AsQueryable().Where(d => d.UserId == UserId);

                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.WorkDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.WorkDate <= _ToDt);
                }
                query = query.OrderByDescending(d => d.Id).Take(5);
                var setting = _settingService.LoadSetting<MetadataSettings>();
                var startTime = TimeSpan.ParseExact(setting.StartTime, "hh\\:mm", CultureInfo.InvariantCulture);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<AttendancesResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<AttendancesResponse>> AttendancesCheckInTakeDetail(AttendancesRequest request)
        {
            var result = new BaseResponse<AttendancesResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var UserName = TokenHelper.CurrentIdentity().UserName;
                var Today = DateTime.Today;
                var query = await _datacontext.FUV_Attendances.SingleOrDefaultAsync(r => r.UserId == UserId && r.WorkDate == Today);
                if (query != null)
                {
                    result.Item = query.MapTo<AttendancesResponse>();
                    if (result.Item.LocationImagesCheckin.HasValue())
                    {
                        var IdList = result.Item?.LocationImagesCheckin;
                        result.Item.ImagesCheckin = await GetImageList(IdList);
                    }
                    if (result.Item.LocationImagesCheckout.HasValue())
                    {
                        var IdList = result.Item?.LocationImagesCheckout;
                        result.Item.ImagesCheckout = await GetImageList(IdList);
                    }
                    result.Success = true;
                }
                else
                {
                    var Emloyee = await _datacontext.WMS_Employees.AsQueryable().FirstOrDefaultAsync(d => d.UserLogin == UserName);
                    if(Emloyee != null)
                    {
                        var attemployee = new FUV_Attendances
                        {
                            UserId = UserId,
                            LocationCode = Emloyee.LocationCode,
                            WorkDate = DateTime.Today,
                            CreatedAt = DateTime.Now,
                        };
                        _datacontext.FUV_Attendances.Add(attemployee);
                        await _datacontext.SaveChangesAsync();
                    }
                }
                var query2 = await _datacontext.FUV_Attendances.SingleOrDefaultAsync(r => r.UserId == UserId && r.WorkDate == Today);
                if (query2 != null)
                {
                    result.Item = query2.MapTo<AttendancesResponse>();
                    if (result.Item.LocationImagesCheckin.HasValue())
                    {
                        var IdList = result.Item?.LocationImagesCheckin;
                        result.Item.ImagesCheckin = await GetImageList(IdList);
                    }
                    if (result.Item.LocationImagesCheckout.HasValue())
                    {
                        var IdList = result.Item?.LocationImagesCheckout;
                        result.Item.ImagesCheckout = await GetImageList(IdList);
                    }
                    result.Success = true;
                }
                else
                {
                    result.Message = "No data";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        #region Chấm công vào
        public async Task<BaseResponse<bool>> AttendancesCheckIn(AttendancesRequest request, List<HttpPostedFile> file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var Today = DateTime.Today;
                var entity = await _datacontext.FUV_Attendances.SingleOrDefaultAsync(r => r.UserId == UserId && r.WorkDate == Today);
                entity.CheckInTime = DateTime.Now;
                entity.CheckInData = request.CheckInData;
                entity.LongCheckIn = request.LongCheckIn;
                entity.LatCheckIn = request.LatCheckIn;
                if (file.Count > 0)
                {
                    List<int> imagesListId = new List<int>();
                    foreach (var image in file)
                    {
                        if(image != null)
                        {
                            HttpPostedFile file_Img = null;
                            if (_imageService.CheckImageFileType(image.FileName))
                            {
                                file_Img = image;
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file_Img.InputStream.CopyTo(ms);
                                var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                                var setting = _settingService.LoadSetting<MetadataSettings>();
                                await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                imagesListId.Add(Images.Image.Id);
                            }
                        }
                    }
                    entity.LocationImagesCheckin = string.Join(",", imagesListId);
                }
                var task = _datacontext.FUV_Checkins.Add(new FUV_Checkin
                {
                    UserId = UserId,
                    Long = request.LongCheckIn,
                    Lat = request.LatCheckIn,
                    TimeCheckin = DateTime.Now,
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
        #endregion

        #region Chấm công ra
        public async Task<BaseResponse<bool>> AttendancesCheckOut(AttendancesRequest request, List<HttpPostedFile> file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var Today = DateTime.Today;
                var entity = await _datacontext.FUV_Attendances.SingleOrDefaultAsync(r => r.UserId == UserId && r.WorkDate == Today);
                entity.CheckOutTime = DateTime.Now;
                entity.CheckOutData = request.CheckOutData;
                entity.LongCheckOut = request.LongCheckOut;
                entity.LatCheckOut = request.LatCheckOut;
                if (file.Count > 0)
                {
                    List<int> imagesListId = new List<int>();
                    foreach (var image in file)
                    {
                        if(image != null)
                        {
                            HttpPostedFile file_Img = null;
                            if (_imageService.CheckImageFileType(image.FileName))
                            {
                                file_Img = image;
                            }
                            using (MemoryStream ms = new MemoryStream())
                            {
                                file_Img.InputStream.CopyTo(ms);
                                var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                                var setting = _settingService.LoadSetting<MetadataSettings>();
                                await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                                imagesListId.Add(Images.Image.Id);
                            }
                        }
                    }
                    entity.LocationImagesCheckout = string.Join(",", imagesListId);
                }
                var task = _datacontext.FUV_Checkins.Add(new FUV_Checkin
                {
                    UserId = UserId,
                    Long = request.LongCheckOut,
                    Lat = request.LatCheckOut,
                    TimeCheckin = DateTime.Now,
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
        #endregion

        #region Thống kê chấm công
        public async Task<BaseResponse<AttendancesResponse>> TakeAllAttendancesByUserId(AttendancesRequest request)
        {
            var result = new BaseResponse<AttendancesResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Attendances.Where(x => x.UserId == UserId);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.WorkDate);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.WorkDate <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.WorkDate).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<AttendancesResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<BaseResponse<bool>> EditNoteAttendances(AttendancesRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Attendances.SingleOrDefaultAsync(r => r.Id == request.Id);
                entity.Note = request.Note;
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
        #endregion

        #endregion
    }
}
