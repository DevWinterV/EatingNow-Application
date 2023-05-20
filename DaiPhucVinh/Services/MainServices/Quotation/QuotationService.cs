using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Excel;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Quotation;
using Falcon.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Quotation
{
    public interface IQuotationService
    {
        Task<BaseResponse<QuotationResponse>> TakeAlls(QuotationRequest request);
        Task<BaseResponse<Quotation_LineResponse>> TakeAllsQuotation_Line(QuotationRequest request, int Id);
        Task<BaseResponse<Quotation_TemplateResponse>> TakeAllsQuotation_Template(Quotation_TemplateRequest request);
        Task<BaseResponse<QuotationResponse>> CreateOrUpdate_Quotation(QuotationRequest request);
        Task<BaseResponse<bool>> Remove_Quotation(QuotationRequest request);
        Task<BaseResponse<WordStreamResponse>> SearchData_WordStream(int quotationId);
        Task<BaseResponse<HopDongResponse>> TakeQuotation(string KhachHang_Code);
        Task<BaseResponse<QuotationResponse>> SearchQuotation(int QuotationCode);
        Task<BaseResponse<Quotation_LineResponse>> TakeQuotationLine(int Quotation_Code);
        Task<string> ExcelExportQuotation(string templateName, QuotationRequest request);
        Task<BaseResponse<HopDongResponse>> TakeQuotationById(int Id);
        Task<BaseResponse<QuotationResponse>> GetQuotationById(int Id);

    }
    public class QuotationService : IQuotationService
    {
        private readonly DataContext _datacontext;
        private readonly IExportService _exportService;
        private readonly ILogService _logService;
        public QuotationService(DataContext datacontext, IExportService exportService, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _exportService = exportService;
        }

        #region Quotation
        public async Task<BaseResponse<QuotationResponse>> TakeAlls(QuotationRequest request)
        {
            var result = new BaseResponse<QuotationResponse> { };
            try
            {
                var query = _datacontext.WMS_Quotations.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.DocumentNo.Contains(request.Term));
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.CustomerCode.HasValue())
                {
                    query = query.Where(d => d.CustomerCode == request.CustomerCode);
                }
                if (request.EmployeeCode.HasValue())
                {
                    query = query.Where(d => d.EmployeeCode == request.EmployeeCode);
                }
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.Date);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.Date <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<QuotationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<Quotation_LineResponse>> TakeAllsQuotation_Line(QuotationRequest request, int Id)
        {
            var result = new BaseResponse<Quotation_LineResponse> { };
            try
            {
                var query = _datacontext.WMS_QuotationLines.Where(x => x.Quotation_Id == Id).AsQueryable();
                result.DataCount = await query.CountAsync();
                query = query.OrderBy(d => d.OrderBy).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                var resultList = data.MapTo<Quotation_LineResponse>();

                foreach (var item in resultList)
                {
                    var item_DN = _datacontext.WMS_InventoryItems.AsQueryable().Where(x => x.ItemCode == item.Code && x.LocationCode == "45764568");
                    var item_BD = _datacontext.WMS_InventoryItems.AsQueryable().Where(x => x.ItemCode == item.Code && x.LocationCode == "45764569");
                    var item_Q7 = _datacontext.WMS_InventoryItems.AsQueryable().Where(x => x.ItemCode == item.Code && x.LocationCode == "45764570");
                    var item_HN = _datacontext.WMS_InventoryItems.AsQueryable().Where(x => x.ItemCode == item.Code && x.LocationCode == "45764571");
                    var item_QN = _datacontext.WMS_InventoryItems.AsQueryable().Where(x => x.ItemCode == item.Code && x.LocationCode == "45764572");

                    var DongNai = item_DN.Sum(s => s.Qty);
                    var BinhDuong = item_BD.Sum(s => s.Qty);
                    var HCM = item_Q7.Sum(s => s.Qty);
                    var HaNoi = item_HN.Sum(s => s.Qty);
                    var QuyNhon = item_QN.Sum(s => s.Qty);

                    item.DongNai = DongNai ?? 0;
                    item.BinhDuong = BinhDuong ?? 0;
                    item.HCM = HCM ?? 0;
                    item.HaNoi = HaNoi ?? 0;
                    item.QuyNhon = QuyNhon ?? 0;
                }

                foreach (var item in resultList)
                {
                    var flag = false;
                    var imageList = _datacontext.WMS_Item_Images.AsQueryable().Where(d => d.ItemCode == item.ItemCode).ToList();
                    if (imageList.Count > 0)
                    {
                        item.AbsolutePath = imageList[0].Image;
                        if (imageList != null)
                        {
                            foreach (var image in imageList)
                            {
                                if (!flag)
                                {
                                    if (image.IsMain == true)
                                    {
                                        item.AbsolutePath = image.Image;
                                        flag = true;
                                    }
                                }

                            }
                        }
                    }
                }
                result.Data = resultList;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<Quotation_TemplateResponse>> TakeAllsQuotation_Template(Quotation_TemplateRequest request)
        {
            var result = new BaseResponse<Quotation_TemplateResponse> { };
            try
            {
                var query = _datacontext.WMS_Quotation_Templetes.AsQueryable();
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<Quotation_TemplateResponse>();
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

        #region Funtioning
        public async Task<BaseResponse<QuotationResponse>> CreateOrUpdate_Quotation(QuotationRequest request)
        {
            var result = new BaseResponse<QuotationResponse> { };
            try
            {
                if(request.Id == 0) //Tạo mới
                {
                    var quotation = _datacontext.WMS_Quotations.Add(new WMS_Quotation
                    {
                        DocumentNo = request.DocumentNo,
                        Date = request.Date,
                        CustomerCode = request.CustomerCode,
                        EmployeeCode = request.EmployeeCode,
                        Time = request.Time,
                        CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        CreationDate = DateTime.Now,
                        LastUpdatedBy = TokenHelper.CurrentIdentity().UserName,
                        LastUpdateDate = DateTime.Now,
                        Note = request.GhiChu,
                        PaymentType = request.ThanhToan,
                        WarrantyType = request.BaoHanh,
                        WeliveryType = request.GiaoNhan,
                        IsSendEmail = false,
                        CommissionRate = request.PhanTramChiecKhau,
                        CommissionAmt = request.TienChietKhau,
                        Amt = request.TongTien,
                        AmtLCY = request.ConLai,
                        VATRate = request.PhanTramVAT,
                        VAT = request.TienVAT,
                        LocationCode = request.LocationCode,
                    });
                    foreach(var line in request.dataItemCheck)
                    {
                        _datacontext.WMS_QuotationLines.Add(new WMS_QuotationLine
                        {
                            Quotation_Id = quotation.Id,
                            ItemCode = line.Code,
                            UnitPrice = line.price,
                            Description = line.Description,
                            OrderBy = line.OrderBy,
                            ChatLuong_Id = line.ChatLuongId,
                            Qty = line.qty,
                            Amt = line.amt,
                            Status = 1,
                        });
                     
                    }
                    _datacontext.WMS_TransactionInformations.Add(new WMS_TransactionInformation
                    {
                        TransactionType_Id = 2,
                        Date = DateTime.Now,
                        Year = DateTime.Now.Year,
                        CustomerCode = request.CustomerCode,
                        BeginTime = request.Date,
                        Description = "Báo giá số " + request.DocumentNo,
                        EmployeeCode = request.EmployeeCode,
                        CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        CreationDate = DateTime.Now,
                        AppointmentDate = request.Time,
                        DanhGia_Id = 1,
                        BaoGia_Id = quotation.Id,
                        LocationCode = quotation.LocationCode,
                    });
                    await _datacontext.SaveChangesAsync();
                    result.Success = true;
                    result.Item = quotation.MapTo<QuotationResponse>();
                }
                else //Cập nhật
                {
                    var quotation = await _datacontext.WMS_Quotations.SingleOrDefaultAsync(x => x.Id == request.Id);
                    if(quotation != null)
                    {
                        quotation.DocumentNo = request.DocumentNo;
                        quotation.Date = request.Date;
                        quotation.CustomerCode = request.CustomerCode;
                        quotation.EmployeeCode = request.EmployeeCode;
                        quotation.Time = request.Time;
                        quotation.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
                        quotation.LastUpdateDate = DateTime.Now;
                        quotation.Note = request.GhiChu;
                        quotation.PaymentType = request.ThanhToan;
                        quotation.WarrantyType = request.BaoHanh;
                        quotation.WeliveryType = request.GiaoNhan;
                        quotation.IsSendEmail = false;
                        quotation.CommissionRate = request.PhanTramChiecKhau;
                        quotation.CommissionAmt = request.TienChietKhau;
                        quotation.Amt = request.TongTien;
                        quotation.AmtLCY = request.ConLai;
                        quotation.VATRate = request.PhanTramVAT;
                        quotation.VAT = request.TienVAT;
                        quotation.LocationCode = request.LocationCode;
                        var quotationline = await _datacontext.WMS_QuotationLines.Where(x => x.Quotation_Id == quotation.Id).ToListAsync();
                        _datacontext.WMS_QuotationLines.RemoveRange(quotationline);
                        foreach (var line in request.dataItemCheck)
                        {
                            _datacontext.WMS_QuotationLines.Add(new WMS_QuotationLine
                            {
                                Quotation_Id = quotation.Id,
                                ItemCode = line.Code,
                                UnitPrice = line.price,
                                Description = line.Description,
                                OrderBy = line.OrderBy,
                                ChatLuong_Id = line.ChatLuongId,
                                Qty = line.qty,
                                Amt = line.amt,
                                Status = 1,
                            });
                        }
                        var transaction = await _datacontext.WMS_TransactionInformations.SingleOrDefaultAsync(x => x.BaoGia_Id == quotation.Id);
                        if (transaction != null)
                        {
                            transaction.Date = DateTime.Now;
                            transaction.Year = DateTime.Now.Year;
                            transaction.CustomerCode = request.CustomerCode;
                            transaction.BeginTime = request.Date;
                            transaction.EmployeeCode = request.EmployeeCode;
                            transaction.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
                            transaction.LastUpdateDate = DateTime.Now;
                            transaction.AppointmentDate = request.Time;
                            transaction.LocationCode = request.LocationCode;
                        }
                        var customer = await _datacontext.WMS_Customers.SingleOrDefaultAsync(x => x.Code == request.CustomerCode);
                        if(customer != null)
                        {
                            customer.PhoneNo = request.CustomerPhoneNo;
                            customer.Email = request.CustomerEmail;
                            customer.Address = request.CustomerAddress;
                        }
                        var employee = await _datacontext.WMS_Employees.SingleOrDefaultAsync(x => x.EmployeeCode == request.EmployeeCode);
                        if (employee != null)
                        {
                            employee.Tel = request.EmployeePhoneNo;
                            employee.Email = request.EmployeeEmail;
                        }
                    }
                    await _datacontext.SaveChangesAsync();
                    result.Success = true;
                    result.Item = quotation.MapTo<QuotationResponse>();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> Remove_Quotation(QuotationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var quotation = await _datacontext.WMS_Quotations.SingleOrDefaultAsync(x => x.Id == request.Id);
                
                var transaction = await _datacontext.WMS_TransactionInformations.Where(x => x.BaoGia_Id == quotation.Id).ToListAsync();
                if(transaction != null)
                {
                    _datacontext.WMS_TransactionInformations.RemoveRange(transaction);
                }
                var quotationline = await _datacontext.WMS_QuotationLines.Where(x => x.Quotation_Id == quotation.Id).ToListAsync();
                if(quotationline != null)
                {
                    _datacontext.WMS_QuotationLines.RemoveRange(quotationline);
                }
                _datacontext.WMS_Quotations.Remove(quotation);
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
        public async Task<BaseResponse<WordStreamResponse>> SearchData_WordStream(int quotationId)
        {
            var result = new BaseResponse<WordStreamResponse> { };
            try
            {
                if(quotationId == 0)
                {
                    result.Message = "Không tìm thấy phiếu báo giá";
                    return result;
                }    
                var query = await _datacontext.WMS_Quotations.FindAsync(quotationId);
                result.Item = query.MapTo<WordStreamResponse>();
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

        public async Task<BaseResponse<HopDongResponse>> TakeQuotation(string KhachHang_Code)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = await _datacontext.WMS_Quotations.AsNoTracking().Where(code => code.CustomerCode == KhachHang_Code).ToListAsync();
                result.Data = query.MapTo<HopDongResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<QuotationResponse>> SearchQuotation(int QuotationCode)
        {
            var result = new BaseResponse<QuotationResponse> { };
            try
            {
                var query = await _datacontext.WMS_Quotations.SingleOrDefaultAsync(x => x.Id == QuotationCode);
                result.Item = query.MapTo<QuotationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<Quotation_LineResponse>> TakeQuotationLine(int Quotation_Code)
        {
            var result = new BaseResponse<Quotation_LineResponse> { };
            try
            {
                var query = await _datacontext.WMS_QuotationLines.AsNoTracking().Where(code => code.Quotation_Id == Quotation_Code).ToListAsync();
                result.Data = query.MapTo<Quotation_LineResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<Quotation_LineResponse>> TakeExport(QuotationRequest request)
        {
            var result = new BaseResponse<Quotation_LineResponse> { };
            try
            {
                var quotationLine =await _datacontext.WMS_QuotationLines.AsQueryable().Where(d => d.Quotation_Id == request.Id).OrderBy(d => d.OrderBy).ToArrayAsync();
                List<Quotation_LineResponse> quotationList = new List<Quotation_LineResponse>();
                foreach (var item in quotationLine)
                {
                    var img = await _datacontext.WMS_Item_Images.FirstOrDefaultAsync(d => d.ItemCode == item.ItemCode);
                    if (img == null)
                    {
                        var newData = new Quotation_LineResponse()
                        {
                            Name = item.WMS_Item.Name,
                            ImageByte = null,
                            Model = item.WMS_Item.Model,
                            qty = item.Qty,
                            price = item.UnitPrice,
                            amt = item.Amt,
                            OrderBy = item.OrderBy,
                        };
                        quotationList.Add(newData);
                    }
                    else
                    {
                        var newData = new Quotation_LineResponse()
                        {
                            Name = item.WMS_Item.Name,
                            ImageByte = img.Image,
                            Model = item.WMS_Item.Model,
                            qty = item.Qty,
                            price = item.UnitPrice,
                            LinkVideo = item.WMS_Item.LinkVideo,
                            amt = item.Amt,
                            OrderBy = item.OrderBy,
                        };
                        quotationList.Add(newData);
                    }
                }
                result.Data = quotationList; 

                /*result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id);
                request.Page = 0;
                request.PageSize = int.MaxValue;
                var data = await query.ToListAsync();
                result.Data = data.MapTo<DayoffsResponse>();*/

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDongResponse>> TakeQuotationById(int Id)
        {
            var result = new BaseResponse<HopDongResponse> { };
            try
            {
                var query = await _datacontext.WMS_Quotations.FindAsync(Id);
                result.Item = query.MapTo<HopDongResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<QuotationResponse>> GetQuotationById(int Id)
        {
            var result = new BaseResponse<QuotationResponse> { };
            try
            {
                var query = await _datacontext.WMS_Quotations.FindAsync(Id);
                result.Item = query.MapTo<QuotationResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<string> ExcelExportQuotation(string templateName, QuotationRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            var datas = await TakeExport(request);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _exportService.ExportToXlsxQuotationImage(stream, templateName, datas, request);
                bytes = stream.ToArray();
            }
            string nameFileExport = $"DanhSachSanPhamBaoGia_{DateTime.Now.Ticks}.xlsx";
            string downloadFolder = "/download/" + "QLBaoGia";
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            File.WriteAllBytes(Path.Combine(exportFilePath), bytes);
            return downloadFolder + "/" + nameFileExport;
        }
    }
}
