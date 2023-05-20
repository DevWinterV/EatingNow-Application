using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Database;
using System.Collections.Generic;
using DaiPhucVinh.Shared.Quotation;
using DaiPhucVinh.Shared.inventoryitem;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Dayoffs;
using DaiPhucVinh.Shared.Attendances;
using OfficeOpenXml.Drawing;
using System.Drawing;
using System.Text;
using DaiPhucVinh.Shared.Inventoryitem;

namespace DaiPhucVinh.Services.Excel
{
    public interface IExportService
    {
        void ExportToXlsxInventory(Stream stream, string templateName, BaseResponse<InventoryItemExportResponse> datas, InventoryitemRequest request);
        void ExportToXlsxRevenueStatistic(Stream stream, string templateName, BaseResponse<HopDongResponse> datas,HopDongRequest request);
        void ExportToXlsxDayoffs(Stream stream, string templateName, BaseResponse<DayoffsResponse> datas, DayoffsRequest request);
        void ExportToXlsxAttendances(Stream stream, string templateName, BaseResponse<AttendancesResponse> datas, AttendancesRequest request);
        void ExportToXlsxQuotationImage(Stream stream, string templateName, BaseResponse<Quotation_LineResponse> datas, QuotationRequest request);
    }
    public class ExportService : IExportService
    {
        private readonly ILogService _logService;
        public ExportService(ILogService logService)
        {
            _logService = logService;
        }
        #region danh sách khách hàng
        //public void ExportToXlsxCustomers(Stream stream, string templateName, BaseResponse<CustomersDto> datas)
        //{
        //    if (string.IsNullOrEmpty(templateName))
        //        throw new ArgumentNullException(nameof(templateName));
        //    using (var templateDocumentStream = File.OpenRead(templateName))
        //    {
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //        using (var xlPackage = new ExcelPackage(templateDocumentStream))
        //        {
        //            var sheet = xlPackage.Workbook.Worksheets["Sheet1"];
        //            //Tieu de
        //            var rowIndex = 2;
        //            var column = 1;
        //            const int index = 1;
        //            foreach (var d in datas.Datas)
        //            {
        //                try
        //                {
        //                    //[1] STT
        //                    sheet.Cells[rowIndex, column].Value = rowIndex - index;
        //                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                    column++;
        //                    sheet.Cells[rowIndex, column].Value = d.Code;
        //                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                    column++;
        //                    sheet.Cells[rowIndex, column].Value = d.Name;
        //                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                    column++;
        //                    sheet.Cells[rowIndex, column].Value = d.PhoneNo;
        //                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                    column++;
        //                    sheet.Cells[rowIndex, column].Value = d.CardNumber;
        //                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                    column++;
        //                    sheet.Cells[rowIndex, column].Value = d.TaxCode;
        //                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                    rowIndex++;
        //                    column = 1;
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw ex;
        //                }
        //            }
        //            xlPackage.SaveAs(stream);
        //        }
        //    }
        //}
        #endregion

        #region danh sách sản phẩm tồn kho hiện hành
        public void ExportToXlsxInventory(Stream stream, string templateName, BaseResponse<InventoryItemExportResponse> datas,InventoryitemRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            using (var templateDocumentStream = File.OpenRead(templateName))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var xlPackage = new ExcelPackage(templateDocumentStream))
                {
                    var sheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    //Tieu de
                    var rowIndex = 3;
                    var column = 1;
                    const int index = 2;
                    foreach (var d in datas.Data)
                    {
                        try
                        {
                            //[1] STT
                            sheet.Cells[rowIndex, column].Value = rowIndex - index;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(249, 231, 118));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.Name; //Tên máy
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(249, 231, 118));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.Model; //Model
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(249, 231, 118));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.TotalInventory; //Tổng tồn
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(249, 231, 118));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.HaNoi; //Hà Nội
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(184, 199, 201));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.HCM; //HCM
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(97, 229, 247));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.BinhDuong; //Bình Dương
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(130, 108, 148));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.DongNai; //Đồng Nai
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(250, 211, 133));
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.QuyNhon; //Quy Nhơn
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Fill.SetBackground(Color.FromArgb(12, 153, 212));
                            column++;

                            rowIndex++;
                            column = 1;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    xlPackage.SaveAs(stream);
                }
            }
        }
        #endregion
        #region Danh sách thống kê doanh thu
        public void ExportToXlsxRevenueStatistic(Stream stream, string templateName, BaseResponse<HopDongResponse> datas, HopDongRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            using (var templateDocumentStream = File.OpenRead(templateName))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var xlPackage = new ExcelPackage(templateDocumentStream))
                {
                    var sheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    //Tieu de
                    var rowIndex = 4;
                    var column = 1;
                    const int index = 3;
                    foreach (var d in datas.Data)
                    {
                        try
                        {
                            //[1] STT
                            sheet.Cells[rowIndex, column].Value = rowIndex - index;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.BenA_TenCongTy;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.SoHopDong;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.NgayKy;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.TongTien_TruocThue;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.PhanTramThue;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.TienThue;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.TongTien;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.EmployeeName;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.LocationName;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            rowIndex++;
                            column = 1;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    xlPackage.SaveAs(stream);
                }
            }
        }
        #endregion
        #region Danh sách quản lý tồn kho
        public void ExportToXlsxDayoffs(Stream stream, string templateName, BaseResponse<DayoffsResponse> datas, DayoffsRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            using (var templateDocumentStream = File.OpenRead(templateName))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var xlPackage = new ExcelPackage(templateDocumentStream))
                {
                    var sheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    //Tieu de
                    var rowIndex = 3;
                    var column = 1;
                    const int index = 2;
                    foreach (var d in datas.Data)
                    {
                        try
                        {
                            //[1] STT
                            sheet.Cells[rowIndex, column].Value = rowIndex - index;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.EmployeeName;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.StartDate;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.DayRequests;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.DayApproved;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.Approved == true ? "Đồng ý" : "Không đồng ý";
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.ApprovedDescription.HasValue() ? "Đã phản hồi" : "";
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            sheet.Cells[rowIndex, column].Value = d.LocationName;
                            sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            column++;
                            rowIndex++;
                            column = 1;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    xlPackage.SaveAs(stream);
                }
            }
        }
        #endregion
        #region Danh sách quản lý chấm công
        public void ExportToXlsxAttendances(Stream stream, string templateName, BaseResponse<AttendancesResponse> datas, AttendancesRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            using (var templateDocumentStream = File.OpenRead(templateName))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var xlPackage = new ExcelPackage(templateDocumentStream))
                {
                    var sheet = xlPackage.Workbook.Worksheets["Sheet1"];
                    //Tieu de
                    var rowIndex = 3;
                    var column = 1;
                    const int index = 2;
                    foreach (var d in datas.Data)
                    {
                        try
                        {
                            string emptyString = "";
                            //TimeSpan  TimConvert = d.StartTime - d.StartTime - d.StartTime;
                    
                            //if (d.StartTime.Ticks < 0)
                            //{
                                //[1] STT
                                sheet.Cells[rowIndex, column].Value = rowIndex - index;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.UserName;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.WorkDate;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = !d.CheckInTime.HasValue ? emptyString : d.CheckInTime.Value.ToString("HH:mm:ss");
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.StartTime.Value.Ticks >= 0 ? emptyString : d.StartTime.Value.ToString(@"hh\:mm\:ss"); 
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = !d.CheckOutTime.HasValue ? emptyString : d.CheckOutTime.Value.ToString("HH:mm:ss");
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.Accept == true ? "Đồng ý" : "";
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.Note.HasValue() ? "có giải trình" : "";
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.NoteReply.HasValue() ? "có phản hồi" : "";
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.LocationName;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                rowIndex++;
                                column = 1;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    xlPackage.SaveAs(stream);
                }
            }
        }
        #endregion
        #region Danh sách hình ảnh sản phẩm của báo giá
        public void ExportToXlsxQuotationImage(Stream stream, string templateName, BaseResponse<Quotation_LineResponse> datas, QuotationRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            using (var templateDocumentStream = File.OpenRead(templateName))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var xlPackage = new ExcelPackage(templateDocumentStream))
                {
                    var sheet = xlPackage.Workbook.Worksheets["Sheet1"];


                    //header khách hàng.
                    var RowIndexHeader = 2;

                    sheet.Cells[RowIndexHeader, 1, RowIndexHeader, 8].Merge = true;
                    sheet.Cells[RowIndexHeader, 1, RowIndexHeader, 8].Value = "BẢNG DỰ TOÁN DỰ ÁN: MÁY MÓC SẢN XUẤT GỖ" + "\n" + "CHỦ ĐẦU TƯ: " + request.CustomerName.ToUpper() + "\n" + "ĐỊA CHỈ: " + request.CustomerAddress.ToUpper();
                    sheet.Cells[RowIndexHeader, 1].Style.Font.SetFromFont(new Font("Times New Roman", 12));
                    sheet.Cells[RowIndexHeader, 1].Style.Font.Bold = true;

                    //Tieu de
                    double? SumAmt = 0;
                    var rowIndex = 4;
                    var column = 1;
                    const int index = 3;
                    foreach (var d in datas.Data)
                    {

                        try
                        {
                            SumAmt += d.amt;
                            if (d.LinkVideo != null)
                            {
                                
                                //[1] STT
                                sheet.Cells[rowIndex, column].Value = rowIndex - index;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = d.Name;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                if (d.ImageByte != null)
                                {
                                    MemoryStream ms = new MemoryStream(d.ImageByte);
                                    Image img = Image.FromStream(ms);
                                    var pics = sheet.Drawings.AddPicture(d.Name, img);
                                    pics.SetPosition(rowIndex - 1, 8, 2, 50);
                                    pics.SetSize(100, 80);
                                }

                                sheet.Cells[rowIndex, column].Value = "";
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = d.Model;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = d.qty;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = d.price;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = d.amt;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                var checkHttps = d.LinkVideo.Contains("https");
                                var checkHttp = d.LinkVideo.Contains("http");
                                if (checkHttps || checkHttp)
                                {
                                    sheet.Cells[rowIndex, column].Hyperlink = new Uri(d.LinkVideo);
                                    sheet.Cells[rowIndex, column].Value = d.LinkVideo;
                                    sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                    sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                    sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                    sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    sheet.Cells[rowIndex, column].Style.WrapText = true;
                                    sheet.Cells[rowIndex, column].Style.Font.UnderLine = true;
                                    sheet.Cells[rowIndex, column].Style.Font.Color.SetColor(Color.Blue);
                                        
                                }
                                column++;
                                rowIndex++;
                                column = 1;
                            }
                            else
                            { //[1] STT
                                sheet.Cells[rowIndex, column].Value = rowIndex - index;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                //sheet.SelectedRange.IsFullRow = true;
                                sheet.Cells[rowIndex, column].Value = d.Name;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                if (d.ImageByte != null)
                                {
                                    MemoryStream ms = new MemoryStream(d.ImageByte);
                                    Image img = Image.FromStream(ms);
                                    var pics = sheet.Drawings.AddPicture(d.Name + rowIndex, img);
                                    pics.SetPosition(rowIndex - 1, 8, 2, 50);
                                    pics.SetSize(100, 80);
                                }

                                sheet.Cells[rowIndex, column].Value = "";
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = d.Model;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.qty;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.price;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;
                                sheet.Cells[rowIndex, column].Value = d.amt;
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                column++;

                                sheet.Cells[rowIndex, column].Value = "";
                                sheet.Cells[rowIndex, column].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                sheet.Cells[rowIndex, column].Style.WrapText = true;
                                sheet.Cells[rowIndex, column].Style.Font.UnderLine = true;
                                sheet.Cells[rowIndex, column].Style.Font.Color.SetColor(Color.Blue);

                                column++;
                                rowIndex++;
                                column = 1;

                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }

                    sheet.Cells[rowIndex, 1, rowIndex, 2].Merge = true;

                    sheet.Cells[rowIndex, 1, rowIndex, 2].Value = "TỔNG CỘNG TRƯỚC THUẾ";
                    sheet.Cells[rowIndex, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 1].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));
                    sheet.Cells[rowIndex, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14));
                    sheet.Cells[rowIndex, 1].Style.Font.Bold = true;
                    sheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                    sheet.Cells[rowIndex, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 2].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));




                    sheet.Cells[rowIndex, 3].Value = "";
                    sheet.Cells[rowIndex, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 3].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));

                    sheet.Cells[rowIndex, 4].Value = "";
                    sheet.Cells[rowIndex, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 4].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));

                    sheet.Cells[rowIndex, 5].Value = "";
                    sheet.Cells[rowIndex, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 5].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));

                    sheet.Cells[rowIndex, 6].Value = "";
                    sheet.Cells[rowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 6].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));

                    sheet.Cells[rowIndex, 7].Value = SumAmt;
                    sheet.Cells[rowIndex, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 7].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));
                    sheet.Cells[rowIndex, 7].Style.Font.SetFromFont(new Font("Times New Roman", 14));
                    sheet.Cells[rowIndex, 7].Style.Font.Bold = true;


                    sheet.Cells[rowIndex, 8].Value = "";
                    sheet.Cells[rowIndex, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[rowIndex, 8].Style.Fill.SetBackground(Color.FromArgb(255, 255, 0));

                    xlPackage.SaveAs(stream);
                }
            }
        }
        #endregion

    }
}
