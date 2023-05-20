using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Excel;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Word;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.inventoryitem;
using DaiPhucVinh.Shared.Location;
using DaiPhucVinh.Shared.Product;
using Falcon.Core;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO.Packaging;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Shared.Inventoryitem;

namespace DaiPhucVinh.Services.MainServices.InventoryItem
{
    public interface IInventoryItemService
    {
        Task<BaseResponse<InventoryitemResponse>> TakeAlls(InventoryitemRequest request);
        Task<BaseResponse<InventoryitemResponse>> TakeAllsInventoryByLocation(InventoryitemRequest request);
        Task<BaseResponse<InventoryitemResponse>> TakeExpport(InventoryitemRequest request);
        Task<string> ExportItemPrice(string templateName, InventoryitemRequest request);
    }
    public class InventoryItemService : IInventoryItemService
    {
        private readonly DataContext _datacontext;
        private readonly IExportService _exportService;
        private readonly ILogService _logService;

        public InventoryItemService(DataContext datacontext, IExportService exportService, ILogService logService)
        {
            _datacontext = datacontext;
            _exportService = exportService;
            _logService = logService;
        }
        public async Task<BaseResponse<InventoryitemResponse>> TakeAllsInventoryByLocation(InventoryitemRequest request)
        {
            var result = new BaseResponse<InventoryitemResponse> { };
            try
            {
                var query = _datacontext.WMS_Items.AsQueryable();
                var query1 = _datacontext.WMS_InventoryItems.AsQueryable();
                if (request.Term.HasValue())
                {
                    if(request.typeSearch == 1)
                    {
                        query = query.Where(d => d.Name.Contains(request.Term) || d.Model == request.Term);
                        query1 = query1.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model == request.Term);
                    }
                    if(request.typeSearch == 2)
                    {
                        query = query.Where(d => d.Name.Contains(request.Term) || d.Model.Contains(request.Term));
                        query1 = query1.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model.Contains(request.Term));
                    }
                }
                if (request.ItemGroupCode.HasValue())
                {
                    query = query.Where(d => d.ItemGroup_Code == request.ItemGroupCode);
                    query1 = query1.Where(d => d.WMS_Item.ItemGroup_Code == request.ItemGroupCode);
                }
                result.DataCount = await query.CountAsync();
                result.CustomData = new
                {
                    //TongSoLuong =  _datacontext.WMS_InventoryItems.AsQueryable()
                    TongSoLuong = await query1.SumAsync(d => d.Qty)
                };
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<InventoryitemResponse>();
                foreach (var item in result.Data)
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


                  //  item.OrderBy = item_OrderBy.OrderBy;
                    item.DongNai = DongNai ?? 0;
                    item.BinhDuong = BinhDuong ?? 0;
                    item.HCM = HCM ?? 0;
                    item.HaNoi = HaNoi ?? 0;
                    item.QuyNhon = QuyNhon ?? 0;
                    var qtyQuery = _datacontext.WMS_InventoryItems.Where(x => x.ItemCode == item.Code).ToList();
                    item.Qty = (qtyQuery.Sum(d => d.Qty)) ?? 0;

                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;

        }
        public async Task<BaseResponse<InventoryitemResponse>> TakeAlls(InventoryitemRequest request)
        {
            var result = new BaseResponse<InventoryitemResponse> { };
            try
            {
                var query = _datacontext.WMS_InventoryItems.AsQueryable();
                if (request.typeSearch == 1)
                {
                    if (request.Term.HasValue())
                    {
                        query = query.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model == request.Term);
                        var inventory = query.ToList();
                        if (inventory.Count == 0)
                        {
                            var item = _datacontext.WMS_Items.FirstOrDefault(x => x.Model == request.Term || x.Name.Contains(request.Term));
                            if (item != null)
                            {
                                var Item = new WMS_InventoryItem
                                {
                                    DocumentDate = DateTime.Now,
                                    DocumentType = 1,
                                    ItemCode = item.Code,
                                    CurrencyCode = "VND",
                                    ExchangeRate = 1,
                                    Qty = 0,
                                    LocationCode = "45764568",
                                };
                                _datacontext.WMS_InventoryItems.Add(Item);
                                await _datacontext.SaveChangesAsync();
                            }
                            query = _datacontext.WMS_InventoryItems.AsQueryable();
                            query = query.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model == request.Term);
                            var test = query.ToList();
                        }
                    }
                }
                if (request.typeSearch == 2)
                {
                    if (request.Term.HasValue())
                    {
                        query = query.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model.Contains(request.Term));
                    }
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.ItemGroupCode.HasValue())
                {
                    query = query.Where(d => d.WMS_Item.ItemGroup_Code == request.ItemGroupCode);
                }
                if (request.dataItemCode != null && request.dataItemCode.Any())
                {
                    query = query.Where(d => !request.dataItemCode.Contains(d.Id));
                }
                result.DataCount = await query.CountAsync();
                result.CustomData = new
                {
                    TongSoLuong = await query.SumAsync(d => d.Qty)
                };
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<InventoryitemResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<BaseResponse<InventoryitemResponse>> TakeExpport(InventoryitemRequest request)
        {
            var result = new BaseResponse<InventoryitemResponse> { };
            try
            {
                var query = _datacontext.WMS_InventoryItems.AsQueryable().Where(r => r.Qty != 0);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model.Contains(request.Term));
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.ItemGroupCode.HasValue())
                {
                    query = query.Where(d => d.WMS_Item.ItemGroup_Code == request.ItemGroupCode);
                }

                result.DataCount = await query.CountAsync();
                result.CustomData = new
                {
                    TongSoLuong = await query.SumAsync(d => d.Qty)
                };
                request.Page = 0;
                request.PageSize = int.MaxValue;
                var data = await query.ToListAsync();
                result.Data = data.MapTo<InventoryitemResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<BaseResponse<InventoryItemExportResponse>> TakeInventoryExpport(InventoryitemRequest request)
        {
            var result = new BaseResponse<InventoryItemExportResponse> { };
            try
            {
                var query = _datacontext.WMS_InventoryItems.AsQueryable();
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.WMS_Item.Name.Contains(request.Term) || d.WMS_Item.Model.Contains(request.Term));
                }
                if (request.LocationCode.HasValue())
                {
                    query = query.Where(d => d.LocationCode == request.LocationCode);
                }
                if (request.ItemGroupCode.HasValue())
                {
                    query = query.Where(d => d.WMS_Item.ItemGroup_Code == request.ItemGroupCode);
                }
                var entry = await query.ToListAsync();
                var listItem = entry.GroupBy(p => p.ItemCode).ToList();
                List<InventoryItemExportResponse> list = new List<InventoryItemExportResponse>();
                foreach (var item in listItem)
                {
                    //var productGroup = await _datacontext.WMS_ItemCategories.FirstOrDefaultAsync(d => d.Code.Equals(item.Key.ItemCategoryCode));
                    var productDetail = await _datacontext.WMS_Items.FirstOrDefaultAsync(d => d.Code.Equals(item.Key));
                    var tongton = (item.Sum(d => d.Qty)) ?? 0;
                    double HaNoi = 0, HCM = 0, BinhDuong = 0, DongNai = 0, QuyNhon = 0;
                    foreach (var element in item)
                    {
                        if(element.LocationCode == "45764571")
                        {
                            HaNoi = (_datacontext.WMS_InventoryItems.Where(d => d.ItemCode.Equals(item.Key) && d.LocationCode == element.LocationCode).Sum(d => d.Qty)) ?? 0;
                        }
                        if (element.LocationCode == "45764570")
                        {
                            HCM = (_datacontext.WMS_InventoryItems.Where(d => d.ItemCode.Equals(item.Key) && d.LocationCode == element.LocationCode).Sum(d => d.Qty)) ?? 0;
                        }
                        if (element.LocationCode == "45764569")
                        {
                            BinhDuong = (_datacontext.WMS_InventoryItems.Where(d => d.ItemCode.Equals(item.Key) && d.LocationCode == element.LocationCode).Sum(d => d.Qty)) ?? 0;
                        }
                        if (element.LocationCode == "45764568")
                        {
                            DongNai = (_datacontext.WMS_InventoryItems.Where(d => d.ItemCode.Equals(item.Key) && d.LocationCode == element.LocationCode).Sum(d => d.Qty)) ?? 0;
                        }
                        if (element.LocationCode == "45764572")
                        {
                            QuyNhon = (_datacontext.WMS_InventoryItems.Where(d => d.ItemCode.Equals(item.Key) && d.LocationCode == element.LocationCode).Sum(d => d.Qty)) ?? 0;
                        }

                    }
                    list.Add(new InventoryItemExportResponse
                    {
                        //ItemCategoryCode = item.Key.ItemCategoryCode,
                        //ItemCategoryName = productGroup.Name, //ten nhom sp
                        Name = productDetail.Name, //ten sp
                        Model = productDetail.Model,
                        TotalInventory = tongton,
                        HaNoi = HaNoi,
                        HCM = HCM,
                        BinhDuong = BinhDuong,
                        DongNai = DongNai,
                        QuyNhon = QuyNhon,
                    });
                }
                result.Data = list.MapTo<InventoryItemExportResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);

            }
            return result;
        }
        public async Task<string> ExportItemPrice ( string templateName,InventoryitemRequest request)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new ArgumentNullException(nameof(templateName));
            var datas = await TakeInventoryExpport(request);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _exportService.ExportToXlsxInventory(stream, templateName, datas,request);
                bytes = stream.ToArray();
            }
            string nameFileExport = $"DanhSachTonKhoHienHanh_{DateTime.Now.Ticks}.xlsx";
            string downloadFolder = "/download/" + request.LocationName;
            if (!Directory.Exists(CommonHelper.MapPath(downloadFolder)))
            {
                Directory.CreateDirectory(CommonHelper.MapPath(downloadFolder));
            }
            string exportFilePath = CommonHelper.MapPath(downloadFolder + "/" + nameFileExport);
            File.WriteAllBytes(Path.Combine(exportFilePath), bytes);
            await _datacontext.SaveChangesAsync();
            return downloadFolder + "/" + nameFileExport;
        }
    }
  
}
