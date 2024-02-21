using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Constants;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.CustomerNotification;
using DaiPhucVinh.Shared.DeliveryDriver;
using DaiPhucVinh.Shared.OrderHeader;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.Payment;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Falcon.Web.Core.Settings;
using Microsoft.AspNet.SignalR;
using Microsoft.ML;
using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;


namespace DaiPhucVinh.Services.MainServices.EN_CustomerService
{
    public interface IENCustomerService
    {
        Task<BaseResponse<EN_CustomerResponse>> CheckCustomer(EN_CustomerRequest request);
        Task<BaseResponse<EN_CustomerResponse>> TakeAllCustomer(EN_CustomerRequest request);
        Task<BaseResponse<OrderHeaderResponse>> TakeOrderByCustomer(EN_CustomerRequest request);


        Task<BaseResponse<EN_CustomerResponse>> TakeAllCustomerByProvinceId(EN_CustomerRequest request);
        Task<BaseResponse<EN_CustomerAddressResponse>> TakeAllCustomerAddressById(EN_CustomerRequest request);

        Task<BaseResponse<bool>> CreateOrderCustomer(EN_CustomerRequest request);
        Task<BaseResponse<bool>> PaymentConfirm(PaymentConfirmRequest request);

        Task<BaseResponse<bool>> CreateCustomerAddress(EN_CustomerAddressRequest request);

        Task<BaseResponse<bool>> UpdateToken(EN_CustomerRequest request);
        Task<BaseResponse<bool>> CreateNotificationCustomer(EN_CustomerNotificationRequest request);

        Task<BaseResponse<EN_CustomerResponse>> CheckCustomerEmail(EN_CustomerRequest request);
        Task<BaseResponse<bool>> UpdateInfoCustomer(EN_CustomerRequest request, HttpPostedFile file);
        Task<BaseResponse<EN_CustomerAddressResponse>> CheckCustomerAddress(EN_CustomerRequest request);
        Task<BaseResponse<bool>> DeleteAddress(EN_CustomerAddressRequest Id);
        Task<BaseResponse<bool>> RemoveOrderLine(OrderLineRequest request);
        Task<BaseResponse<bool>> RemoveOrderHeader(OrderLineRequest request);
        Task<BaseResponse<bool>> DeleteAllNotification(EN_CustomerNotificationRequest request);
        Task<BaseResponse<bool>> SetIsReadAllNotification(EN_CustomerNotificationRequest request);

        Task<BaseResponse<EN_CustomerNotificationResponse>> GetAllNotificationCustomer(EN_CustomerNotificationRequest request);

    }
    public class ENCustomerService : IENCustomerService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        private readonly ISettingService _settingService;
        public string HostAddress => HttpContext.Current.Request.Url.ToString().Replace(HttpContext.Current.Request.Url.PathAndQuery, "");


        public ENCustomerService(DataContext datacontext, ILogService logService, ISettingService settingService)
        {
            _datacontext = datacontext;
            _logService = logService;
            _settingService = settingService;
        }
        public async Task<BaseResponse<EN_CustomerResponse>> CheckCustomer(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerResponse> { };
            try
            {
                var query = _datacontext.EN_Customer.Where(x => x.CustomerId == request.CustomerId).AsQueryable();
                query = query.OrderBy(d => d.CustomerId);
                if (query.Count() > 0)
                {
                    // Trường hợp đã tồn tại một bản ghi với CustomerId đã cho
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<EN_CustomerResponse>();
                    result.Success = true;
                    result.DataCount = data.Count();
                }
                else
                {
                    if (request.Phone != "")
                    {
                        string phone = request.Phone;
                        string phoneoutput = phone.Replace("+84", "");
                        var queryphone = _datacontext.EN_Customer.Where(x => x.Phone.Contains(phoneoutput)).AsQueryable();
                        queryphone = queryphone.OrderBy(d => d.CustomerId);
                        if (queryphone.Any())
                        {
                            foreach (var customer in queryphone)
                            {
                                customer.CustomerId = request.CustomerId;
                            }
                            _datacontext.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                        }
                        var data = await queryphone.ToListAsync();
                        result.Data = data.MapTo<EN_CustomerResponse>();
                        result.Success = true;
                        result.DataCount = data.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<EN_CustomerAddressResponse>> CheckCustomerAddress(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerAddressResponse> { };
            try
            {
                var query = _datacontext.EN_CustomerAddress.Where(x => x.CustomerId == request.CustomerId && x.Defaut == true).AsQueryable();
                query = query.OrderBy(d => d.CustomerId);
                var data = await query.ToListAsync();
                if (data.Count > 0)
                {
                    result.Data = data.MapTo<EN_CustomerAddressResponse>();
                    result.Success = true;
                    result.DataCount = data.Count();
                }
                else if (request.Phone != "")
                {
                    string phone = request.Phone;
                    string phoneoutput = phone.Replace("84", "");
                    var queryphone = _datacontext.EN_Customer.Where(x => x.Phone.Contains(phoneoutput)).FirstOrDefault();
                    var addresses = _datacontext.EN_CustomerAddress.Where(x => x.CustomerId == queryphone.CustomerId && x.Defaut == true).AsQueryable();
                    addresses = addresses.OrderBy(d => d.CustomerId);
                    var data1 = await addresses.ToListAsync();
                    result.Data = data1.MapTo<EN_CustomerAddressResponse>();
                    result.Success = true;
                    result.DataCount = data.Count();
                }
                else
                {
                    result.Success = false;
                    result.Message = "NotFondAddress";
                    result.DataCount = data.Count();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> UpdateToken(EN_CustomerRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var checkCustomer = await _datacontext.EN_Customer.Where(x => x.CustomerId == request.CustomerId).FirstOrDefaultAsync();

                if (checkCustomer != null)
                {
                    if (request.TokenApp != null)
                    {
                        checkCustomer.TokenApp = request.TokenApp;
                    }
                    else
                    {
                        checkCustomer.TokenWeb = request.TokenWeb;
                    }
                    await _datacontext.SaveChangesAsync();
                    result.Success = true;
                }
                else
                {
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
        public async Task<BaseResponse<bool>> UpdateInfoCustomer(EN_CustomerRequest request, HttpPostedFile file)
        {
            var relativePath = "";
            var result = new BaseResponse<bool> { };
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (file != null)
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] pictureBinary = ms.GetBuffer();
                        string CustomerName = "DaiPhucVinh\\image";
                        var storageFolder = $@"\uploads\{CustomerName}";
                        if (!Directory.Exists(LocalMapPath(storageFolder)))
                            Directory.CreateDirectory(LocalMapPath(storageFolder));

                        string fileName = Path.GetFileName(file.FileName);
                        string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}" + "-" + $"{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                        var storageFolderPath = Path.Combine(LocalMapPath(storageFolder), newFileName);
                        File.WriteAllBytes(storageFolderPath, pictureBinary);

                        relativePath = Path.Combine(storageFolder, newFileName);
                    }

                    var checkCustomer = await _datacontext.EN_Customer.Where(x => x.CustomerId.Equals(request.CustomerId)).FirstOrDefaultAsync();

                    if (checkCustomer != null)
                    {
                        checkCustomer.CompleteName = request.CompleteName;
                        checkCustomer.Phone = request.Phone;
                        checkCustomer.Email = request.Email;
                        if (file != null)
                            checkCustomer.ImageProfile = HostAddress + GenAbsolutePath(relativePath);
                        else
                            checkCustomer.ImageProfile = request.ImageProfile;
                    }
                    else
                    {
                        if (request.CompleteName != "" && request.Phone != "" && request.CustomerId != "")
                        {
                            var newcustomer = new EN_Customer();
                            newcustomer.CustomerId = request.CustomerId;
                            newcustomer.CompleteName = request.CompleteName;
                            newcustomer.Phone = request.Phone;
                            newcustomer.Email = request.Email;
                            newcustomer.Status = true;
                            if (file != null)
                                newcustomer.ImageProfile = HostAddress + GenAbsolutePath(relativePath);
                            else
                                newcustomer.ImageProfile = "";
                            _datacontext.EN_Customer.Add(newcustomer);
                        }
                        else if (request.CompleteName != "" && request.Phone != "")
                        {
                            var newcustomer = new EN_Customer();
                            newcustomer.CustomerId = "NewCustomer" + DateTime.Now;
                            newcustomer.CompleteName = request.CompleteName;
                            newcustomer.Phone = request.Phone;
                            newcustomer.Email = request.Email;
                            newcustomer.Status = true;
                            if (file != null)
                                newcustomer.ImageProfile = HostAddress + GenAbsolutePath(relativePath);
                            else
                                newcustomer.ImageProfile = "";
                            _datacontext.EN_Customer.Add(newcustomer);
                        }

                    }
                    await _datacontext.SaveChangesAsync();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                result.Success = false;
                _logService.InsertLog(ex);
            }
            return result;
        }
        public string GenAbsolutePath(string relativePath)
        {
            var systemSettings = _settingService.LoadSetting<SystemSettings>();
            var path = systemSettings.Domain + relativePath.Replace("\\", "/");
            path = path.Replace("//", "/");
            return path;
        }


        public async Task<BaseResponse<bool>> CreateOrderCustomer(EN_CustomerRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var checkPayment = await _datacontext.EN_Paymentonlines.Include(x => x.CategoryPayment).Where(x => x.userId.Equals(request.UserId)).ToListAsync();
                var VNPaySetting = checkPayment.FirstOrDefault(x => x.CategoryPayment.name.Equals("VNPay"));
                var MomoSetting = checkPayment.FirstOrDefault(x => x.CategoryPayment.name.Equals("MOMO"));
                if (request.Payment != "PaymentOnDelivery")
                {
                    if (VNPaySetting == null || MomoSetting != null)
                    {
                        result.Success = false;
                        result.Message = "Not_Payment";
                        return result;
                    }
                }
                if (request.CustomerId == null)
                {
                    result.Success = false;
                    result.Message = "No_order";
                    return result;
                }

                if (request.OrderLine == null)
                {
                    result.Success = false;
                    result.Message = "No_order";
                    return result;
                }
                
                foreach( var item in request.OrderLine)
                {
                    var checkQty = await _checkQuantity(item.FoodListId);
                    if (!checkQty)
                    {
                        result.Success = false;
                        result.CustomData = new object[]
                        {
                             3
                        };
                        result.Message = "Sản phẩm " + item.FoodName + " hiện đã hết số lượng ... Xin lỗi vì sự bất tiện này!";
                        return result;
                    }
                    // Sô lượng hiện còn đủ cung ứng
                    var Quantityremaining = await _checkQuantitySupplied(item.FoodListId);
                    if (Quantityremaining == 0)
                    {
                        result.Success = false;
                        result.CustomData = new object[]
                        {
                            1
                        };
                        result.Message = item.FoodName +" hiện đã hết số lượng ... Xin lỗi vì sự bất tiện này!";
                        return result;
                    }
                    else if(item.qty > Quantityremaining)
                    {
                        result.Success = false;
                        result.CustomData = new object[]
                        {
                             2
                        };
                        result.Message = "Số lượng "+ item.FoodName +" chỉ còn "+ Quantityremaining.ToString();
                        return result;
                    }
                }
                   
                string url = "";
                string OrderHeaderId = "EattingNowOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //Thanh toán khi nhận
                if (request.Payment == "PaymentOnDelivery")
                {

                    var createOrderHeader = new EN_OrderHeader()
                    {
                        OrderHeaderId = OrderHeaderId,
                        CreationDate = DateTime.Now,
                        CustomerId = request.CustomerId,
                        TotalAmt = request.TotalAmt,
                        TransportFee = request.TransportFee,
                        IntoMoney = request.IntoMoney,
                        UserId = request.UserId,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        FormatAddress = request.Format_Address,
                        NameAddress = request.Name_Address,
                        RecipientName = request.RecipientName,
                        RecipientPhone = request.RecipientPhone,
                        Status = false,
                        PaymentStatusID =   7
                    };
                    _datacontext.EN_OrderHeader.Add(createOrderHeader);

                    foreach (var item in request.OrderLine.ToList())
                    {
                        var foodUpdate = await  _datacontext.EN_FoodList.FirstOrDefaultAsync(x => x.FoodListId.Equals(item.FoodListId));
                        var createOrderLine = new EN_OrderLine()
                        {
                            OrderHeaderId = OrderHeaderId,
                            FoodListId = item.FoodListId,
                            CategoryId = item.CategoryId,
                            FoodName = item.FoodName,
                            Price = item.Price,
                            qty = item.qty,
                            UploadImage = item.UploadImage,
                            Description = item.Description,
                        };
                        // Nếu có kiểm soát về số lượng thì trừ 
                        if(foodUpdate.Qtycontrolled == true) foodUpdate.qty = foodUpdate.qty - item.qty;
                        await _datacontext.SaveChangesAsync();
                        _datacontext.EN_OrderLine.Add(createOrderLine);
                    }
                    result.Message = "";

                }
                //Thanh toán với VNPAY
                else if(request.Payment == "VNPay"){
                    var createOrderHeader = new EN_OrderHeader()
                    {
                        OrderHeaderId = OrderHeaderId,
                        CreationDate = DateTime.Now,
                        CustomerId = request.CustomerId,
                        TotalAmt = request.TotalAmt,
                        TransportFee = request.TransportFee,
                        IntoMoney = request.IntoMoney,
                        UserId = request.UserId,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        FormatAddress = request.Format_Address,
                        NameAddress = request.Name_Address,
                        RecipientName = request.RecipientName,
                        RecipientPhone = request.RecipientPhone,
                        Status = false,
                        PaymentStatusID = 5,
                    };
                    _datacontext.EN_OrderHeader.Add(createOrderHeader);

                    foreach (var item in request.OrderLine.ToList())
                    {
                        var createOrderLine = new EN_OrderLine()
                        {
                            OrderHeaderId = OrderHeaderId,
                            FoodListId = item.FoodListId,
                            CategoryId = item.CategoryId,
                            FoodName = item.FoodName,
                            Price = item.Price,
                            qty = item.qty,
                            UploadImage = item.UploadImage,
                            Description = item.Description,
                        };
                        _datacontext.EN_OrderLine.Add(createOrderLine);
                    }
                    await _datacontext.SaveChangesAsync();
                    string urlvnp = ConfigurationManager.AppSettings["Url"];
                    string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
                    //   string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
                    //  string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

                    string tmnCode = VNPaySetting.TmnCode;
                    string hashSecret = VNPaySetting.HashSecret;
                    PayLib pay = new PayLib();
                    pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
                    pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
                    pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
                    pay.AddRequestData("vnp_Amount", (request.IntoMoney * 100).ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
                    pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
                    pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
                    pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
                    pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
                    pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
                    pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang "+ OrderHeaderId); //Thông tin mô tả nội dung thanh toán
                    pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
                    pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
                    pay.AddRequestData("vnp_TxnRef", OrderHeaderId); //mã hóa đơn

                    string paymentUrl = pay.CreateRequestUrl(urlvnp, hashSecret);
                    result.Message = paymentUrl != "/*" ? paymentUrl : "/*";
                    result.Success = true;
                    return result;

                }
                else
                {
                    var createOrderHeader = new EN_OrderHeader()
                    {
                        OrderHeaderId = OrderHeaderId,
                        CreationDate = DateTime.Now,
                        CustomerId = request.CustomerId,
                        TotalAmt = request.TotalAmt,
                        TransportFee = request.TransportFee,
                        IntoMoney = request.IntoMoney,
                        UserId = request.UserId,
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        FormatAddress = request.Format_Address,
                        NameAddress = request.Name_Address,
                        RecipientName = request.RecipientName,
                        RecipientPhone = request.RecipientPhone,
                        Status = false,
                        PaymentStatusID = 5                    };
                    _datacontext.EN_OrderHeader.Add(createOrderHeader);

                    foreach (var item in request.OrderLine.ToList())
                    {
                        var createOrderLine = new EN_OrderLine()
                        {
                            OrderHeaderId = OrderHeaderId,
                            FoodListId = item.FoodListId,
                            CategoryId = item.CategoryId,
                            FoodName = item.FoodName,
                            Price = item.Price,
                            qty = item.qty,
                            UploadImage = item.UploadImage,
                            Description = item.Description,
                        };
                        _datacontext.EN_OrderLine.Add(createOrderLine);
                    }

                    string finaltotal = request.IntoMoney.ToString();
                    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                    string partnerCode = "MOMOOJOI20210710";
                    string accessKey = "iPXneGmrJH0G8FOP";
                    string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
                    string orderInfo = "Đơn hàng của  " + request.CompleteName;
                    string returnUrl = "https://localhost:3000";
                    string notifyurl = "https://3d0f-2001-ee0-537b-1970-291a-94d2-a413-6c84.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

                    string amount = finaltotal;
                    string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
                    string requestId = DateTime.Now.Ticks.ToString();
                    string extraData = "";

                    //Before sign HMAC SHA256 signature
                    string rawHash = "partnerCode=" +
                        partnerCode + "&accessKey=" +
                        accessKey + "&requestId=" +
                        requestId + "&amount=" +
                        amount + "&orderId=" +
                        orderid + "&orderInfo=" +
                        orderInfo + "&returnUrl=" +
                        returnUrl + "&notifyUrl=" +
                        notifyurl + "&extraData=" +
                        extraData;

                    MoMoSecurity crypto = new MoMoSecurity();
                    //sign signature SHA256
                    string signature = crypto.signSHA256(rawHash, serectkey);

                    //build body json request
                    JObject message = new JObject
                                {
                                { "partnerCode", partnerCode },
                                { "accessKey", accessKey },
                                { "requestId", requestId },
                                { "amount", amount },
                                { "orderId", orderid },
                                { "orderInfo", orderInfo },
                                { "returnUrl", returnUrl },
                                { "notifyUrl", notifyurl },
                                { "extraData", extraData },
                                { "requestType", "captureMoMoWallet" },
                                { "signature", signature }
                              };
                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                    JObject jmessage = JObject.Parse(responseFromMomo);

                    url = jmessage.GetValue("payUrl").ToString();
                    result.Message = url != "/*" ? url : "/*";
                    result.Success = true;
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

        //Kiểm soát khả năng cung ứng món ăn khi khách đặt
        private async Task<int> _checkQuantitySupplied(int FoodId)
        {
            var food = await _datacontext.EN_FoodList.FirstOrDefaultAsync(x => x.FoodListId.Equals(FoodId));
            if (food == null) return 0;
            if (food.QtySuppliedcontrolled)
            {
                var quantitySupplied = food.QuantitySupplied;
                var listFoodSaled = await _datacontext.EN_OrderLine.Include(x => x.EN_OrderHeader).Where(x => x.EN_OrderHeader.CreationDate < DateTime.Now && x.FoodListId.Equals(FoodId)).ToListAsync();
                int? SumQuantitySaled = 0;
                foreach (var item in listFoodSaled)
                {
                    SumQuantitySaled += item.qty;
                }
                if (food.QuantitySupplied > SumQuantitySaled)
                    return (int)(quantitySupplied - SumQuantitySaled);
                else
                    return 0;
            }
            else
            {
                return 9999;
            }


        }

        //Kiểm soát số lượng tồn của món ăn khi khách đặt
        private async Task<bool> _checkQuantity(int FoodId)
        {
            var food = await _datacontext.EN_FoodList.FirstOrDefaultAsync(x => x.FoodListId.Equals(FoodId));
            if(food == null) return false; 
            if(food.Qtycontrolled == true)
            {
                int? quantity = food.qty;
                return quantity > 0 ? true : false;
            }
            else
            {
                return true;
            }

        }

        public class MoMoSecurity
        {
            private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            public MoMoSecurity()
            {
                //encrypt and decrypt password using secure
            }
            public string getHash(string partnerCode, string merchantRefId,
                string amount, string paymentCode, string storeId, string storeName, string publicKeyXML)
            {
                string json = "{\"partnerCode\":\"" +
                    partnerCode + "\",\"partnerRefId\":\"" +
                    merchantRefId + "\",\"amount\":" +
                    amount + ",\"paymentCode\":\"" +
                    paymentCode + "\",\"storeId\":\"" +
                    storeId + "\",\"storeName\":\"" +
                    storeName + "\"}";

                byte[] data = Encoding.UTF8.GetBytes(json);
                string result = null;
                using (var rsa = new RSACryptoServiceProvider(4096)) //KeySize
                {
                    try
                    {
                        // MoMo's public key has format PEM.
                        // You must convert it to XML format. Recommend tool: https://superdry.apphb.com/tools/online-rsa-key-converter
                        rsa.FromXmlString(publicKeyXML);
                        var encryptedData = rsa.Encrypt(data, false);
                        var base64Encrypted = Convert.ToBase64String(encryptedData);
                        result = base64Encrypted;
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }

                }

                return result;

            }
            public string buildQueryHash(string partnerCode, string merchantRefId,
                string requestid, string publicKey)
            {
                string json = "{\"partnerCode\":\"" +
                    partnerCode + "\",\"partnerRefId\":\"" +
                    merchantRefId + "\",\"requestId\":\"" +
                    requestid + "\"}";

                byte[] data = Encoding.UTF8.GetBytes(json);
                string result = null;
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    try
                    {
                        // client encrypting data with public key issued by server
                        rsa.FromXmlString(publicKey);
                        var encryptedData = rsa.Encrypt(data, false);
                        var base64Encrypted = Convert.ToBase64String(encryptedData);
                        result = base64Encrypted;
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }

                }

                return result;

            }

            public string buildRefundHash(string partnerCode, string merchantRefId,
                string momoTranId, long amount, string description, string publicKey)
            {
                string json = "{\"partnerCode\":\"" +
                    partnerCode + "\",\"partnerRefId\":\"" +
                    merchantRefId + "\",\"momoTransId\":\"" +
                    momoTranId + "\",\"amount\":" +
                    amount + ",\"description\":\"" +
                    description + "\"}";

                byte[] data = Encoding.UTF8.GetBytes(json);
                string result = null;
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    try
                    {
                        // client encrypting data with public key issued by server
                        rsa.FromXmlString(publicKey);
                        var encryptedData = rsa.Encrypt(data, false);
                        var base64Encrypted = Convert.ToBase64String(encryptedData);
                        result = base64Encrypted;
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }

                }

                return result;

            }
            public string signSHA256(string message, string key)
            {
                byte[] keyByte = Encoding.UTF8.GetBytes(key);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                using (var hmacsha256 = new HMACSHA256(keyByte))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    string hex = BitConverter.ToString(hashmessage);
                    hex = hex.Replace("-", "").ToLower();
                    return hex;

                }
            }
        }

        public class PaymentRequest
        {
            public PaymentRequest()
            {
            }
            public static string sendPaymentRequest(string endpoint, string postJsonString)
            {
                try
                {
                    HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);
                    var postData = postJsonString;
                    var data = Encoding.UTF8.GetBytes(postData);
                    httpWReq.ProtocolVersion = HttpVersion.Version11;
                    httpWReq.Method = "POST";
                    httpWReq.ContentType = "application/json";
                    httpWReq.ContentLength = data.Length;
                    httpWReq.ReadWriteTimeout = 30000;
                    httpWReq.Timeout = 15000;
                    Stream stream = httpWReq.GetRequestStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                    string jsonresponse = "";
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string temp = null;
                        while ((temp = reader.ReadLine()) != null)
                        {
                            jsonresponse += temp;
                        }
                    }
                    return jsonresponse;
                }
                catch (WebException e)
                {
                    return e.Message;
                }
            }
        }
        private string LocalMapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }

        public async Task<BaseResponse<EN_CustomerResponse>> CheckCustomerEmail(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerResponse> { };
            try
            {
                var query = _datacontext.EN_Customer.Where(x => x.Email == request.Email).AsQueryable();
                query = query.OrderBy(d => d.CustomerId);
                result.DataCount = await query.CountAsync();
                if (result.DataCount > 0)
                {
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<EN_CustomerResponse>();
                    result.Success = true;
                }
                else
                {
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

        /// <summary>
        /// Lấy dữ liệu tát cả các khách hàng trên hệ thống
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<EN_CustomerResponse>> TakeAllCustomer(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerResponse> { };
            try
            {
                if (request.Term == "")
                {
                    var query = _datacontext.EN_Customer.OrderBy(x => x.CustomerId).AsQueryable();
                    result.DataCount = await query.CountAsync();
                    if (request.PageSize != 0)
                    {
                        query = query.OrderBy(d => d.CustomerId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    }
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<EN_CustomerResponse>();
                }
                else
                {
                    var query = _datacontext.EN_Customer
                        .Where(x => x.CompleteName.Contains(request.Term)
                                || x.Phone.Contains(request.Term)
                                || x.Email.Contains(request.Term)
                    ).OrderBy(x => x.CustomerId).AsQueryable();
                    result.DataCount = await query.CountAsync();
                    if (request.PageSize != 0)
                    {
                        query = query.OrderBy(d => d.CustomerId).Skip(request.Page * request.PageSize).Take(request.PageSize);
                    }
                    var data = await query.ToListAsync();
                    result.Data = data.MapTo<EN_CustomerResponse>();
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
        public async Task<BaseResponse<EN_CustomerResponse>> TakeAllCustomerByProvinceId(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerResponse> { };
            try
            {

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateCustomerAddress(EN_CustomerAddressRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                // Kiểm tra xem khach hàng đã đăng ký chưa
                var checkCustomer = await _datacontext.EN_Customer.Where(x => x.CustomerId == request.CustomerId).FirstOrDefaultAsync();
                if (checkCustomer == null)
                {
                    var checkPhoneCustomer = await _datacontext.EN_Customer.Where(x => x.Phone == request.PhoneCustomer).FirstOrDefaultAsync();
                    if (checkPhoneCustomer == null)
                    {
                        var createCustomer = new EN_Customer()
                        {
                            CustomerId = request.CustomerId,
                            CompleteName = request.CustomerName,
                            Phone = request.PhoneCustomer,
                            Status = true,
                        };
                        result.Message = "Add Customer ";
                        _datacontext.EN_Customer.Add(createCustomer);
                    }
                    else
                    {
                        request.CustomerId = checkPhoneCustomer.CustomerId;
                        // checkPhoneCustomer.CustomerId = request.CustomerId;
                        checkPhoneCustomer.CompleteName = request.CustomerName;
                        checkPhoneCustomer.Phone = request.PhoneCustomer;
                        checkPhoneCustomer.Status = true;
                        result.Message = "Update Customer ";
                    }
                }
                if (request.AddressId == 0)
                {
                    if (request.Defaut)
                    {
                        // Đặt tất cả các bản ghi khác có Defaut là false và CustomerId là request.CustomerId
                        var otherAddresses = _datacontext.EN_CustomerAddress
                            .Where(x => x.CustomerId.Equals(request.CustomerId) && x.Defaut == true)
                            .ToList();

                        foreach (var address in otherAddresses)
                        {
                            address.Defaut = false;
                        }
                    }
                    var newCustomerAddress = new EN_CustomerAddress();
                    newCustomerAddress.CustomerId = request.CustomerId;
                    newCustomerAddress.Name_Address = request.Name_Address;
                    newCustomerAddress.Format_Address = request.Format_Address;
                    newCustomerAddress.Longitude = request.Longitude;
                    newCustomerAddress.Latitude = request.Latitude;
                    newCustomerAddress.CustomerName = request.CustomerName;
                    newCustomerAddress.PhoneCustomer = request.PhoneCustomer;
                    newCustomerAddress.ProvinceId = request.ProvinceId;
                    newCustomerAddress.DistrictId = request.DistrictId;
                    newCustomerAddress.WardId = request.WardId;
                    newCustomerAddress.Defaut = request.Defaut;
                    _datacontext.EN_CustomerAddress.Add(newCustomerAddress);
                    result.Message = " and Address Success";

                }
                else
                {
                    var UpdateAddressCustomer = _datacontext.EN_CustomerAddress.Where(x => x.AddressId.Equals(request.AddressId) && x.CustomerId.Equals(request.CustomerId)).FirstOrDefault();
                    if (UpdateAddressCustomer == null)
                    {
                        result.Message = "CustomerAddressNotFound";
                        result.Success = false;
                    }
                    else if (request.Defaut)// Nếu tồn tại và Mặc định là đúng 
                    {
                        // Đặt tất cả các bản ghi khác có Defaut là true và CustomerId là request.CustomerId
                        var otherAddresses = _datacontext.EN_CustomerAddress
                            .Where(x => x.CustomerId.Equals(request.CustomerId) && x.Defaut == true)
                            .ToList();
                        // Cập nhật lại tất cả địa chỉ khác là False
                        foreach (var address in otherAddresses)
                        {
                            address.Defaut = false;
                        }
                        UpdateAddressCustomer.Name_Address = request.Name_Address;
                        UpdateAddressCustomer.Format_Address = request.Format_Address;
                        UpdateAddressCustomer.Longitude = request.Longitude;
                        UpdateAddressCustomer.Latitude = request.Latitude;
                        UpdateAddressCustomer.CustomerName = request.CustomerName;
                        UpdateAddressCustomer.PhoneCustomer = request.PhoneCustomer;
                        UpdateAddressCustomer.ProvinceId = request.ProvinceId;
                        UpdateAddressCustomer.DistrictId = request.DistrictId;
                        UpdateAddressCustomer.WardId = request.WardId;
                        UpdateAddressCustomer.Defaut = request.Defaut;
                        result.Message = " and Update Success";
                    }
                    else
                    {
                        UpdateAddressCustomer.Name_Address = request.Name_Address;
                        UpdateAddressCustomer.Format_Address = request.Format_Address;
                        UpdateAddressCustomer.Longitude = request.Longitude;
                        UpdateAddressCustomer.Latitude = request.Latitude;
                        UpdateAddressCustomer.CustomerName = request.CustomerName;
                        UpdateAddressCustomer.PhoneCustomer = request.PhoneCustomer;
                        UpdateAddressCustomer.ProvinceId = request.ProvinceId;
                        UpdateAddressCustomer.DistrictId = request.DistrictId;
                        UpdateAddressCustomer.WardId = request.WardId;
                        UpdateAddressCustomer.Defaut = request.Defaut;
                        result.Message = " and Update Success";
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

        public async Task<BaseResponse<bool>> DeleteAddress(EN_CustomerAddressRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var address = await _datacontext.EN_CustomerAddress.FindAsync(request.AddressId);
                if (address != null)
                {
                    _datacontext.EN_CustomerAddress.Remove(address);
                    result.Success = true;
                    result.Message = "Delete Success";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Not Found";
                }
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<EN_CustomerAddressResponse>> TakeAllCustomerAddressById(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerAddressResponse> { };
            try
            {

                var query = _datacontext.EN_CustomerAddress.Where(x => x.CustomerId == request.CustomerId).OrderBy(x => !x.Defaut).AsQueryable();
                result.DataCount = query.Count();
                result.Success = true;
                var data = await query.ToListAsync();
                if (data.Count > 0)
                {
                    result.Data = data.MapTo<EN_CustomerAddressResponse>();
                    result.Success = true;
                }
                else
                {
                    string phone = request.Phone;
                    string phoneoutput = phone.Replace("84", "");
                    var queryphone = _datacontext.EN_Customer.Where(x => x.Phone.Contains(phoneoutput)).FirstOrDefault();
                    var addresses = _datacontext.EN_CustomerAddress.Where(x => x.CustomerId == queryphone.CustomerId && x.Defaut == true).AsQueryable();
                    addresses = addresses.OrderBy(d => d.CustomerId);
                    var data1 = await addresses.ToListAsync();
                    result.Data = data1.MapTo<EN_CustomerAddressResponse>();
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

        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderByCustomer(EN_CustomerRequest request)
        {
            var result = new BaseResponse<OrderHeaderResponse> { };
            try
            {

                var query = _datacontext.EN_OrderHeader.AsQueryable();//.Where(x => x.CustomerId == request.CustomerId).OrderByDescending(x => x.CreationDate).AsQueryable();

                if (request.Status != null && request.OrderType != 0)
                {
                    query = query.Where(x => x.CustomerId == request.CustomerId && x.Status == request.Status).OrderByDescending(x => x.CreationDate);
                }
                else
                {
                    query = query.Where(x => x.CustomerId == request.CustomerId).OrderByDescending(x => x.CreationDate);

                }
                result.DataCount = query.Count();
                result.Success = true;
                var data = await query.ToListAsync();
                if (data.Count > 0)
                {
                    result.Data = data.MapTo<OrderHeaderResponse>();
                    result.Success = true;
                    result.Message = "Success";
                }
                else
                {
                    result.Data = data.MapTo<OrderHeaderResponse>();
                    result.Success = true;
                    result.Message = "NoOrder";
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> RemoveOrderLine(OrderLineRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var orderLine = await _datacontext.EN_OrderLine.FindAsync(request.OrderLineId);
                if (orderLine != null)
                {
                    _datacontext.EN_OrderLine.Remove(orderLine);
                    result.Success = true;
                    result.Message = "Delete Success";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Not Found";
                }
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;

        }

        public async Task<BaseResponse<bool>> RemoveOrderHeader(OrderLineRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var orderLine = await _datacontext.EN_OrderHeader.FindAsync(request.OrderHeaderId);
                if (orderLine != null)
                {
                    _datacontext.EN_OrderHeader.Remove(orderLine);
                    result.Success = true;
                    result.Message = "Delete Success";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Not Found";
                }
                await _datacontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<EN_CustomerNotificationResponse>> GetAllNotificationCustomer(EN_CustomerNotificationRequest request)
        {
            var result = new BaseResponse<EN_CustomerNotificationResponse> { };
            try
            {
                var query = _datacontext.EN_CustomerNotifications.AsQueryable();
                result.DataCount = await query.Where(x => !x.IsRead && x.CustomerID.Equals(request.CustomerID)).CountAsync();
                if (request.CustomerID == null)
                {
                    result.Success = false;
                    result.Message = "CustomerNotFound";
                    return result;

                }
                if (request.CustomerID != null)
                {
                    query = query.Where(x => x.CustomerID.Equals(request.CustomerID));
                    query = query.OrderByDescending(x => x.NotificationDate);
                }
                if (request.DefautLineNoifi)
                {
                    query = query.Take(8);

                }
                var data = await query.ToListAsync();
                result.Data = data.MapTo<EN_CustomerNotificationResponse>();
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> CreateNotificationCustomer(EN_CustomerNotificationRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var checkCustomer = await _datacontext.EN_Customer.Where(x => x.CustomerId == request.CustomerID).FirstOrDefaultAsync();

                if (checkCustomer != null)
                {
                    var newnotification = new EN_CustomerNotifications{
                        CustomerID = request.CustomerID,
                        NotificationDate = DateTime.Now,
                        SenderName = request.SenderName,
                        Message = request.Message,
                        IsRead = false,
                        Action_Link = request.Action_Link
                    };
                    _datacontext.EN_CustomerNotifications.Add(newnotification);
                    await _datacontext.SaveChangesAsync();
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Customer Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<bool>> DeleteAllNotification(EN_CustomerNotificationRequest request)
        {
            var result = new BaseResponse<bool>();

            try
            {
                // Kiểm tra xem khách hàng có tồn tại không
                var checkCustomer = await _datacontext.EN_Customer
                    .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerID);

                if (checkCustomer == null)
                {
                    result.Success = false;
                    result.Message = "Customer Not Found";
                    return result;
                }

                // Lấy danh sách thông báo của khách hàng dựa trên CustomerID
                var notifications = _datacontext.EN_CustomerNotifications
                    .Where(n => n.CustomerID == request.CustomerID)
                    .ToList();

                // Xóa các thông báo
                _datacontext.EN_CustomerNotifications.RemoveRange(notifications);

                // Sử dụng tính năng bất đồng bộ để lưu thay đổi vào cơ sở dữ liệu
                await _datacontext.SaveChangesAsync();

                result.Message = "Delete All Notification Success";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }

            return result;
        }

        public async Task<BaseResponse<bool>> SetIsReadAllNotification(EN_CustomerNotificationRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                // Kiểm tra xem khách hàng có tồn tại không
                var checkCustomer = await _datacontext.EN_Customer
                    .FirstOrDefaultAsync(x => x.CustomerId == request.CustomerID);

                if (checkCustomer == null)
                {
                    result.Success = false;
                    result.Message = "Customer Not Found";
                    return result;
                }

                // Lấy danh sách thông báo của khách hàng dựa trên CustomerID
                var notifications = _datacontext.EN_CustomerNotifications
                    .Where(n => n.CustomerID == request.CustomerID)
                    .ToList();

                // Đánh dấu tất cả thông báo là đã đọc
                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                }

                // Sử dụng tính năng bất đồng bộ để lưu thay đổi vào cơ sở dữ liệu
                await _datacontext.SaveChangesAsync();

                result.Message = "Marked All Notifications as Read";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }

            return result;
        }

        public async Task<BaseResponse<bool>> PaymentConfirm(PaymentConfirmRequest request)
        {
            var result = new BaseResponse<bool>();
            try
            {
                string vnp_SecureHash = "";
                if (request != null)
                {
                    string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                    var vnpayData = request.ToDictionary();
                    PayLib pay = new PayLib();

                    // lấy toàn bộ dữ liệu được trả về
                    foreach (var kvp in vnpayData)
                    {
                        string key = kvp.Key;
                        string value = kvp.Value;

                        if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                        {
                            if (key == "vnp_SecureHash")
                                vnp_SecureHash = value;
                            pay.AddResponseData(key, value);
                        }
                    }


                    string orderId = request.Vnp_TxnRef; //mã hóa đơn
                    string vnpayTranId = request.Vnp_TransactionNo; //mã giao dịch tại hệ thống VNPAY
                    string vnp_ResponseCode = request.Vnp_ResponseCode; //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                     
                    bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00")
                        {
                            var order = await _datacontext.EN_OrderHeader.FirstOrDefaultAsync(x => x.OrderHeaderId.Equals(orderId));
                            order.PaymentStatusID = 2;
                            await _datacontext.SaveChangesAsync();
                            //Thanh toán thành công
                            result.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                            result.Success = true;
                        }
                        else
                        {
                            var order = await _datacontext.EN_OrderHeader.FirstOrDefaultAsync(x => x.OrderHeaderId.Equals(orderId));
                            var listorderline = await _datacontext.EN_OrderLine.Where(x => x.OrderHeaderId.Equals(orderId)).ToListAsync();
                            _datacontext.EN_OrderLine.RemoveRange(listorderline);
                            await _datacontext.SaveChangesAsync();
                            _datacontext.EN_OrderHeader.Remove(order);
                            await _datacontext.SaveChangesAsync();

                            //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                            result.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                            result.Success = false;

                        }
                    }
                    else
                    {
                        result.Message = "Có lỗi xảy ra trong quá trình xử lý";
                        result.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }

            return result;
        }
    }
}
