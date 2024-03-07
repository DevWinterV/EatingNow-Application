using AutoMapper;
using DaiPhucVinh.Shared.Common.Image;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Shared.Common.Languages;
using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Shared.Customer;
using DaiPhucVinh.Shared.Product;
using DaiPhucVinh.Shared.Location;
using DaiPhucVinh.Shared.Quotation;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.TransactionInformation;
using DaiPhucVinh.Shared.LoaiDanhGia;
using DaiPhucVinh.Shared.inventoryitem;
using DaiPhucVinh.Shared.Chart;
using System.Collections.Generic;
using DaiPhucVinh.Services.Helper;
using System.Linq;
using System;
using DaiPhucVinh.Shared.User;
using DaiPhucVinh.Shared.Post;
using DaiPhucVinh.Shared.Dayoffs;
using DaiPhucVinh.Shared.Video;
using DaiPhucVinh.Shared.Task;
using DaiPhucVinh.Shared.Attendances;
using DaiPhucVinh.Shared.ProjectImages;
using DaiPhucVinh.Shared.Catalogue;
using DaiPhucVinh.Shared.ImageRecords;
using DaiPhucVinh.Shared.Feedback;
using DaiPhucVinh.Shared.UserGuides;
using System.Globalization;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Services.Framework;
using Falcon.Web.Core.Infrastructure;
using DaiPhucVinh.Shared.Notification;
using DaiPhucVinh.Shared.CategoryItem;
using DaiPhucVinh.Shared.Checkin;
using DaiPhucVinh.Shared.UnitOfMeasure;
using DaiPhucVinh.Shared.Auth;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Shared.ItemImages;
using DaiPhucVinh.Shared.ChatRoom;
using DaiPhucVinh.Shared.Quality;
using DaiPhucVinh.Shared.WorkLog;
using DaiPhucVinh.Shared.Cuisine;
using DaiPhucVinh.Shared.District;
using DaiPhucVinh.Shared.Province;
using DaiPhucVinh.Shared.Ward;
using DaiPhucVinh.Shared.AccountType;
using DaiPhucVinh.Shared.Store;
using DaiPhucVinh.Shared.CategoryList;
using DaiPhucVinh.Shared.FoodList;
using DaiPhucVinh.Shared.Account;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineResponse;
using DaiPhucVinh.Shared.DeliveryDriver;
using DaiPhucVinh.Shared.CustomerNotification;

namespace DaiPhucVinh.Services.Infrastructure
{
    public class AutoMapperApiProfile : Profile
    {
        public AutoMapperApiProfile()
        {
            CreateMap<ImageRecord, ImageDto>();
            CreateMap<WMS_Customers, CustomerResponse>()
                .ForMember(d => d.Tinh, o => o.MapFrom(s => s.WMS_Province.Name))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.CustomerTypeName, o => o.MapFrom(s => s.WMS_CustomersType.Name));
            CreateMap<WMS_Quotation, PriceQuoteResponse>();
            CreateMap<WMS_HopDong, RevenueResponse>()
                .ForMember(d => d.NgayKy, o => o.MapFrom(s => s.NgayKy.HasValue ? s.NgayKy.Value.ToString("dd/MM") : ""))
                .ForMember(d => d.Thang, o => o.MapFrom(s => s.NgayKy.HasValue ? s.NgayKy.Value.ToString("MM") : ""))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
                .ForMember(d => d.City_Id, o => o.MapFrom(s => s.WMS_Customer.TinhThanh_Id))
                .ForMember(d => d.City_Name, o => o.MapFrom(s => s.WMS_Customer.Tinh))
                .ForMember(d => d.Item, o => o.MapFrom(s => HopDong_CT(s)));
            CreateMap<WMS_HopDong_ChiTiet, ItemResponse>();
            CreateMap<WMS_Quotation, WordStreamResponse>()
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.WMS_Customer.Address))
                .ForMember(d => d.CustomerPhoneNo, o => o.MapFrom(s => s.WMS_Customer.PhoneNo))
                .ForMember(d => d.Position, o => o.MapFrom(s => s.WMS_Employee.Position))
                .ForMember(d => d.CustomerEmail, o => o.MapFrom(s => s.WMS_Customer.Email))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.EmployeePhoneNo, o => o.MapFrom(s => s.WMS_Employee.Tel))
                .ForMember(d => d.EmployeeEmail, o => o.MapFrom(s => s.WMS_Employee.Email))
                .ForMember(d => d.TongTien, o => o.MapFrom(s => s.Amt))
                .ForMember(d => d.PhanTramChiecKhau, o => o.MapFrom(s => s.CommissionRate))
                .ForMember(d => d.PhanTramVAT, o => o.MapFrom(s => s.VATRate))
                .ForMember(d => d.TienVAT, o => o.MapFrom(s => s.VAT))
                .ForMember(d => d.TienChietKhau, o => o.MapFrom(s => s.CommissionAmt))
                .ForMember(d => d.ConLai, o => o.MapFrom(s => s.AmtLCY))
                .ForMember(d => d.ThanhToan, o => o.MapFrom(s => s.PaymentType))
                .ForMember(d => d.BaoHanh, o => o.MapFrom(s => s.WarrantyType))
                .ForMember(d => d.GiaoNhan, o => o.MapFrom(s => s.WeliveryType))
                .ForMember(d => d.GhiChu, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.Quotation_Line, o => o.MapFrom(s => Quotation_Line(s)));

            CreateMap<WMS_CustomersType, CustomerTypeResponse>();
            CreateMap<WMS_Employee, EmployeeResponse>()
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
             .ForMember(d => d.ImageRecordId, o => o.MapFrom(s => s.ImageRecordId.ToString()));
            CreateMap<WMS_HopDong_Templete, HopDong_TemplateResponse>();
            CreateMap<WMS_Item, ProductResponse>()
                 .ForMember(d => d.CodeItemCategory, o => o.MapFrom(s => s.WMS_ItemCategory.Code))
                .ForMember(d => d.NameItemCategory, o => o.MapFrom(s => s.WMS_ItemCategory.Name))
                .ForMember(d => d.ItemGroup_Code, o => o.MapFrom(s => s.WMS_ItemGroup.Code))
                .ForMember(d => d.ItemGroup_Name, o => o.MapFrom(s => s.WMS_ItemGroup.Name));
            CreateMap<WMS_ItemGroup, ProductItemGroupResponse>();
            CreateMap<WMS_Item, CategoryItemResponse>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.WMS_ItemCategory.Code))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.WMS_ItemCategory.Name));
            CreateMap<WMS_UnitOfMeasure, UnitOfMeasureResponse>();
            CreateMap<WMS_Item, UnitOfMeasureResponse>()
            .ForMember(d => d.Code, o => o.MapFrom(s => s.WMS_UnitOfMeasure.Code))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.WMS_UnitOfMeasure.Name));
            CreateMap<WMS_Item_Image, ProductImageResponse>();
            CreateMap<WMS_Location, LocationResponse>();
            CreateMap<WMS_QuotationLine, HopDongResponse>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.WMS_Item.Name))
            .ForMember(d => d.price, o => o.MapFrom(s => s.UnitPrice))
            .ForMember(d => d.qty, o => o.MapFrom(s => s.Qty))
            .ForMember(d => d.amt, o => o.MapFrom(s => s.Amt));
            CreateMap<WMS_HopDong, HopDongResponse>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
            .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.WMS_Location.Code))
            .ForMember(d => d.quotationName, o => o.MapFrom(s => s.WMS_Quotation.DocumentNo))
            .ForMember(d => d.KhachHang_Name, o => o.MapFrom(s => s.WMS_Customer.Name))
            .ForMember(d => d.ListHD_CT, o => o.MapFrom(s => s.WMS_HopDong_ChiTiet))
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
            .ForMember(d => d.Code, o => o.MapFrom(s => s.LocationCode));
            CreateMap<WMS_HopDong, WordStreamHopDongResponse>()
            .ForMember(d => d.SoHopDong, o => o.MapFrom(s => s.SoHopDong))
            .ForMember(d => d.BenA_TenCongTy, o => o.MapFrom(s => s.BenA_TenCongTy))
            .ForMember(d => d.BenA_DiaChi, o => o.MapFrom(s => s.BenA_DiaChi))
            .ForMember(d => d.BenA_SoDienThoai, o => o.MapFrom(s => s.BenA_SoDienThoai))
            .ForMember(d => d.BenA_Fax, o => o.MapFrom(s => s.BenA_Fax))
            .ForMember(d => d.BenA_Email, o => o.MapFrom(s => s.BenA_Email))
            .ForMember(d => d.BenA_MST, o => o.MapFrom(s => s.BenA_MST))
            .ForMember(d => d.BenA_SoTaiKhoanNganHang, o => o.MapFrom(s => s.BenA_SoTaiKhoanNganHang))
            .ForMember(d => d.BenA_NguoiDaiDien, o => o.MapFrom(s => s.BenA_NguoiDaiDien))
            .ForMember(d => d.BenA_ChucVu, o => o.MapFrom(s => s.BenA_ChucVu))
            .ForMember(d => d.BenB_TenCongTy, o => o.MapFrom(s => s.BenB_TenCongTy))
            .ForMember(d => d.BenB_DiaChi, o => o.MapFrom(s => s.BenB_DiaChi))
            .ForMember(d => d.BenB_SoDienThoai, o => o.MapFrom(s => s.BenB_SoDienThoai))
            .ForMember(d => d.BenB_Fax, o => o.MapFrom(s => s.BenB_Fax))
            .ForMember(d => d.BenB_Email, o => o.MapFrom(s => s.BenB_Email))
            .ForMember(d => d.BenB_MST, o => o.MapFrom(s => s.BenB_MST))
            .ForMember(d => d.BenB_SoTaiKhoanNganHang, o => o.MapFrom(s => s.BenB_SoTaiKhoanNganHang))
            .ForMember(d => d.BenB_NguoiDaiDien, o => o.MapFrom(s => s.BenB_NguoiDaiDien))
            .ForMember(d => d.BenB_ChucVu, o => o.MapFrom(s => s.BenB_ChucVu))
            .ForMember(d => d.TongTien_TruocThue, o => o.MapFrom(s => s.TongTien_TruocThue))
            .ForMember(d => d.PhanTramThue, o => o.MapFrom(s => s.PhanTramThue))
            .ForMember(d => d.TienThue, o => o.MapFrom(s => s.TienThue))
            .ForMember(d => d.TongTien, o => o.MapFrom(s => s.TongTien))
            .ForMember(d => d.Dieu2, o => o.MapFrom(s => s.Dieu2))
            .ForMember(d => d.Dieu3, o => o.MapFrom(s => s.Dieu3))
            .ForMember(d => d.Dieu4, o => o.MapFrom(s => s.Dieu4))
            .ForMember(d => d.Dieu5, o => o.MapFrom(s => s.Dieu5))
            .ForMember(d => d.HopDong_ChiTiet, o => o.MapFrom(s => HopDong_CT1(s)));
            CreateMap<WMS_Quotation, HopDongResponse>()
                .ForMember(d => d.quotationName, o => o.MapFrom(s => s.DocumentNo))
                .ForMember(d => d.KhachHang_Name, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName));
            CreateMap<WMS_Quotation, QuotationResponse>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.CustomerPhoneNo, o => o.MapFrom(s => s.WMS_Customer.PhoneNo))
                .ForMember(d => d.CustomerEmail, o => o.MapFrom(s => s.WMS_Customer.Email))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.WMS_Customer.Address))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.EmployeePhoneNo, o => o.MapFrom(s => s.WMS_Employee.Tel))
                .ForMember(d => d.EmployeeEmail, o => o.MapFrom(s => s.WMS_Employee.Email))
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
                .ForMember(d => d.TongTien, o => o.MapFrom(s => s.Amt))
                .ForMember(d => d.PhanTramChiecKhau, o => o.MapFrom(s => s.CommissionRate))
                .ForMember(d => d.TienChietKhau, o => o.MapFrom(s => s.CommissionAmt))
                .ForMember(d => d.PhanTramVAT, o => o.MapFrom(s => s.VATRate))
                .ForMember(d => d.TienVAT, o => o.MapFrom(s => s.VAT))
                .ForMember(d => d.ConLai, o => o.MapFrom(s => s.AmtLCY))
                .ForMember(d => d.GhiChu, o => o.MapFrom(s => s.Note))
                .ForMember(d => d.ThanhToan, o => o.MapFrom(s => s.PaymentType))
                .ForMember(d => d.BaoHanh, o => o.MapFrom(s => s.WarrantyType))
                .ForMember(d => d.GiaoNhan, o => o.MapFrom(s => s.WeliveryType));
            CreateMap<WMS_Quotation_Templete, Quotation_TemplateResponse>();
            CreateMap<WMS_QuotationLine, Quotation_LineResponse>()
                .ForMember(d => d.ItemGroupCode, o => o.MapFrom(s => s.WMS_Item.WMS_ItemGroup.Code))
                .ForMember(d => d.ItemGroupName, o => o.MapFrom(s => s.WMS_Item.WMS_ItemGroup.Name))
                .ForMember(d => d.unitOfMeasureCode, o => o.MapFrom(s => s.WMS_Item.WMS_UnitOfMeasure.Code))
                .ForMember(d => d.unitOfMeasureName, o => o.MapFrom(s => s.WMS_Item.WMS_UnitOfMeasure.Name))
                .ForMember(d => d.IsDelete, o => o.MapFrom(s => s.WMS_Item.IsDelete))
                .ForMember(d => d.CountryOfOriginCode, o => o.MapFrom(s => s.WMS_Item.CountryOfOriginCode))
                .ForMember(d => d.Code2, o => o.MapFrom(s => s.WMS_Item.Code2))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.WMS_Item.Code))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.WMS_Item.Name))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.WMS_Item.Model))
                .ForMember(d => d.LinkVideo, o => o.MapFrom(s => s.WMS_Item.LinkVideo))
                .ForMember(d => d.Specifications, o => o.MapFrom(s => s.WMS_Item.Specifications))
                .ForMember(d => d.Quotation_id, o => o.MapFrom(s => s.WMS_Quotation.Id))
                .ForMember(d => d.qty, o => o.MapFrom(s => s.Qty))
                .ForMember(d => d.price, o => o.MapFrom(s => s.UnitPrice))
                .ForMember(d => d.ChatLuongId, o => o.MapFrom(s => s.FSW_ChatLuong.Id))
                .ForMember(d => d.ChatLuongName, o => o.MapFrom(s => s.FSW_ChatLuong.Name))
                .ForMember(d => d.amt, o => o.MapFrom(s => s.Amt));
            CreateMap<WMS_TransactionType, TransactionHistoryTypeResponse>();
            CreateMap<WMS_Province, DistrictResponse>();
            CreateMap<WMS_TransactionInformation, TransactionInformationResponse>()
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.CustomerPhone, o => o.MapFrom(s => s.WMS_Customer.PhoneNo))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.WMS_Customer.Address))
                .ForMember(d => d.TransactionTypeName, o => o.MapFrom(s => s.WMS_TransactionType.Name))
                .ForMember(d => d.ProvinceName, o => o.MapFrom(s => s.WMS_Customer.WMS_Province.Name))
                .ForMember(d => d.DanhGia_Id, o => o.MapFrom(s => s.WMS_LoaiDanhGia.Id))
                .ForMember(d => d.SoBaoGia, o => o.MapFrom(s => s.WMS_Quotation.DocumentNo))
                .ForMember(d => d.SoHopDong, o => o.MapFrom(s => s.WMS_HopDong.SoHopDong))
                .ForMember(d => d.TenLoaiDanhGia, o => o.MapFrom(s => s.WMS_LoaiDanhGia.Name));
            CreateMap<WMS_TransactionInformation, TransactionModalResponse>()
                .ForMember(d => d.PersonContact, o => o.MapFrom(s => s.WMS_Customer.PersonContact))
                .ForMember(d => d.TransactionTypeName, o => o.MapFrom(s => s.WMS_TransactionType.Name))
                .ForMember(d => d.TransactionTypeId, o => o.MapFrom(s => s.TransactionType_Id))
                .ForMember(d => d.CustomerPhone, o => o.MapFrom(s => s.WMS_Customer.PhoneNo))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.WMS_Customer.Address))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.TenLoaiDanhGia, o => o.MapFrom(s => s.WMS_LoaiDanhGia.Name))
                .ForMember(d => d.FileAttach, o => o.MapFrom(s => s.FileAttach))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.SoHopDong, o => o.MapFrom(s => s.WMS_HopDong.SoHopDong))
                .ForMember(d => d.HopDong_Id, o => o.MapFrom(s => s.WMS_HopDong.Id))
                .ForMember(d => d.SoBaoGia, o => o.MapFrom(s => s.WMS_Quotation.DocumentNo))
                .ForMember(d => d.BaoGia_Id, o => o.MapFrom(s => s.WMS_Quotation.Id))
                .ForMember(d => d.FileAttach, o => o.MapFrom(s => s.FileAttach));
            CreateMap<WMS_HopDong_ChiTiet, HopDong_ChiTietResponse>()
                .ForMember(d => d.NgayTaoHopDong, o => o.MapFrom(s => s.WMS_HopDong.NgayTaoHopDong))
                .ForMember(d => d.SoHopDong, o => o.MapFrom(s => s.WMS_HopDong.SoHopDong))
                .ForMember(d => d.Code, o => o.MapFrom(s => s.ItemCode))
                .ForMember(d => d.ChatLuong, o => o.MapFrom(s => s.ChatLuong))
                .ForMember(d => d.qty, o => o.MapFrom(s => s.SoLuong))
                .ForMember(d => d.price, o => o.MapFrom(s => s.DonGia))
                .ForMember(d => d.amt, o => o.MapFrom(s => s.ThanhTien))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.WMS_Item.Model))
                .ForMember(d => d.ChatLuongName, o => o.MapFrom(s => s.FSW_ChatLuong.Name))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.WMS_Item.Name));
            CreateMap<WMS_LoaiDanhGia, LoaiDanhGiaResponse>();
            CreateMap<WMS_InventoryItem, InventoryitemResponse>()
                .ForMember(d => d.ItemGroup, o => o.MapFrom(s => s.WMS_Item.WMS_ItemGroup.Code))
                .ForMember(d => d.LocationCode, o => o.MapFrom(s => s.WMS_Location.Code))
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.WMS_Item.UnitPrice))
               .ForMember(d => d.Code, o => o.MapFrom(s => s.WMS_Item.Code))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.WMS_Item.Name))
                .ForMember(d => d.Model, o => o.MapFrom(s => s.WMS_Item.Model))
                .ForMember(d => d.AbsolutePath, o => o.MapFrom(s => FindImgFromId(s)));
            CreateMap<WMS_Item, InventoryitemResponse>()
                .ForMember(d => d.ItemGroup, o => o.MapFrom(s => s.WMS_ItemGroup.Code))
                .ForMember(d => d.UnitPrice, o => o.MapFrom(s => s.UnitPrice ?? 0))
                .ForMember(d => d.AbsolutePath, o => o.MapFrom(s => FindImgFromIds(s)));
            CreateMap<WMS_TransactionInformation, AppointmentTrackingResponse>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.TenLoaiDanhGia, o => o.MapFrom(s => s.WMS_LoaiDanhGia.Name))
                .ForMember(d => d.TransactionTypeName, o => o.MapFrom(s => s.WMS_TransactionType.Name))
                .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.WMS_Employee.FullName))
                .ForMember(d => d.SoNgayDenHen, o => o.MapFrom(s => TinhSoNgayDenHen(s).Item1))
                .ForMember(d => d.DayRemain, o => o.MapFrom(s => TinhSoNgayDenHen(s).Item2));
            CreateMap<User, UserResponse>()
                .ForMember(d => d.RoleSystemName, o => o.MapFrom(s => s.Role.Display));
            CreateMap<Role, RolesResponse>();
            CreateMap<FUV_Posts, PostResponse>()
                .ForMember(d => d.ImagePath, o => o.MapFrom(s => s.ImageRecord != null ? s.ImageRecord.AbsolutePath : ""));
            CreateMap<FUV_Posts, PostMobileResponse>()
                .ForMember(d => d.ImagePath, o => o.MapFrom(s => s.ImageRecord.AbsolutePath))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.UpdatedAt.HasValue ? s.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm") : ""));
            CreateMap<FUV_Videos, VideoResponse>()
                .ForMember(d => d.AbsolutePath, o => o.MapFrom(s => s.ImageRecord.AbsolutePath))
                .ForMember(d => d.UpdatedAt, o => o.MapFrom(s => s.UpdatedAt.HasValue ? s.UpdatedAt.Value.ToString("dd/MM/yyyy HH:mm") : ""));
            CreateMap<FUV_Dayoffs, DayoffsResponse>()
            .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
            .ForMember(d => d.EmployeeCode, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.User.DisplayName));
            CreateMap<FUV_TaskStatus, TaskStatusResponse>();
            CreateMap<FUV_TaskTypes, TaskTypeResponse>();
            CreateMap<FUV_TaskResult, TaskResultResponse>();
            CreateMap<FUV_Tasks, TaskResponse>()
                .ForMember(d => d.ItemName, o => o.MapFrom(s => s.WMS_Item.Name))
                .ForMember(d => d.TaskTypeName, o => o.MapFrom(s => s.FUV_TaskTypes.Name))
                .ForMember(d => d.AssignUserName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.WMS_Customer.Address))
                .ForMember(d => d.CustomerPhoneDefault, o => o.MapFrom(s => s.WMS_Customer.PhoneNo))
                .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
                .ForMember(d => d.LocationAddress, o => o.MapFrom(s => s.WMS_Location.Address))
                .ForMember(d => d.TaskStatusName, o => o.MapFrom(s => s.FUV_TaskStatus.Name))
                .ForMember(d => d.TaskStatusClassName, o => o.MapFrom(s => s.FUV_TaskStatus.ClassName))
                .ForMember(d => d.TaskResultName, o => o.MapFrom(s => s.FUV_TaskResult.Name));
            CreateMap<FUV_Tasks, TaskMobileResponse>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.WMS_Customer.Name))
                .ForMember(d => d.AssignUserName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.CustomerPhone, o => o.MapFrom(s => s.WMS_Customer.PhoneNo))
                .ForMember(d => d.CustomerAddress, o => o.MapFrom(s => s.WMS_Customer.Address))
                .ForMember(d => d.TaskTypeName, o => o.MapFrom(s => s.FUV_TaskTypes.Name))
                .ForMember(d => d.TaskStatusName, o => o.MapFrom(s => s.FUV_TaskStatus.Name))
                .ForMember(d => d.TaskResultName, o => o.MapFrom(s => s.FUV_TaskResult.Name));

            CreateMap<FUV_Attendances, AttendancesResponse>()
             .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.DisplayName))
             .ForMember(d => d.StartTime, o => o.MapFrom(s => StripMilliseconds(TimeSpanSubtract(s.CheckInTime.HasValue ? s.CheckInTime.Value.TimeOfDay : TimeSpan.Zero))))
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name))
             .ForMember(d => d.LocationAddress, o => o.MapFrom(s => s.WMS_Location.Address))
             .ForMember(d => d.Long, o => o.MapFrom(s => s.WMS_Location.Long))
             .ForMember(d => d.Lat, o => o.MapFrom(s => s.WMS_Location.Lat));
            CreateMap<FUV_Attendances, ExportAttendancesResponse>()
             .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.DisplayName))
             .ForMember(d => d.LocationName, o => o.MapFrom(s => s.WMS_Location.Name));
            CreateMap<FUV_ProjectImages, ProjectImagesResponse>();

            CreateMap<FUV_Catalogs, CatalogueResponse>();
            CreateMap<FUV_Catalogs, CatalogueDto>()
                .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt.ToString("dd/MM/yyyy HH:mm")));

            CreateMap<ImageRecord, ImageRecordsResponse>();
            CreateMap<FUV_Feedbacks, FeedbackResponse>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.DisplayName));
            CreateMap<FUV_UserGuides, UserGuidesResponse>()
                .ForMember(d => d.ItemName, o => o.MapFrom(s => s.WMS_Item.Name));
            CreateMap<FUV_Notifications, NotificationResponse>()
                .ForMember(d => d.NotificationTypeName, o => o.MapFrom(s => s.FUV_NotificationTypes.Name))
                .ForMember(d => d.NotificationGroupName, o => o.MapFrom(s => s.FUV_NotificationGroups.Name));
            CreateMap<FUV_NotificationTypes, NotificationTypeResponse>();
            CreateMap<FUV_NotificationGroups, NotificationGroupResponse>();
            CreateMap<WMS_ItemCategory, CategoryItemResponse>();
            CreateMap<WMS_ItemCategory, CategoryDto>();
            CreateMap<WMS_Item, ProductGroupMobileResponse>();
            CreateMap<WMS_Item, ProductDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.WMS_ItemGroup.Name));
            CreateMap<FUV_Checkin, CheckinResponse>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.DisplayName));

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            CreateMap<Role, RoleDto>()
                .ForMember(d => d.lstPermissons, o => o.MapFrom(s => s.Permissons.Split(';').ToList()));

            CreateMap<WMS_HopDong_ChiTiet, ProductByCustomerResponse>()
           .ForMember(d => d.ItemCode, o => o.MapFrom(s => s.WMS_Item.Code))
           .ForMember(d => d.ItemName, o => o.MapFrom(s => s.WMS_Item.Name));
            CreateMap<WMS_Item_Image, ItemImageResponse>();

            CreateMap<FUV_ChatRoom, ChatRoomResponse>();
            CreateMap<FUV_ChatRoomMember, ChatRoomResponse>()
                 .ForMember(d => d.Id, o => o.MapFrom(s => s.FUV_ChatRoom.Id))
                 .ForMember(d => d.Uuid, o => o.MapFrom(s => s.FUV_ChatRoom.Uuid))
                 .ForMember(d => d.Name, o => o.MapFrom(s => s.FUV_ChatRoom.Name))
                 .ForMember(d => d.UserOwnerId, o => o.MapFrom(s => s.FUV_ChatRoom.UserOwnerId));
            CreateMap<FUV_ChatRoomMember, EmployeeResponse>();
            CreateMap<FUV_ChatDescriptions, ChatDescriptionResponse>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName));
            CreateMap<FSW_ChatLuong, QualityResponse>();
            CreateMap<FUV_WorkLog, WorkLogResponse>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName));

            CreateMap<EN_Cuisine, CuisineResponse>();
            CreateMap<EN_Province, ProvinceResponse>();
            CreateMap<EN_District, DistrictResponse>()
            .ForMember(d => d.ItemProvinceCode, o => o.MapFrom(s => s.Province.ProvinceId));
            CreateMap<EN_Ward, WardResponse>()
            .ForMember(d => d.ItemDistrictCode, o => o.MapFrom(s => s.District.DistrictId));
            CreateMap<EN_AccountType, AccountTypeResponse>();
            CreateMap<EN_Store, StoreResponse>()
                .ForMember(d => d.Province, o => o.MapFrom(s => s.Province.Name))
                .ForMember(d => d.Cuisine, o => o.MapFrom(s => s.Cuisine.Name));

            CreateMap<EN_CategoryList, CategoryListResponse>()
                .ForMember(d => d.NameStore, o => o.MapFrom(s => s.Store.FullName))
                .ForMember(d => d.DescriptionStore, o => o.MapFrom(s => s.Store.Description))
                .ForMember(d => d.OpenTime, o => o.MapFrom(s => s.Store.OpenTime))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Store.Address))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.Store.Phone))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Store.Latitude))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Store.Longitude))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Store.Email));

            CreateMap<EN_Store, CategoryListResponse>()
              .ForMember(d => d.NameStore, o => o.MapFrom(s => s.FullName))
              .ForMember(d => d.DescriptionStore, o => o.MapFrom(s => s.Description))
              .ForMember(d => d.OpenTime, o => o.MapFrom(s => s.OpenTime))
              .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
              .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone))
              .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Latitude))
              .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Longitude))
              .ForMember(d => d.Email, o => o.MapFrom(s => s.Email));

            CreateMap<EN_FoodList, FoodListResponse>()
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.CategoryName));
            CreateMap<FoodList_Store, FoodListResponse>()
            .ForMember(d => d.FoodListId, o => o.MapFrom(s => s.foodItem.FoodListId))
            .ForMember(d => d.Category, o => o.MapFrom(s => s.foodItem.Category))
            .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.foodItem.CategoryId))
            .ForMember(d => d.FoodName, o => o.MapFrom(s => s.foodItem.FoodName))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.foodItem.Price))
            .ForMember(d => d.qty, o => o.MapFrom(s => s.foodItem.qty))
            .ForMember(d => d.UploadImage, o => o.MapFrom(s => s.foodItem.UploadImage))
            .ForMember(d => d.Description, o => o.MapFrom(s => s.foodItem.Description))
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.foodItem.UserId))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.foodItem.Status))
            .ForMember(d => d.Hint, o => o.MapFrom(s => s.foodItem.Hint))
            .ForMember(d => d.IsNew, o => o.MapFrom(s => s.foodItem.IsNew))
            .ForMember(d => d.IsNoiBat, o => o.MapFrom(s => s.foodItem.IsNoiBat))
            .ForMember(d => d.QuantitySupplied, o => o.MapFrom(s => s.foodItem.QuantitySupplied))
            .ForMember(d => d.ExpiryDate, o => o.MapFrom(s => s.foodItem.ExpiryDate))
            .ForMember(d => d.Qtycontrolled, o => o.MapFrom(s => s.foodItem.Qtycontrolled))
            .ForMember(d => d.QtySuppliedcontrolled, o => o.MapFrom(s => s.foodItem.QtySuppliedcontrolled))
            .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Storeitem.Latitude))
            .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Storeitem.Longitude))
            .ForMember(d => d.storeName, o => o.MapFrom(s => s.Storeitem.FullName));



            CreateMap<EN_Account, AccountResponse>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Store.AbsoluteImage))
                .ForMember(d => d.StoreName, o => o.MapFrom(s => s.Store.FullName));
            CreateMap<EN_OrderHeader, OrderHeaderResponse>()
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.EN_Customer.CompleteName))
                .ForMember(d => d.StoreName, o => o.MapFrom(s => s.EN_Store.FullName))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.EN_Customer.Phone))
                .ForMember(d => d.TokenWeb, o => o.MapFrom(s => s.EN_Customer.TokenWeb))
                .ForMember(d => d.TokenApp, o => o.MapFrom(s => s.EN_Customer.TokenApp));
    
            CreateMap<EN_Customer, EN_CustomerResponse>();

            CreateMap<EN_OrderLine, OrderLineReponse>()
                .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.qty * s.Price));
            CreateMap<EN_DeliveryDiver, DeliveryDriverResponse>()
                .ForMember(d => d.ProvinceName, o => o.MapFrom(s => s.Province.Name))
                .ForMember(d => d.DistrictName, o => o.MapFrom(s => s.District.Name))
                .ForMember(d => d.WardName, o => o.MapFrom(s => s.Ward.Name));
            CreateMap<EN_CustomerAddress, EN_CustomerAddressResponse>()
                .ForMember(d => d.ProvinceName, o => o.MapFrom(s => s.Province.Name))
                .ForMember(d => d.DistrictName, o => o.MapFrom(s => s.District.Name))
                .ForMember(d => d.WardName, o => o.MapFrom(s => s.Ward.Name));
            CreateMap<EN_CustomerNotifications, EN_CustomerNotificationResponse>();
        }
        #region private methods
        private byte[] FindImgFromId(WMS_InventoryItem item)
        {
            List<int> imagesListId = new List<int>();
            string img = item.ItemCode;
            var _datacontext = EngineContext.Current.Resolve<DataContext>();
            //imagesListId = img.Split(',')?.Select(Int32.Parse)?.ToList();
            //var test = imagesListId[0];
            var link = _datacontext.WMS_Item_Images.FirstOrDefault(s=>s.ItemCode == img);
            return link != null ? link.Image : null;
        }
        private byte[] FindImgFromIds(WMS_Item item)
        {
            List<int> imagesListId = new List<int>();
            string img = item.Code;
            var _datacontext = EngineContext.Current.Resolve<DataContext>();
            //imagesListId = img.Split(',')?.Select(Int32.Parse)?.ToList();
            //var test = imagesListId[0];
            var link = _datacontext.WMS_Item_Images.FirstOrDefault(s => s.ItemCode == img);
            return link != null ? link.Image : null;
        }
        private List<HopDong_ChiTietResponse> HopDong_CT1(WMS_HopDong hopdong)
        {
            var hd_ct = hopdong.WMS_HopDong_ChiTiet.ToList().MapTo<HopDong_ChiTietResponse>();
            hd_ct = hd_ct.ToList();
            return hd_ct;
        }
        private List<ItemResponse> HopDong_CT(WMS_HopDong hopdong)
        {
            var hd_ct = hopdong.WMS_HopDong_ChiTiet.ToList().MapTo<ItemResponse>();
            hd_ct = hd_ct.ToList();
            return hd_ct;
        }
        private bool CheckLate(DateTime? date)
        {
            return true;
        }
        private TimeSpan TimeSpanSubtract(TimeSpan checkIn)
        {
            var _settingService = EngineContext.Current.Resolve<ISetting>();
            MetadataSettings settings = _settingService.LoadSetting<MetadataSettings>();
            return checkIn != TimeSpan.Zero ? TimeSpan.ParseExact(settings.StartTime, "hh\\:mm", CultureInfo.InvariantCulture) - checkIn : TimeSpan.Zero;
        }
        private TimeSpan StripMilliseconds(TimeSpan time)
        {
            return time != TimeSpan.Zero ? new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds) : new TimeSpan(time.Days);
        }
        private List<Quotation_LineResponse> Quotation_Line(WMS_Quotation baogia)
        {
            var bg_ct = baogia.WMS_QuotationLine.ToList().MapTo<Quotation_LineResponse>();
            bg_ct = bg_ct.ToList();
            return bg_ct;
        }

        private double GetMinDay(double songayhen, DateTime? date)
        {
            if (date.HasValue)
            {
                DateTime currentdate = DateTime.Now;
                DateTime datetime = date.Value;
                TimeSpan datevalue = datetime.Subtract(currentdate);
                var songay = Math.Ceiling(datevalue.TotalDays);
                return Math.Min(songayhen, songay);
            }
            return songayhen;
        }
        private Tuple<string, double> TinhSoNgayDenHen(WMS_TransactionInformation trans)
        {
            var songaydenhen = "";
            double dayRemain = 0;
            string todayString = "Hôm nay";
            string earlyString = "{0} ngày";
            string lateString = "Trễ {0} ngày";

            DateTime currentdate = DateTime.Now;
            DateTime appointmentdate = trans.AppointmentDate.Value;
            TimeSpan songayhenvalue = appointmentdate.Subtract(currentdate);
            var songayhen = Math.Ceiling(songayhenvalue.TotalDays);

            double firstMin = Math.Ceiling(GetMinDay(songayhen, trans.NgayChuyenTien));
            double lastMin = Math.Ceiling(GetMinDay(firstMin, trans.NgayGiaoHang));

            if (lastMin == 0)
            {
                dayRemain = 0;
                songaydenhen = todayString;
            }
            else if (lastMin > 0)
            {
                dayRemain = lastMin;
                songaydenhen = string.Format(earlyString, lastMin);
            }
            else
            {
                dayRemain = lastMin;
                songaydenhen = string.Format(lateString, Math.Abs(lastMin));
            }

            return new Tuple<string, double>(songaydenhen, dayRemain);
        }
       
        #endregion
    }
}

