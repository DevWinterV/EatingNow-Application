using DaiPhucVinh.Api;
using System.Threading.Tasks;
using DaiPhucVinh.Shared.Post;
using System.Web.Http;
using DaiPhucVinh.Services.MainServices.Post;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Shared.Common;

namespace PCheck.WebUI.ApiApp
{
    [RoutePrefix("xapi/news")]
    public class NewsController : BaseApiController
    {
        private readonly IPostService _postService;
        private readonly IImageService _imageService;
        public NewsController(IPostService postService, IImageService imageService)
        {
            _postService = postService;
            _imageService = imageService;
        }
        [HttpPost]
        [Route("")]
        public object AllNews()
        {
            return new
            {
                Ok = "true"
            };
        }

        #region Take Data for mobile

        #region Take Post
        [HttpPost]
        [Route("TakeAllPostsForMobile")]
        public async Task<BaseResponse<PostMobileResponse>> TakeAllPostsForMobile(PostMobileRequest request) => await _postService.TakeAllPostsForMobile(request);
        #endregion

        #endregion
    }
}