using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.ImageRecords;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/imagerecord")]
    
    public class ImageRecordController : SecureApiController
    {
        private readonly IImageRecordService _imageRecordService;
        private readonly IImageService _imageService;
        public ImageRecordController(IImageRecordService imageRecordService, IImageService imageService)
        {
            _imageRecordService = imageRecordService;
            _imageService = imageService;
        }
        [HttpPost]
        [Route("TakeAllImages")]
        public async Task<BaseResponse<ImageRecordsResponse>> TakeAllImages([FromBody] ImageRecordsRequest request) => await _imageRecordService.TakeAllImages(request);

        [HttpPost]
        [Route("CreateImage")]
        public async Task<BaseResponse<bool>> CreateImage()
        {

            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile img = null;

            if (httpRequest.Files.Count > 0)
            {
                img = httpRequest.Files[0];
                return await _imageRecordService.CreateImage(img);
            }
            else return new BaseResponse<bool>
            {
                Success = false,
                Message = "Form not found!"
            };
        }
        [HttpPost]
        [Route("RemoveImage")]
        public async Task RemoveImage([FromBody] ImageRecordsRequest request) => await _imageService.DeleteImage(request.Id);

    }
}