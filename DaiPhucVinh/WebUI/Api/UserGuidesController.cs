using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.UserGuides;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.UserGuides;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCheck.WebUI.Api
{
    [RoutePrefix("api/userguides")]
    public class UserGuidesController : SecureApiController
    {
        private readonly IUserGuidesService _userGuidesService;
        private readonly IImageService _imageService;
        public UserGuidesController(IUserGuidesService userGuidesService, IImageService imageService)
        {
            _userGuidesService = userGuidesService;
            _imageService = imageService;
        }

        [HttpPost]
        [Route("TakeAllUserGuides")]
        public async Task<BaseResponse<UserGuidesResponse>> TakeAllUserGuides([FromBody] UserGuidesRequest request) => await _userGuidesService.TakeAllUserGuides(request);

        [HttpGet]
        [Route("TakeUserGuidesById")]
        public async Task<BaseResponse<UserGuidesResponse>> TakeUserGuidesById(int Id) => await _userGuidesService.TakeUserGuidesById(Id);

        [HttpPost]
        [Route("CreateUserGuides")]
        public async Task<BaseResponse<bool>> CreateUserGuides()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile fileImg = null;
            HttpPostedFile filePdf = null;
            //file
            if (httpRequest.Files.Count > 0)
            {
                for (var i = 0; i < httpRequest.Files.Count; i++)
                {
                    if (httpRequest.Files[i].ContentType == "application/pdf")
                    {
                        filePdf = httpRequest.Files[i];
                    }
                    if (_imageService.CheckImageFileType(httpRequest.Files[i].FileName))
                    {
                        fileImg = httpRequest.Files[i];
                    }

                }

            }
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<UserGuidesRequest>(jsonRequest);
                return await _userGuidesService.CreateUserGuides(fileImg, filePdf, request);
            }

            return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("UpdateUserGuides")]
        public async Task<BaseResponse<bool>> UpdateUserGuides()
        {
            var httpRequest = HttpContext.Current.Request;
            HttpPostedFile fileImg = null;
            HttpPostedFile filePdf = null;
            //file
            if (httpRequest.Files.Count > 0)
            {
                for (var i = 0; i < httpRequest.Files.Count; i++)
                {
                    if (httpRequest.Files[i].ContentType == "application/pdf")
                    {
                        filePdf = httpRequest.Files[i];
                    }
                    if (_imageService.CheckImageFileType(httpRequest.Files[i].FileName))
                    {
                        fileImg = httpRequest.Files[i];
                    }

                }

            }
            if (httpRequest.Form.Count > 0)
            {
                var jsonRequest = httpRequest.Form[0];
                var request = JsonConvert.DeserializeObject<UserGuidesRequest>(jsonRequest);
                return await _userGuidesService.UpdateUserGuides(fileImg, filePdf, request);
            }

            return new BaseResponse<bool>
            {
                Success = false,
                Message = "File not found!"
            };
        }
        [HttpPost]
        [Route("RemoveUserGuides")]
        public async Task<BaseResponse<bool>> RemoveUserGuides([FromBody] UserGuidesRequest request) => await _userGuidesService.RemoveUserGuides(request);

    }
}