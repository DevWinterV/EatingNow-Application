using DaiPhucVinh.Api;
using DaiPhucVinh.Services.MainServices.Catalogue;
using DaiPhucVinh.Services.MainServices.CategoryItem;
using DaiPhucVinh.Services.MainServices.Customer;
using DaiPhucVinh.Services.MainServices.Post;
using DaiPhucVinh.Services.MainServices.Product;
using DaiPhucVinh.Services.MainServices.ProjectImages;
using DaiPhucVinh.Services.MainServices.UserGuides;
using DaiPhucVinh.Shared.Catalogue;
using DaiPhucVinh.Shared.CategoryItem;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Notification;
using DaiPhucVinh.Shared.Post;
using DaiPhucVinh.Shared.Product;
using DaiPhucVinh.Shared.ProjectImages;
using DaiPhucVinh.Shared.UserGuides;
using DaiPhucVinh.Shared.Video;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCheck.WebUI.ApiApp
{
    [RoutePrefix("xapi/everyoneOperation")]
    public class EveryoneOperationController : BaseApiController
	{
        private readonly ICategoryItemService _categoryItemService;
        private readonly IProductService _productService;
        private readonly IUserGuidesService _userGuidesService;
        private readonly ICatalogueService _catalogueService;
        private readonly IProjectImagesService _projectImagesService;
        private readonly ICustomerService _customerService;


        public EveryoneOperationController(
            ICategoryItemService categoryItemService,
            IProductService productService,
            IUserGuidesService userGuidesService,
            ICatalogueService catalogueService,
            IProjectImagesService projectImagesService,
            ICustomerService customerService)
        {
            _categoryItemService = categoryItemService;
            _productService = productService;
            _userGuidesService = userGuidesService;
            _catalogueService = catalogueService;
            _projectImagesService = projectImagesService;
            _customerService = customerService;
        }

        #region [Danh mục sản phẩm]
        [HttpPost]
        [Route("categoryItemTakeAll")]
        public async Task<BaseResponse<CategoryItemResponse>> TakeAllCategoryItem(CategoryItemRequest request) => await _categoryItemService.TakeAllCategoryItem(request);

        //[HttpPost]
        //[Route("categoryItemTakeGroup")]
        //public async Task<BaseResponse<ProductDto>> TakeGroupCategoryItem(ProductRequest request) => await _categoryItemService.TakeGroupCategoryItemByCode(request);

        [HttpPost]
        [Route("productTakeByGroupCode")]
        public async Task<BaseResponse<ProductDto>> TakeProductByGroupCode([FromBody] ProductRequest request) => await _productService.TakeProductByGroupCode(request);

        [HttpPost]
        [Route("takeProductDetailByCode")]
        public async Task<BaseResponse<ProductDto>> TakeProductDetailByCode(ProductRequest request) => await _productService.TakeProductDetailByCode(request);

        #endregion

        #region [Thông tin thêm]

        #region Ảnh công trình
        [HttpPost]
        [Route("projectImageTakeAll")]
        public async Task<BaseResponse<ProjectImagesResponse>> TakeAllProjectImages(ProjectImageRequest request) => await _projectImagesService.TakeAllProjectImagesForMobile(request);
        #endregion

        #region Catalogue
        [HttpPost]
        [Route("catalogueTakeAll")]
        public async Task<BaseResponse<CatalogueDto>> TakeAllCatalogues(CatalogueRequest request) => await _catalogueService.TakeAllCataloguesForMobile(request);
        #endregion

        #region Hướng dẫn sử dụng
        [HttpPost]
        [Route("userGuideTakeAll")]
        public async Task<BaseResponse<UserGuidesResponse>> TakeAllUserGuides(UserGuidesRequest request) => await _userGuidesService.TakeAllUserGuidesForMobile(request);
        #endregion

        #endregion

        #region Load data (Role: Customer)

        #region List product 
        [HttpPost]
        [Route("productTakeByCustomerCode")]
        public async Task<BaseResponse<ProductByCustomerDto>> TakeProductByCustomerCode(HopDongRequest request) => await _customerService.TakeProductByCustomerCode(request);
        #endregion

        #region Search Product
        [HttpPost]
        [Route("searchProduct")]
        public async Task<BaseResponse<ProductDto>> SearchProducts(ProductRequest request) => await _productService.SearchProducts(request);
        #endregion

        #region List New Product
        [HttpPost]
        [Route("newProduct")]
        public async Task<BaseResponse<ProductDto>> NewProducts(ProductRequest request) => await _productService.NewProducts(request);
        #endregion

        #endregion

        #region [Notification]
        [HttpPost]
        [Route("takeNotification")]
        public async Task<BaseResponse<NotificationResponse>> TakeNotifications(NotificationRequest request) => await _productService.TakeNotifications(request);
        [HttpPost]
        [Route("updateStatusNotification")]
        public async Task<BaseResponse<bool>> UpdateStatusNotifications(NotificationRequest request) => await _productService.UpdateStatusNotifications(request);
        #endregion
    }
}