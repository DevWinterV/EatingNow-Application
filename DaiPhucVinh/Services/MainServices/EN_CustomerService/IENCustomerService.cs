using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Constants;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Services.Settings;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.DeliveryDriver;
using DaiPhucVinh.Shared.OrderHeader;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Falcon.Web.Core.Settings;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.BuilderProperties;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;


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
        Task<BaseResponse<bool>> CreateCustomerAddress(EN_CustomerAddressRequest request);

        Task<BaseResponse<bool>> UpdateToken(EN_CustomerRequest request);
        Task<BaseResponse<EN_CustomerResponse>> CheckCustomerEmail(EN_CustomerRequest request);
        Task<BaseResponse<bool>> UpdateInfoCustomer(EN_CustomerRequest request, HttpPostedFile file);
        Task<BaseResponse<EN_CustomerAddressResponse>> CheckCustomerAddress(EN_CustomerRequest request);
        Task<BaseResponse<bool>> DeleteAddress(EN_CustomerAddressRequest Id);
        Task<BaseResponse<bool>> RemoveOrderLine(OrderLineRequest request);
        Task<BaseResponse<bool>> RemoveOrderHeader(OrderLineRequest request);


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
                    if(request.Phone != "")
                    {
                        string phone = request.Phone;
                        string phoneoutput = phone.Replace("84", "");
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

        public async Task<BaseResponse<EN_CustomerAddressResponse>> CheckCustomerAddress(EN_CustomerRequest  request)
        {
            var result = new BaseResponse<EN_CustomerAddressResponse> { };
            try
            {
                var query = _datacontext.EN_CustomerAddress.Where(x => x.CustomerId == request.CustomerId && x.Defaut == true).AsQueryable();
                query = query.OrderBy(d => d.CustomerId);
                var data = await query.ToListAsync();
                if(data.Count > 0)
                {
                    result.Data = data.MapTo<EN_CustomerAddressResponse>();
                    result.Success = true;
                    result.DataCount = data.Count();
                }
                else if(request.Phone != "")
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
                        if(request.CompleteName != "" && request.Phone != "" && request.CustomerId != "")
                        {
                            var newcustomer = new EN_Customer();
                            newcustomer.CustomerId = request.CustomerId;
                            newcustomer.CompleteName = request.CompleteName;
                            newcustomer.Phone = request.Phone;
                            newcustomer.Email = request.Email;
                            newcustomer.Status =true;
                            if (file != null)
                                newcustomer.ImageProfile = HostAddress + GenAbsolutePath(relativePath);
                            else
                                newcustomer.ImageProfile = "";
                            _datacontext.EN_Customer.Add(newcustomer);
                        }
                        else if(request.CompleteName != "" && request.Phone!="")
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
                if (request.CustomerId == null)
                {
                    result.Success = false;
                    result.Message = "No order!";
                    return result;
                }
                string url = "";
                string OrderHeaderId = "EattingNowOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
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
                        Status = false
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
                        Status = false
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
                }
               
                await _datacontext.SaveChangesAsync();
                result.Message = url != "/*" ? url : "/*";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
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
                if(result.DataCount > 0)
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
                if(request.Term == "")
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
            catch(Exception ex)
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
                if(address != null)
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
                if(data.Count > 0)
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
                
                if(request.Status != null && request.OrderType != 0)
                {
                    query = query.Where(x => x.CustomerId == request.CustomerId && x.Status == request.Status ).OrderByDescending(x => x.CreationDate);
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
    }
    public class NotificationHub : Hub
    {
        public void SendNotification(string message)
        {
            Clients.All.receiveNotification(message);
        }
    }
}
