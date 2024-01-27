using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using System.Data.Entity;

namespace DaiPhucVinh.Services.Database
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataContext")
        {
        }


        #region Datasets

        public DbSet<ImageRecord> ImageRecords { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }

        #endregion

        public DbSet<WMS_Customers> WMS_Customers { get; set; }
        public DbSet<WMS_CustomersType> WMS_CustomersTypes { get; set; }
        public DbSet<WMS_Employee> WMS_Employees { get; set; }
        public DbSet<WMS_HopDong> WMS_HopDongs { get; set; }
        public DbSet<WMS_HopDong_Templete> WMS_HopDong_Templetes { get; set; }
        public DbSet<WMS_Item> WMS_Items { get; set; }
        public DbSet<WMS_Item_Image> WMS_Item_Images { get; set; }
        public DbSet<WMS_ItemGroup> WMS_ItemGroups { get; set; }
        public DbSet<WMS_Location> WMS_Locations { get; set; }
        public DbSet<WMS_Quotation> WMS_Quotations { get; set; }
        public DbSet<WMS_Quotation_Templete> WMS_Quotation_Templetes { get; set; }
        public DbSet<WMS_QuotationLine> WMS_QuotationLines { get; set; }
        public DbSet<WMS_TransactionType> WMS_TransactionTypes { get; set; }
        public DbSet<WMS_Province> WMS_Provinces { get; set; }
        public DbSet<WMS_TransactionInformation> WMS_TransactionInformations { get; set; }
        public DbSet<WMS_HopDong_ChiTiet> WMS_HopDong_ChiTiets { get; set; }
        public DbSet<WMS_UnitPrice> WMS_UnitPrices { get; set; }
        public DbSet<WMS_LoaiDanhGia> WMS_LoaiDanhGias { get; set; }
        public DbSet<WMS_InventoryItem> WMS_InventoryItems { get; set; }
        public DbSet<FUV_Posts> FUV_Posts { get; set; }
        public DbSet<FUV_Videos> FUV_Videos { get; set; }
        public DbSet<FUV_Dayoffs> FUV_Dayoffs { get; set; }
        public DbSet<FUV_TaskStatus> FUV_TaskStatus { get; set; }
        public DbSet<FUV_Tasks> FUV_Tasks { get; set; }
        public DbSet<FUV_TaskTypes> FUV_TaskTypes { get; set; }
        public DbSet<FUV_TaskResult> FUV_TaskResults { get; set; }
        public DbSet<FUV_Attendances> FUV_Attendances { get; set; }
        public DbSet<FUV_Catalogs> FUV_Catalogs { get; set; }
        public DbSet<FUV_ProjectImages> fUV_ProjectImages { get; set; }
        public DbSet<FUV_Feedbacks> FUV_Feedbacks { get; set; }
        public DbSet<FUV_UserGuides> FUV_UserGuides { get; set; }
        public DbSet<FUV_Notifications> FUV_Notifications { get; set; }
        public DbSet<FUV_NotificationTypes> FUV_NotificationTypes { get; set; }
        public DbSet<FUV_NotificationGroups> FUV_NotificationGroups { get; set; }
        public DbSet<WMS_ItemCategory> WMS_ItemCategories { get; set; }
        public DbSet<FUV_Checkin> FUV_Checkins { get; set; }
        public DbSet<WMS_UnitOfMeasure> WMS_UnitOfMeasures { get; set; }
        public DbSet<WMS_AutoGenCodeConfig> WMS_AutoGenCodeConfigs { get; set; }
        public DbSet<FUV_ChatRoom> FUV_ChatRooms { get; set; }
        public DbSet<FUV_ChatDescriptions> FUV_ChatDescriptions { get; set; }
        public DbSet<FUV_ChatRoomMember> FUV_ChatRoomMembers { get; set; }
        public DbSet<FUV_WorkLog> FUV_WorkLogs { get; set; }
        public DbSet<FSW_ChatLuong> FSW_ChatLuongs { get; set; }
        public DbSet<FUV_MobileDevices> FUV_MobileDevice { get; set; }


        public DbSet<EN_Cuisine> EN_Cuisine { get; set; }
        public DbSet<EN_Province> EN_Province { get; set; }
        public DbSet<EN_District> EN_District { get; set; }
        public DbSet<EN_Ward> EN_Ward { get; set; }
        public DbSet<EN_AccountType> EN_AccountType { get; set; }
        public DbSet<EN_Store> EN_Store { get; set; }
        public DbSet<EN_CategoryList> EN_CategoryList { get; set; }
        public DbSet<EN_FoodList> EN_FoodList { get; set; }
        public DbSet<EN_Account> EN_Account { get; set; }
        public DbSet<EN_OrderHeader> EN_OrderHeader { get; set; }
        public DbSet<EN_PaymentStatus> EN_PaymentStatus { get; set; }
        public DbSet<EN_Promotion> EN_Promotion { get; set; }
        public DbSet<EN_Customer> EN_Customer { get; set; }
        public DbSet<EN_OrderLine> EN_OrderLine { get; set; }
        public DbSet<EN_DeliveryDiver> EN_DeliveryDriver { get; set; }
        public DbSet<EN_FavoriteFoods> EN_FavoriteFoods { get; set; }
        public DbSet<EN_CustomerAddress> EN_CustomerAddress { get; set; }
        public DbSet<EN_CustomerNotifications> EN_CustomerNotifications { get; set; }
        public DbSet<EN_CategoryPayment> EN_CategoryPayments { get; set; }
        public DbSet<EN_Paymentonline> EN_Paymentonlines { get; set; }


    }
}