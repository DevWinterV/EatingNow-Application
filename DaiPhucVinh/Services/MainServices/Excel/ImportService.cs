using DaiPhucVinh.Shared.Excel;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Excel
{
    public class ImportService
    {
        private readonly DataContext _dataContext;
        public ImportService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Import Khách hàng
        public async Task<object> KhachHang(List<CustomerModel> request)
        {
            try
            {
                //int newArea = 0, newTable = 0;
                foreach (var customers in request)
                {
                    //var kh = await _dataContext.WMS_Customers.FirstOrDefaultAsync(d => d.Name == customers.Name);
                    //if (kh == null)
                    //{
                    //    kh = new WMS_Customer
                    //    {
                    //        Code = await _commonService.AutoGencode(nameof(WMS_Customer)),
                    //        Name = customers.Name,
                    //        PartnerCode = customers.PartnerCode,
                    //        LocationCode = customers.LocationCode,
                    //        Address = customers.Address,
                    //        PhoneNo = customers.PhoneNo,
                    //        CardNumber = customers.CardNumber,
                    //        IDCard = customers.IDCard,
                    //        BirthDay = customers.BirthDay,
                    //        FaxNo = customers.FaxNo,
                    //        Email = customers.Email,
                    //        TaxCode = customers.TaxCode,
                    //        //CustomerGroup_Code = customers.CustomerGroup_Code,
                    //        //CustomerType_Code = customers.CustomerType_Code,
                    //        ConNoPhaiThu = customers.ConNoPhaiThu,
                    //        DiemHienTai = customers.DiemHienTai,
                    //        DiemTichLuy = customers.DiemTichLuu,
                    //        ChuKyThanhToan = customers.ChuKyThanhToan,
                    //        NumberBankAccount = customers.NumberBankAccount,
                    //        BankName = customers.BankName,
                    //        CreatedBy = customers.CreatedBy,
                    //        CreationDate = DateTime.Now,
                    //        LastUpdatedBy = customers.LastUpdatedBy,
                    //        LastUpdateDate = DateTime.Now,
                    //        IsActive = true
                    //    };
                    //    _posContext.WMS_Customers.Add(kh);
                    //    await _posContext.SaveChangesAsync();
                    //    _posContext.Dispose();
                    //    //newArea++;
                    //}                    
                }
                return new { Success = true/*, Ban = newTable, KhuVuc = newArea*/ };
            }
            catch (Exception ex)
            {
                return new { Success = false, Message = ex.ToString() };
            }
        }
        #endregion

    }
}
