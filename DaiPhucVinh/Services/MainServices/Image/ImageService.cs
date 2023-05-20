using Falcon.Web.Core.Log;
using Falcon.Web.Core.Settings;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using ImageResizer;
using System.Web;
using DaiPhucVinh.Shared.Common.Image;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Services.Database;
using System.Collections.Generic;
using Falcon.Core.Infrastructure;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Shared.Common;

namespace DaiPhucVinh.Services.MainServices.Image
{
    public interface IImageService
    {
        string HostAddress { get; }

        Size CalculateDimensions(Size originalSize, int targetSize, bool ensureSizePositive = true);
        bool CheckImageFileType(string fileName);
        Task DeleteImage(int id);
        string GenAbsolutePath(string relativePath);
        Task<ImageResponse> InsertImage(byte[] pictureBinary, string filename, string storeName = "DaiPhucVinh", bool imagetype = false);
        Task<ImageResponse> InsertImageWithServerSetting(byte[] pictureBinary, string filename, string storeName = "DaiPhucVinh", string cdnServer="", bool isMain = false);
        Task<ImageResponse> InsertFile(byte[] pictureBinary, string filename, string storeName = "DaiPhucVinh");

        Task ResizeImage(int imageId, int size = 0);
        Task ResizeImageHaveStoreName(int imageId, int size = 0, string storeName = "DaiPhucVinh");
        ImageResponse SaveThumbnail(int id);
        string ThumbnailName(string nameImage);
        void WriteOverrideImage(int Id, byte[] content, string storeName = "DaiPhucVinh");
    }

    public class ImageService : IImageService
    {
        const string SubPath = "\\uploads\\DaiPhucVinh\\Image";
        private readonly ILogger _logger;
        private readonly ISettingService _settingService;
        private readonly DataContext _dataContext;

        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");
        public ImageService(ILogger logger,
            ISettingService settingService,
            DataContext dataContext)
        {
            _logger = logger;
            _settingService = settingService;
            _dataContext = dataContext;

        }

        public async Task<ImageResponse> InsertImage(byte[] pictureBinary, string filename, string storeName = "SwiftletReport", bool imagetype = false) //imagetype = true is check load to web
        {
            var image = new ImageRecord();
            try
            {
                var storageFolder = $@"\uploads\{storeName}";
                if (!Directory.Exists(LocalMapPath(storageFolder)))
                    Directory.CreateDirectory(LocalMapPath(storageFolder));

                #region validate
                if (pictureBinary == null || pictureBinary.Length < 0)
                    return new ImageResponse() { Message = "File empty" };
                if (string.IsNullOrEmpty(filename))
                    return new ImageResponse() { Message = "File" };
                filename = Path.GetFileName(filename);
                if (!CheckImageFileType(filename))
                    return new ImageResponse() { Message = "Image file type: .jpg, .png" };
                #endregion
                string newFileName = $"{Path.GetFileNameWithoutExtension(filename)}" + "-" + $"{DateTime.Now.Ticks}{Path.GetExtension(filename)}";
                var storageFolderPath = Path.Combine(LocalMapPath(storageFolder), newFileName);
                File.WriteAllBytes(storageFolderPath, pictureBinary);

                var relativePath = Path.Combine(storageFolder, newFileName);

                image = new ImageRecord()
                {
                    AbsolutePath = HostAddress + GenAbsolutePath(relativePath),
                    RelativePath = relativePath,
                    IsUsed = true,
                    IsExternal = false,
                    CreatedAt = DateTime.Now,
                    FileName = newFileName,
                    IsWeb = imagetype,
                };

                _dataContext.ImageRecords.Add(image);
                await _dataContext.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {

            }
            return new ImageResponse()
            {
                IsOk = true,
                Message = "Success",
                Image = image.MapTo<ImageDto>()
            };
        }

        public async Task<ImageResponse> InsertImageWithServerSetting(byte[] pictureBinary, string filename, string storeName = "SwiftletReport", string cdnServer = "", bool isMain = false)
        {
            var image = new ImageRecord();
            try
            {
                var storageFolder = $@"\uploads\{storeName}";
                if (!Directory.Exists(LocalMapPath(storageFolder)))
                    Directory.CreateDirectory(LocalMapPath(storageFolder));

                #region validate
                if (pictureBinary == null || pictureBinary.Length < 0)
                    return new ImageResponse() { Message = "File empty" };
                if (string.IsNullOrEmpty(filename))
                    return new ImageResponse() { Message = "File" };
                filename = Path.GetFileName(filename);
                if (!CheckImageFileType(filename))
                    return new ImageResponse() { Message = "Image file type: .jpg, .png" };
                #endregion
                string newFileName = $"{Path.GetFileNameWithoutExtension(filename)}" + "-" + $"{DateTime.Now.Ticks}{Path.GetExtension(filename)}";
                var storageFolderPath = Path.Combine(LocalMapPath(storageFolder), newFileName);
                File.WriteAllBytes(storageFolderPath, pictureBinary);

                var relativePath = Path.Combine(storageFolder, newFileName);

                image = new ImageRecord()
                {
                    AbsolutePath = cdnServer + GenAbsolutePath(relativePath),
                    RelativePath = relativePath,
                    IsUsed = true,
                    IsExternal = false,
                    CreatedAt = DateTime.Now,
                    FileName = newFileName,
                    IsMain = isMain
                };

                _dataContext.ImageRecords.Add(image);
                await _dataContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
            return new ImageResponse()
            {
                IsOk = true,
                Message = "Success",
                Image = image.MapTo<ImageDto>()
            };
        }
        public async Task<ImageResponse> InsertFile(byte[] pictureBinary, string filename, string storeName = "SwiftletReport")
        {
            var storageFolder = $@"\uploads\{storeName}";
            if (!Directory.Exists(LocalMapPath(storageFolder)))
                Directory.CreateDirectory(LocalMapPath(storageFolder));
            
                
            #region validate
            if (pictureBinary == null || pictureBinary.Length < 0)
                return new ImageResponse() { Message = "File empty" };
            if (string.IsNullOrEmpty(filename))
                return new ImageResponse() { Message = "File" };
            filename = Path.GetFileName(filename);

            #endregion
            string newFileName = $"{DateTime.Now.Ticks}{Path.GetExtension(filename)}";
            var storageFolderPath = Path.Combine(LocalMapPath(storageFolder), filename);
            File.WriteAllBytes(storageFolderPath, pictureBinary);

            var relativePath = Path.Combine(storageFolder, filename);

            //path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            //return Path.Combine(baseDirectory, path);
            var image = new ImageRecord()
            {
                AbsolutePath = HostAddress + GenAbsolutePath(relativePath),
                RelativePath = relativePath,
                IsUsed = true,
                IsExternal = false,
                CreatedAt = DateTime.Now,
                FileName = filename,
            };
            _dataContext.ImageRecords.Add(image);
            await _dataContext.SaveChangesAsync();
            return new ImageResponse()
            {
                IsOk = true,
                Message = "Success",
                Image = image.MapTo<ImageDto>()
            };
        }
        public string GenAbsolutePath(string relativePath)
        {
            var systemSettings = _settingService.LoadSetting<SystemSettings>();
            var path = systemSettings.Domain + relativePath.Replace("\\", "/");
            path = path.Replace("//", "/");
            return path;
        }
        public async Task DeleteImage(int id)
        {
            var image = await _dataContext.ImageRecords.FindAsync(id);
            if (image == null || image.Deleted)
                return;
            image.Deleted = true;
            await _dataContext.SaveChangesAsync();
            var imageFilePath = image.RelativePath;
            if (string.IsNullOrEmpty(imageFilePath))
                return;
            var path = LocalMapPath(imageFilePath);
            if (!File.Exists(path))
                return;
            File.Delete(path);
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

        public ImageResponse SaveThumbnail(int id)
        {
            var image = _dataContext.ImageRecords.Find(id);
            if (image == null || image.Deleted)
                return new ImageResponse()
                {
                    Message = "Image not found"
                };

            var filePath = Path.Combine(LocalMapPath(SubPath), image.FileName);

            if (!File.Exists(filePath))
                return new ImageResponse()
                {
                    Message = "Image not found"
                };

            var fileImage = File.ReadAllBytes(filePath);
            var thumbnailName = ThumbnailName(image.FileName);
            var thumbnailPath = Path.Combine(LocalMapPath(SubPath), thumbnailName);
            using (var stream = new MemoryStream(fileImage))
            {
                Bitmap b = null;
                try
                {
                    //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                    b = new Bitmap(stream);
                }
                catch (ArgumentException exc)
                {
                    _logger.Error($"Error generating picture thumb. ID={image.Id}", exc.Message);
                }
                if (b == null)
                {
                    //bitmap could not be loaded for some reasons
                    _logger.Error("bitmap could not be loaded for some reasons", "bitmap could not be loaded for some reasons");
                    throw new ArgumentNullException(nameof(b));
                }
                using (var destStream = new MemoryStream())
                {
                    var mediaSettings = _settingService.LoadSetting<MediaSettings>();
                    var newSize = CalculateDimensions(b.Size, mediaSettings.TargetResize);
                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                    {
                        Width = newSize.Width,
                        Height = newSize.Height,
                        Scale = ScaleMode.Both,
                        Quality = 80
                    });
                    var destBinary = destStream.ToArray();
                    File.WriteAllBytes(thumbnailPath, destBinary);

                    b.Dispose();
                }
            }

            var relativePath = Path.Combine(SubPath, thumbnailName);
            var thumb = new ImageRecord()
            {
                AbsolutePath = GenAbsolutePath(relativePath),
                RelativePath = relativePath,
                IsUsed = true,
                IsExternal = false,
                CreatedAt = DateTime.Now,
                FileName = thumbnailName,
            };
            _dataContext.ImageRecords.Add(thumb);
            _dataContext.SaveChanges();
            return new ImageResponse()
            {
                IsOk = true,
                Message = "Success",
                Image = thumb.MapTo<ImageDto>()
            };
        }
        public string ThumbnailName(string nameImage)
        {
            return $"thumb_{nameImage}";
        }

        public Size CalculateDimensions(Size originalSize, int targetSize, bool ensureSizePositive = true)
        {
            float width, height;
            if (originalSize.Height > originalSize.Width)
            {
                // portrait
                width = originalSize.Width * (targetSize / (float)originalSize.Height);
                height = targetSize;
            }
            else
            {
                // landscape or square
                width = targetSize;
                height = originalSize.Height * (targetSize / (float)originalSize.Width);
            }
            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }

        public async Task ResizeImage(int imageId, int size = 0)
        {
            var image = await _dataContext.ImageRecords.FindAsync(imageId);
            if (image == null || image.Deleted)
                return;
            if (size <= 0)
            {
                var mediaSettings = _settingService.LoadSetting<MediaSettings>();
                size = mediaSettings.TargetResize;
            }
            var filePath = Path.Combine(LocalMapPath(SubPath), image.FileName);
            var fileImage = File.ReadAllBytes(filePath);
            using (var stream = new MemoryStream(fileImage))
            {
                Bitmap b = null;
                try
                {
                    //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                    b = new Bitmap(stream);
                }
                catch (ArgumentException exc)
                {
                    _logger.Error($"Error generating picture thumb. ID={image.Id}", exc.Message);
                }
                if (b == null)
                {
                    //bitmap could not be loaded for some reasons
                    _logger.Error("bitmap could not be loaded for some reasons", "bitmap could not be loaded for some reasons");
                    throw new ArgumentNullException(nameof(b));
                }
                if (b.Size.Width == size)
                    return;

                using (var destStream = new MemoryStream())
                {
                    var newSize = CalculateDimensions(b.Size, size);
                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                    {
                        Width = newSize.Width,
                        Height = newSize.Height,
                        Scale = ScaleMode.Both,
                        Quality = 80
                    });
                    var destBinary = destStream.ToArray();
                    File.WriteAllBytes(filePath, destBinary);

                    b.Dispose();
                }
            }
        }

        public void WriteOverrideImage(int Id, byte[] content, string storeName = "SwiftletReport")
        {
            var storageFolder = $@"\uploads\{storeName}";
            var oldImage = _dataContext.ImageRecords.Find(Id);
            var imgPath = Path.Combine(LocalMapPath(storageFolder), oldImage.FileName);

            if (File.Exists(imgPath))
                File.Delete(imgPath);
            string newFileName = $"{DateTime.Now.Ticks}{Path.GetExtension(oldImage.FileName)}";
            oldImage.AbsolutePath = oldImage.AbsolutePath.Replace(oldImage.FileName, newFileName);
            oldImage.RelativePath = oldImage.RelativePath.Replace(oldImage.FileName, newFileName);
            oldImage.FileName = newFileName;
            imgPath = Path.Combine(LocalMapPath(storageFolder), newFileName);
            _dataContext.SaveChanges();
            File.WriteAllBytes(imgPath, content);
        }
        public async Task ResizeImageHaveStoreName(int imageId, int size = 0, string storeName = "DaiPhucVinh")
        {
            var storageFolder = $@"\uploads\{storeName}";
            if (!Directory.Exists(LocalMapPath(storageFolder)))
                Directory.CreateDirectory(LocalMapPath(storageFolder));

            var image = await _dataContext.ImageRecords.FindAsync(imageId);
            if (image == null || image.Deleted)
                return;
            if (size <= 0)
            {
                var mediaSettings = _settingService.LoadSetting<MediaSettings>();
                size = mediaSettings.TargetResize;
            }
            var filePath = Path.Combine(LocalMapPath(storageFolder), image.FileName);
            var fileImage = File.ReadAllBytes(filePath);
            using (var stream = new MemoryStream(fileImage))
            {
                Bitmap b = null;
                try
                {
                    //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                    b = new Bitmap(stream);
                }
                catch (ArgumentException exc)
                {
                    _logger.Error($"Error generating picture thumb. ID={image.Id}", exc.Message);
                }
                if (b == null)
                {
                    //bitmap could not be loaded for some reasons
                    _logger.Error("bitmap could not be loaded for some reasons", "bitmap could not be loaded for some reasons");
                    throw new ArgumentNullException(nameof(b));
                }
                if (b.Size.Width == size)
                    return;

                using (var destStream = new MemoryStream())
                {
                    var newSize = CalculateDimensions(b.Size, size);
                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                    {
                        Width = newSize.Width,
                        Height = newSize.Height,
                        Scale = ScaleMode.Both,
                        Quality = 80
                    });
                    var destBinary = destStream.ToArray();
                    File.WriteAllBytes(filePath, destBinary);

                    b.Dispose();
                }
            }
        }

        
    }
}
