using Falcon.Web.Core.Caching;
using Falcon.Web.Core.Infrastructure;
using Falcon.Web.Core.Log;
using Falcon.Web.Core.Security;
using Falcon.Web.Core.Settings;
using SimpleInjector;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.MainServices;
using DaiPhucVinh.Services.MainServices.Auth;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.Customer;
using DaiPhucVinh.Services.MainServices.Product;
using DaiPhucVinh.Services.MainServices.Location;
using DaiPhucVinh.Services.MainServices.Quotation;
using DaiPhucVinh.Services.MainServices.HopDong;
using DaiPhucVinh.Services.MainServices.Province;
using DaiPhucVinh.Services.MainServices.TransactionInformation;
using DaiPhucVinh.Services.Excel;
using DaiPhucVinh.Services.MainServices.Word;
using DaiPhucVinh.Services.MainServices.LoaiDanhGia;
using DaiPhucVinh.Services.MainServices.InventoryItem;
using DaiPhucVinh.Services.MainServices.Chart_DashBoarb;
using DaiPhucVinh.Services.MainServices.RevenueStatistics;
using DaiPhucVinh.Services.MainServices.User;
using DaiPhucVinh.Services.MainServices.Dayoffs;
using DaiPhucVinh.Services.MainServices.Post;
using DaiPhucVinh.Services.MainServices.Tasks;
using DaiPhucVinh.Services.MainServices.Employee;
using DaiPhucVinh.Services.MainServices.Attendances;
using DaiPhucVinh.Services.MainServices.ProjectImages;
using DaiPhucVinh.Services.MainServices.Catalogue;
using DaiPhucVinh.Services.MainServices.Feedback;
using DaiPhucVinh.Services.MainServices.UserGuides;
using DaiPhucVinh.Services.MainServices.Notification;
using DaiPhucVinh.Services.MainServices.CategoryItem;
using DaiPhucVinh.Services.MainServices.Checkin;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Services.MainServices.UnitOfMeasure;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.MainServices.ChatRoom;
using DaiPhucVinh.Services.MainServices.Report;
using DaiPhucVinh.Services.MainServices.Quality;
using DaiPhucVinh.Services.MainServices.WorkLog;
using DaiPhucVinh.Services.PushNotification;
using DaiPhucVinh.Services.MainServices.Cuisine;
using DaiPhucVinh.Services.MainServices.District;
using DaiPhucVinh.Services.MainServices.CategoryList;
using DaiPhucVinh.Services.MainServices.FoodList;
using DaiPhucVinh.Services.MainServices.OrderHeader;
using DaiPhucVinh.Services.MainServices.EN_CustomerService;
using Microsoft.ML;
using Microsoft.AspNet.SignalR;
using DaiPhucVinh.Services.MainServices.Hubs;
using Microsoft.AspNet.SignalR.Hubs;

namespace DaiPhucVinh.Services.Infrastructure
{
    public static class DependencyRegistra
    {
        public static void Register(Container container)
        {
            EngineContext.Current.Init(new SimpleContainer(container));
            //DB
            container.Register<DataContext>(Lifestyle.Scoped);
            container.Register<FesContext>(Lifestyle.Scoped);
            //Framework
            container.Register<ILogger, Logger>(Lifestyle.Scoped);
            container.Register<ITokenValidation, TokenValidation>(Lifestyle.Scoped);
            container.Register<ICacheManager, MemoryCacheManager>(Lifestyle.Scoped);
            container.Register<ISettingService, SettingService>(Lifestyle.Scoped);
            container.Register<ISetting, SettingService>(Lifestyle.Scoped);
            container.Register<IEncryptionService, EncryptionService>(Lifestyle.Scoped);
            container.Register<IAuthService, AuthService>(Lifestyle.Scoped);
            container.Register<ILogService, LogService>(Lifestyle.Scoped);
            container.Register<IImageService, ImageService>(Lifestyle.Scoped);
            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<IMailMergeService, MailMergeService>(Lifestyle.Scoped);
            container.Register<IWordStreamService, WordStreamService>(Lifestyle.Scoped);
            container.Register<IHub, OrderNotificationHub>(Lifestyle.Scoped);

            //External
            container.Register<IChartService, ChartService>(Lifestyle.Scoped);
            container.Register<IChatRoomService, ChatRoomService>(Lifestyle.Scoped);
            container.Register<ICustomerService, CustomerService>(Lifestyle.Scoped);
            container.Register<IExportService, ExportService>(Lifestyle.Scoped);
            container.Register<IProductService, ProductService>(Lifestyle.Scoped);
			container.Register<ILocationService, LocationService>(Lifestyle.Scoped);
			container.Register<IQuotationService, QuotationService>(Lifestyle.Scoped);
            container.Register<IHopDongService, HopDongService>(Lifestyle.Scoped);
            container.Register<ITransactionInformationService, TransactionInformationService>(Lifestyle.Scoped);
            container.Register<ILoaiDanhGiaService, LoaiDanhGiaService>(Lifestyle.Scoped);
            container.Register<IInventoryItemService, InventoryItemService>(Lifestyle.Scoped);
            container.Register<IRevenueStatisticsService, RevenueStatisticsService>(Lifestyle.Scoped);
            container.Register<IPostService, PostService>(Lifestyle.Scoped); 
            container.Register<IDayoffsService, DayoffsService>(Lifestyle.Scoped);
            container.Register<ITaskService, TaskService>(Lifestyle.Scoped);
            container.Register<ITaskStatusService, TaskStatusService>(Lifestyle.Scoped);
            container.Register<ITaskTypeService, TaskTypeService>(Lifestyle.Scoped);
            container.Register<ITaskResultService, TaskResultService>(Lifestyle.Scoped);
            container.Register<IEmployeeService, EmployeeService>(Lifestyle.Scoped);
            container.Register<IAttendancesService, AttendancesService>(Lifestyle.Scoped);
            container.Register<IProjectImagesService, ProjectImagesService>(Lifestyle.Scoped);
            container.Register<ICatalogueService, CatalogueService>(Lifestyle.Scoped);
            container.Register<IFeedbackService, FeedbackService>(Lifestyle.Scoped);
            container.Register<IUserGuidesService, UserGuidesService>(Lifestyle.Scoped);
            container.Register<INotificationService, NotificationService>(Lifestyle.Scoped);
            container.Register<INotificationTypeService, NotificationTypeService>(Lifestyle.Scoped);
            container.Register<INotificationGroupService, NotificationGroupService>(Lifestyle.Scoped);
            container.Register<ICategoryItemService, CategoryItemService>(Lifestyle.Scoped);
            container.Register<ICheckinService, CheckinService>(Lifestyle.Scoped);
            container.Register<IImageRecordService, ImageRecordService>(Lifestyle.Scoped);
            container.Register<IUnitOfMeasureService, UnitOfMeasureService>(Lifestyle.Scoped);
            container.Register<IRoleService, RoleService>(Lifestyle.Scoped);
            container.Register<ICommonService, CommonService>(Lifestyle.Scoped);
            container.Register<IReportService, ReportService>(Lifestyle.Scoped);
            container.Register<IQualityService, QualityService>(Lifestyle.Scoped);
            container.Register<IWorkLogService, WorkLogService>(Lifestyle.Scoped);
            container.Register<IPushMessageService, PushMessageService>(Lifestyle.Scoped);

            container.Register<ICuisineService, CuisineService>(Lifestyle.Scoped);
            container.Register<IProvinceService, ProvinceService>(Lifestyle.Scoped);
            container.Register<IDistrictService, DistrictService>(Lifestyle.Scoped);
            container.Register<IWardService, WardService>(Lifestyle.Scoped);
            container.Register<IAccountTypeService, AccountTypeService>(Lifestyle.Scoped);
            container.Register<IStoreService, StoreService>(Lifestyle.Scoped);
            container.Register<ICategoryListService, CategoryListService>(Lifestyle.Scoped);
            container.Register<IFoodListService, FoodListService>(Lifestyle.Scoped);

            container.Register<IOrderHeaderService, OrderHeaderService>(Lifestyle.Scoped);
            container.Register<IENCustomerService, ENCustomerService>(Lifestyle.Scoped);

        }
        public static void ApiServerRegister(Container container)
        {
            Register(container);
        }
    }
}