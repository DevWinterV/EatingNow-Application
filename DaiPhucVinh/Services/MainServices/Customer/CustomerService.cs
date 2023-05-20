using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.MainServices.Common;
using DaiPhucVinh.Services.MainServices.Image;
using DaiPhucVinh.Services.MainServices.ImageRecords;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Customer;
using DaiPhucVinh.Shared.Employee;
using DaiPhucVinh.Shared.Feedback;
using DaiPhucVinh.Shared.HopDong;
using DaiPhucVinh.Shared.Notification;
using DaiPhucVinh.Shared.Product;
using DaiPhucVinh.Shared.Task;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Customer
{
    public interface ICustomerService
    {
        Task<BaseResponse<CustomerResponse>> TakeAlls(CustomerRequest request);
        Task<BaseResponse<CustomerTypeResponse>> TakeAllCustomerType(CustomerRequest request);
        Task<BaseResponse<CustomerResponse>> TakeCustomerById(string Id);
        Task<BaseResponse<CustomerResponse>> SearchCustomer(string CustomerCode);
        Task<BaseResponse<HopDong_ChiTietResponse>> TakeContractByCustomerCode(HopDongRequest request , string KhachHang_Code);
        Task<BaseResponse<CustomerResponse>> ProfileTakeDetail_ByUserLogin(CustomerRequest request);
        Task<BaseResponse<HopDong_ChiTietResponse>> TakeContractByContractLineCode(HopDong_ChiTietRequest request, int hopDongCode);
        Task<BaseResponse<bool>> CreateCustomer(CustomerRequest request);
        Task<BaseResponse<bool>> UpdateCustomer(CustomerRequest request);
        Task<BaseResponse<bool>> RemoveCustomer(CustomerRequest request);

        #region [APP]
        Task<BaseResponse<ProductByCustomerDto>> TakeProductByCustomerCode(HopDongRequest request);
        Task<BaseResponse<TaskMobileResponse>> TakeTaskProductByCustomerCode(TaskRequest request);
        Task<BaseResponse<bool>> UpdateTaskByCustomerCode(TaskRequest request);
        Task<BaseResponse<CustomerResponse>> ProfileTakeDetail(CustomerRequest request);
        Task<BaseResponse<bool>> ProfileEditImage(CustomerRequest request, HttpPostedFile file);
        //Feeback
        Task<BaseResponse<FeedbackResponse>> CustomerFeedback_ByUserId(FeedbackRequest request);
        Task<BaseResponse<bool>> CustomerFeedback_Create(FeedbackRequest request);
        Task<BaseResponse<bool>> CustomerFeedback_Edit(FeedbackRequest request);
        Task<BaseResponse<bool>> CustomerFeedback_Remove(FeedbackRequest request);
        #endregion
    }
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ICommonService _commonService;
        private readonly IImageRecordService _imageRecordService;
        private readonly IImageService _imageService;
        private readonly ISetting _settingService;
        public CustomerService(DataContext datacontext, ILogService logService, ICommonService commonService, IImageService imageService, IImageRecordService imageRecordService, ISetting settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _commonService = commonService;
            _imageRecordService = imageRecordService;
            _imageService = imageService;
            _settingService = settingService;
        }
        #region [WebApp]
        public async Task<BaseResponse<CustomerResponse>> TakeAlls(CustomerRequest request)
        {
            var result = new BaseResponse<CustomerResponse> { };
            try
            {
                var query = _datacontext.WMS_Customers.AsQueryable();
                string Name_Unsigned = StringExtensions.ConvertToUnsign(request.Term);
                if (request.Term.HasValue())
                {
                    query = query.Where(d => d.Name_Unsigned.Contains(Name_Unsigned) ||
                                             d.PhoneNo.Contains(request.Term) ||
                                             d.TaxCode.Contains(request.Term));
                }
                if (request.TinhThanh_Id > 0)
                {
                    query = query.Where(d => d.TinhThanh_Id == request.TinhThanh_Id);
                }
                if (request.EmployeeCode.HasValue())
                {
                    query = query.Where(d => d.EmployeeCode == request.EmployeeCode);
                }
                if (request.checkCustomerCode != null && request.checkCustomerCode.Any())
                {
                    query = query.Where(x => x.Code != request.checkCustomerCode);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                var listCustomer = data.MapTo<CustomerResponse>();
                foreach (var customer in listCustomer)
                {
                    if(customer.UserId == null || customer.UserId == 0)
                    {
                        customer.IsExistAccount = false;
                    }
                    else
                    {
                        var existCustomer = await _datacontext.Users.FirstOrDefaultAsync(x => x.Id == customer.UserId && !x.Deleted);
                        if (existCustomer != null)
                        {
                            customer.IsExistAccount = true;
                            customer.DisplayName = existCustomer.DisplayName;
                            customer.UserName = existCustomer.UserName;
                            customer.Active = existCustomer.Active;
                        }
                        else
                        {
                            customer.IsExistAccount = false;
                        }
                    }
                }
                result.Data = listCustomer;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<CustomerTypeResponse>> TakeAllCustomerType(CustomerRequest request)
        {
            var result = new BaseResponse<CustomerTypeResponse> { };
            try
            {
                var query = _datacontext.WMS_CustomersTypes.AsQueryable();
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<CustomerTypeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
    
        public async Task<BaseResponse<CustomerResponse>> TakeCustomerById(string Id)
        {
            var result = new BaseResponse<CustomerResponse> { };
            try
            {
                var data = await _datacontext.WMS_Customers.FindAsync(Id);
                result.Item = data.MapTo<CustomerResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<CustomerResponse>> SearchCustomer(string CustomerCode)
        {
            var result = new BaseResponse<CustomerResponse> { };
            try
            {
                var query = await _datacontext.WMS_Customers.SingleOrDefaultAsync(x => x.Code == CustomerCode);
                result.Item = query.MapTo<CustomerResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
    
        public async Task<BaseResponse<HopDong_ChiTietResponse>> TakeContractByCustomerCode(HopDongRequest request, string KhachHang_Code)
        {
            var result = new BaseResponse<HopDong_ChiTietResponse> { };
            try
            {
                List<object> hopdongList = new List<object>();
                var query = await _datacontext.WMS_HopDongs.AsNoTracking().Where(code => code.KhachHang_Code == KhachHang_Code).OrderByDescending(d => d.NgayTaoHopDong).ToListAsync();
                foreach(var hopdong in query)
                {
                    var chitiet = await _datacontext.WMS_HopDong_ChiTiets.AsNoTracking().Where(d => d.HopDong_Id == hopdong.Id).ToListAsync();
                    foreach(var detail in chitiet)
                    {
                        hopdongList.Add(detail);
                    }
                    
                }
                result.DataCount = hopdongList.Count();
                var data =  hopdongList.Skip(request.Page * request.PageSize).Take(request.PageSize);
                result.Data = data.MapTo<HopDong_ChiTietResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<HopDong_ChiTietResponse>> TakeContractByContractLineCode(HopDong_ChiTietRequest request, int hopDongCode)
        {
            var result = new BaseResponse<HopDong_ChiTietResponse> { };
            try
            {
                var query =  _datacontext.WMS_HopDong_ChiTiets.AsNoTracking().Where(code => code.HopDong_Id == hopDongCode);
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.Id).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<HopDong_ChiTietResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateCustomer(CustomerRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id == 0)
                {
                    if(request.EmployeeCode == "")
                    {
                        request.EmployeeCode = null;
                    }
                    if(request.CustomerType_Id == 0)
                    {
                        request.CustomerType_Id = null;
                    }
                    if(request.TinhThanh_Id == 0)
                    {
                        request.TinhThanh_Id = null;
                    }
                    var customer = new WMS_Customers
                    {
                        Code = await _commonService.AutoGencode(nameof(WMS_Customers)),
                        Name = request.Name,
                        Name_Unsigned = StringExtensions.ConvertToUnsign(request.Name),
                        Address = request.Address,
                        TaxCode = request.TaxCode,
                        PersonEpresent = request.PersonRepresent,
                        Position = request.Position,
                        PhoneNo = request.PhoneNo,
                        FaxNo = request.FaxNo,
                        Email = request.Email,
                        Bank = request.Bank,
                        BankAccount = request.BankAccount,
                        PersonContact = request.PersonContact,
                        ReciverAddress = request.ReciverAddress,
                        LienHeKhac = request.LienHeKhac,
                        CustomerType_Id = request.CustomerType_Id,
                        TinhThanh_Id = request.TinhThanh_Id,
                        Tinh = request.Tinh,
                        EmployeeCode = request.EmployeeCode,
                        CreationDate = DateTime.Now,
                        CreatedBy = TokenHelper.CurrentIdentity().UserName,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = TokenHelper.CurrentIdentity().UserName,
                    };
                    _datacontext.WMS_Customers.Add(customer);
                }
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
        public async Task<BaseResponse<bool>> UpdateCustomer(CustomerRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    if (request.EmployeeCode == "")
                    {
                        request.EmployeeCode = null;
                    }
                    if (request.CustomerType_Id == 0)
                    {
                        request.CustomerType_Id = null;
                    }
                    if (request.TinhThanh_Id == 0)
                    {
                        request.TinhThanh_Id = null;
                    }
                    var customer = await _datacontext.WMS_Customers.FindAsync(request.Code);
                    if (customer != null)
                    {
                        customer.Name = request.Name;
                        customer.Name_Unsigned = StringExtensions.ConvertToUnsign(request.Name);
                        customer.Address = request.Address;
                        customer.TaxCode = request.TaxCode;
                        customer.PersonEpresent = request.PersonRepresent;
                        customer.Position = request.Position;
                        customer.PhoneNo = request.PhoneNo;
                        customer.FaxNo = request.FaxNo;
                        customer.Email = request.Email;
                        customer.Bank = request.Bank;
                        customer.BankAccount = request.BankAccount;
                        customer.PersonContact = request.PersonContact;
                        customer.ReciverAddress = request.ReciverAddress;
                        customer.LienHeKhac = request.LienHeKhac;
                        customer.CustomerType_Id = request.CustomerType_Id;
                        customer.TinhThanh_Id = request.TinhThanh_Id;
                        customer.EmployeeCode = request.EmployeeCode;
                        customer.LastUpdateDate = DateTime.Now;
                        customer.LastUpdatedBy = TokenHelper.CurrentIdentity().UserName;
                    }
                }

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
        public async Task<BaseResponse<bool>> RemoveCustomer(CustomerRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var customer = await _datacontext.WMS_Customers.SingleOrDefaultAsync(x => x.Id == request.Id);
                var error =new  BaseResponse<string> { Success = false};
                try
                {
                    if (customer != null)
                    {
                        _datacontext.WMS_Customers.Remove(customer);
                        await _datacontext.SaveChangesAsync();
                        result.Success = true; 
                        error.Success = true;
                    }
                }
                catch
                {
                    error.Success = false;
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        #endregion        #region [APP]

        #region [APP]

        #region Product by CustomerCode
        public async Task<BaseResponse<ProductByCustomerDto>> TakeProductByCustomerCode(HopDongRequest request)
        {
            var result = new BaseResponse<ProductByCustomerDto> { };
            try
            {
                var query = _datacontext.WMS_HopDong_ChiTiets.AsQueryable();
                if (request == null)
                {
                    result.Message = "Điều kiện rỗng";
                    return result;
                }
                if (request.Term.HasValue())
                {
                    query = query.Where(d => (d.WMS_Item.Name.Contains(request.Term) 
                                                || d.WMS_Item.Model.Contains(request.Term)
                                                || d.WMS_Item.Code.Contains(request.Term))
                                                && d.WMS_Item.IsDelete.HasValue && !d.WMS_Item.IsDelete.Value);
                }
                #region Old Code
                //if (!string.IsNullOrEmpty(request.CustomerCode))
                //{
                //    query = query.Where(x => x.WMS_HopDong.KhachHang_Code.Equals(request.CustomerCode)
                //                                    && x.WMS_HopDong.IsDaKy.HasValue && x.WMS_HopDong.IsDaKy.Value
                //                                    && x.WMS_Item.IsDelete.HasValue && !x.WMS_Item.IsDelete.Value);
                //    var dataGroupDate = query.GroupBy(g => new { NgayKy = DbFunctions.TruncateTime(g.WMS_HopDong.NgayKy), SoHopDong = g.WMS_HopDong.SoHopDong });
                //    var dataDate = await dataGroupDate.Select(s => new ProductByCustomerDto
                //    {
                //        NgayKy = s.Key.NgayKy,
                //        SoHopDong = s.Key.SoHopDong,
                //        ListProduct = s.Select(x => new ProductDto
                //        {
                //            Code = x.WMS_Item.Code,
                //            Name = x.WMS_Item.Name,
                //            Status = x.ChatLuong != null ? x.ChatLuong : (x.ChatLuong_Id.HasValue ? _datacontext.WMS_ItemGroups.FirstOrDefault(f => f.Id == x.ChatLuong_Id.Value).Name : null),
                //            Model = x.WMS_Item.Model,
                //            Specifications = x.WMS_Item.Specifications,
                //            CountryOfOriginCode = x.WMS_Item.CountryOfOriginCode,
                //            ImportDate = x.WMS_HopDong.NgayKy,
                //            ImageRecordId = x.WMS_Item.ImageRecordId,
                //        }).ToList()
                //    }).OrderBy(o => o.NgayKy).Skip(request.Page * request.PageSize).Take(request.PageSize).ToListAsync();

                //    foreach (var data in dataDate)
                //    {
                //        foreach (var item in data.ListProduct)
                //        {
                //            item.MainImagePath = MainImagePath(item.ImageRecordId);
                //            item.PdfPath = GuidePDFPath(item.Code);
                //        }
                //    }

                //    result.Data = dataDate;
                //    result.Success = true;
                //}
                #endregion

                if (!string.IsNullOrEmpty(request.CustomerCode))
                {
                    query = query.Where(x => x.WMS_HopDong.KhachHang_Code.Equals(request.CustomerCode)
                                                    && x.WMS_HopDong.IsDaKy.HasValue && x.WMS_HopDong.IsDaKy.Value
                                                    && x.WMS_Item.IsDelete.HasValue && !x.WMS_Item.IsDelete.Value);
                    var dataGroupDate = query.GroupBy(g => new { SoHopDong = g.WMS_HopDong.SoHopDong, NgayKy = g.WMS_HopDong.NgayKy });
                    var dataDate = await dataGroupDate.Select(s => new ProductByCustomerDto
                    {
                        SoHopDong = s.Key.SoHopDong,
                        NgayKy = s.Key.NgayKy,
                        ListProduct = s.Select(x => new ProductDto
                        {
                            Code = x.WMS_Item.Code,
                            Name = x.WMS_Item.Name,
                            Status = x.ChatLuong != null ? x.ChatLuong : (x.ChatLuong_Id.HasValue ? _datacontext.WMS_ItemGroups.FirstOrDefault(f => f.Id == x.ChatLuong_Id.Value).Name : null),
                            Model = x.WMS_Item.Model,
                            Specifications = x.WMS_Item.Specifications,
                            CountryOfOriginCode = x.WMS_Item.CountryOfOriginCode,
                            ImportDate = x.WMS_HopDong.NgayKy,
                            ImageRecordId = x.WMS_Item.ImageRecordId,
                        }).ToList()
                    }).OrderByDescending(o => o.NgayKy).Skip(request.Page * request.PageSize).Take(request.PageSize).ToListAsync();

                    foreach (var data in dataDate)
                    {
                        foreach (var item in data.ListProduct)
                        {
                            item.MainImagePath = MainImagePath(item.ImageRecordId);
                            item.PdfPath = GuidePDFPath(item.Code);
                            item.ListTask = TaskList(item.Code);
                        }
                    }

                    result.Data = dataDate;
                    result.Success = true;
                }


            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        #endregion

        #region Task by CustomerCode
        public async Task<BaseResponse<TaskMobileResponse>> TakeTaskProductByCustomerCode(TaskRequest request)
        {
            var result = new BaseResponse<TaskMobileResponse> { };
            try
            {
                var queryTask = _datacontext.FUV_Tasks.AsQueryable();
                if (request == null)
                {
                    result.Message = "Điều kiện rỗng";
                    result.Success = false;
                    return result;
                }
                if (!string.IsNullOrEmpty(request.CustomerCode))
                {
                    queryTask = queryTask.Where(w => w.CustomerCode.Equals(request.CustomerCode) && !w.Deleted);
                }
                if (request.FromDt.HasValue)
                {
                    var fromDate = request.FromDt.Value.Date;
                    queryTask = queryTask.Where(w => fromDate <= DbFunctions.TruncateTime(w.UpdatedAt));
                }
                if (request.ToDt.HasValue)
                {
                    var toDate = request.ToDt.Value.Date;
                    queryTask = queryTask.Where(w => DbFunctions.TruncateTime(w.UpdatedAt.Value) <= toDate);
                }
                var data = await queryTask.ToListAsync();

                var mapdata = data.Select(s => new TaskMobileResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    CustomerAddress = s.WMS_Customer != null ? s.WMS_Customer.Address : "",
                    AssignUserName = s.User != null ? s.User.DisplayName : "",
                    CustomerPhone = s.CustomerPhone,
                    TimeToStart = s.TimeToStart,
                    Deadline = s.Deadline.Value,
                    Description = s.Description,
                    RatingPoint = s.RatingPoint,
                    RatingComment = s.RatingComment,
                    Note_Machine = s.Note_Machine,
                    Note_Suggest = s.Note_Suggest,
                    TaskTypeName = s.FUV_TaskTypes != null ? s.FUV_TaskTypes.Name : "",
                    StartDate = s.StartDate,
                    TaskResultId = s.TaskResultId,
                    TaskResultName = s.FUV_TaskResult != null ? s.FUV_TaskResult.Name : "",
                    ListAfterImage = ListImage(s.AfterProcessImages),
                    ListBeforeImage = ListImage(s.BeforeProcessImages),
                    ListDocumentImage = ListImage(s.DocumentImages)
                }).ToList();

                if (data == null)
                {
                    result.Message = "Không tìm thấy dữ liệu";
                    result.Success = false;
                    return result;
                }
                //result.Data = data.MapTo<TaskMobileResponse>();
                result.Data = mapdata;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> UpdateTaskByCustomerCode(TaskRequest request)
        {
            var result = new BaseResponse<bool> { };
            if (request == null)
            {
                result.Success = false;
                result.Message = "Lỗi truyền dữ liệu";
                return result;
            }
            try
            {
                var task = await _datacontext.FUV_Tasks.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (task == null)
                {
                    result.Message = "Không tìm thấy công việc";
                    return result;
                }
                task.RatingPoint = request.RatingPoint;
                task.RatingComment = request.RatingComment;

                _datacontext.SaveChanges();

                result.Success = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        #endregion

        #region Thông tin cá nhân
        public async Task<BaseResponse<CustomerResponse>> ProfileTakeDetail(CustomerRequest request)
        {
            var result = new BaseResponse<CustomerResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = await _datacontext.WMS_Customers.FirstOrDefaultAsync(x => x.UserId == UserId);
                result.Item = query.MapTo<CustomerResponse>();
                if (result.Item.ImageRecordId.HasValue())
                {
                    var IdImage = result.Item?.ImageRecordId;
                    result.Item.Avatar = await _imageRecordService.GetImageList(IdImage);
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<bool>> ProfileEditImage(CustomerRequest request, HttpPostedFile file)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var entity = await _datacontext.WMS_Customers.FirstOrDefaultAsync(d => d.UserId == UserId);
                entity.Name = request.Name;
                entity.PhoneNo = request.PhoneNo;
                entity.Address = request.Address;
                if (file != null)
                {
                    HttpPostedFile file_Img = null;
                    if (_imageService.CheckImageFileType(file.FileName))
                    {
                        file_Img = file;
                    }
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file_Img.InputStream.CopyTo(ms);
                        var Images = await _imageService.InsertImage(ms.GetBuffer(), file_Img.FileName, "DaiPhucVinh\\Image");
                        var setting = _settingService.LoadSetting<MetadataSettings>();
                        await _imageService.ResizeImage(Images.Image.Id, int.Parse(setting.Photomaxwidth));
                        //await _imageService.ResizeImage(Images.Image.Id, 512);
                        entity.ImageRecordId = Images.Image.Id;
                    }
                }

                await _datacontext.SaveChangesAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }
            return result;
        }
        public async Task<BaseResponse<CustomerResponse>> ProfileTakeDetail_ByUserLogin(CustomerRequest request)
        {
            var result = new BaseResponse<CustomerResponse> { };
            try
            {
                var UserLogin = TokenHelper.CurrentIdentity().UserName;
                var user = await _datacontext.Users.FirstOrDefaultAsync(x => x.UserName == UserLogin);
                if (user == null)
                {
                    result.Message = "Không tìm được tài khoản đăng nhập";
                    result.Success = false;
                    return result;
                }

                result.Item = user.MapTo<CustomerResponse>();
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

        #region Customer Feedback
        public async Task<BaseResponse<FeedbackResponse>> CustomerFeedback_ByUserId(FeedbackRequest request)
        {
            var result = new BaseResponse<FeedbackResponse> { };
            try
            {
                var UserId = TokenHelper.CurrentIdentity().UserId;
                var query = _datacontext.FUV_Feedbacks.AsQueryable().Where(s => !s.Deleted && s.UserId == UserId);
                if (request.FromDt.HasValue)
                {
                    var _FromDt = request.FromDt.Value.Date;
                    query = query.Where(x => _FromDt <= x.UpdatedAt);
                }
                if (request.ToDt.HasValue)
                {
                    var _ToDt = request.ToDt.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.UpdatedAt <= _ToDt);
                }
                result.DataCount = await query.CountAsync();
                query = query.OrderByDescending(d => d.CreatedAt).Skip(request.Page * request.PageSize).Take(request.PageSize);
                var data = await query.ToListAsync();
                result.Data = data.MapTo<FeedbackResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CustomerFeedback_Create(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var dayoffs = _datacontext.FUV_Feedbacks.Add(new FUV_Feedbacks
                {
                    Title = request.Title,
                    Description = request.Description,
                    UserId = TokenHelper.CurrentIdentity().UserId,
                    Approved = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = TokenHelper.CurrentIdentity().UserName,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = TokenHelper.CurrentIdentity().UserName,
                    Deleted = false,
                });
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
        public async Task<BaseResponse<bool>> CustomerFeedback_Edit(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Feedbacks.FindAsync(request.Id);
                entity.Title = request.Title;
                entity.Description = request.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedBy = TokenHelper.CurrentIdentity().UserName;
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
        public async Task<BaseResponse<bool>> CustomerFeedback_Remove(FeedbackRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var entity = await _datacontext.FUV_Feedbacks.FindAsync(request.Id);
                entity.Deleted = true;
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
        #endregion

        #region [PRIVATE]
        private string MainImagePath(string listImageId)
        {
            string MainImage = "";
            var queryImageRecord = _datacontext.ImageRecords.AsQueryable();
            if (!string.IsNullOrEmpty(listImageId))
            {
                var listId = listImageId.Split(',').Where(x => int.TryParse(x, out _))
                                                                    .Select(int.Parse)
                                                                    .ToArray();
                if (listId.Count() > 0)
                {
                    foreach (var id in listId)
                    {
                        MainImage = "";
                        var mainImage = queryImageRecord.FirstOrDefault(x => x.Id == id
                                                                    && x.IsMain.HasValue && x.IsMain.Value
                                                                    && !x.Deleted);
                        if (mainImage != null)
                        {
                            MainImage = mainImage.AbsolutePath;
                            break;
                        }
                        else
                        {
                            int imgId = listId[0];
                            MainImage = queryImageRecord.FirstOrDefault(x => x.Id == imgId).AbsolutePath;
                        }
                    }
                }
            }
            return MainImage;
        }
        private List<ImageRecord> ListImage(string listImageId)
        {
            var queryImageRecord = _datacontext.ImageRecords.AsQueryable();
            var list = new List<ImageRecord>();

            if (!string.IsNullOrEmpty(listImageId))
            {
                var listId = listImageId.Split(',').Where(x => int.TryParse(x, out _))
                                                                    .Select(int.Parse)
                                                                    .ToArray();

                if (listId.Count() > 0)
                {
                    foreach (var id in listId)
                    {
                        var data = queryImageRecord.FirstOrDefault(d => d.Id == id);
                        if (data == null)
                        {
                            return list;
                        }
                        var img = new ImageRecord
                        {
                            FileName = data.FileName,
                            AbsolutePath = data.AbsolutePath,
                        };
                        list.Add(img);
                    }
                }
            }
            return list;
        }
        private string GuidePDFPath(string itemCode)
        {
            string pdfFilePath = "";
            var queryImageRecord = _datacontext.ImageRecords.AsQueryable();
            var queryUserGuide = _datacontext.FUV_UserGuides.AsQueryable();
            if (!string.IsNullOrEmpty(itemCode))
            {
                var dataUserGuides = queryUserGuide.FirstOrDefault(x => x.ItemCode == itemCode && !x.Deleted);
                if (dataUserGuides != null)
                {
                    var pdfId = dataUserGuides.PdfFile;
                    int pdfIdParse = int.Parse(pdfId);
                    var pdfFile = queryImageRecord.FirstOrDefault(x => x.Id == pdfIdParse);
                    if (pdfFile != null)
                    {
                        pdfFilePath = pdfFile.AbsolutePath;
                    }
                }
            }
            return pdfFilePath;
        }
        private List<TaskMobileResponse> TaskList(string itemCode)
        {
            var lstTask = new List<TaskMobileResponse>{ };

            var queryTask = _datacontext.FUV_Tasks.AsQueryable();

            try
            {
                if (!string.IsNullOrEmpty(itemCode))
                {
                    queryTask = queryTask.Where(x => x.ItemCode == itemCode);
                }
                var mapdata = queryTask.Select(s => new TaskMobileResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Model = _datacontext.WMS_Items.FirstOrDefault(x => x.Code == itemCode).Model,
                    CustomerAddress = s.WMS_Customer != null ? s.WMS_Customer.Address : "",
                    TimeToStart = s.TimeToStart,
                    Deadline = s.Deadline.Value,
                    StartDate = s.StartDate,
                    TaskResultId = s.TaskResultId,
                    TaskResultName = s.FUV_TaskResult != null ? s.FUV_TaskResult.Name : "",
                }).ToList();
                lstTask = mapdata;
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstTask;
        } 
        #endregion

        #endregion
    }
}
