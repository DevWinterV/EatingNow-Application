using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
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

namespace DaiPhucVinh.Services.MainServices.EN_CustomerService
{
    public interface IENCustomerService
    {
        Task<BaseResponse<EN_CustomerResponse>> CheckCustomer(EN_CustomerRequest request);
        Task<BaseResponse<bool>> CreateOrderCustomer(EN_CustomerRequest request);
        Task<BaseResponse<bool>> UpdateToken(EN_CustomerRequest request);

    }
    public class ENCustomerService : IENCustomerService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public ENCustomerService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<EN_CustomerResponse>> CheckCustomer(EN_CustomerRequest request)
        {
            var result = new BaseResponse<EN_CustomerResponse> { };
            try
            {
                var query = _datacontext.EN_Customer.Where(x => x.CustomerId == request.CustomerId).AsQueryable();
                query = query.OrderBy(d => d.CustomerId);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<EN_CustomerResponse>();
                result.Success = true;
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
                    _datacontext.SaveChangesAsync();
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

        public async Task<BaseResponse<bool>> CreateOrderCustomer(EN_CustomerRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                string url = "";
                string OrderHeaderId = "EattingNowOrder_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var checkCustomer = await _datacontext.EN_Customer.Where(x => x.CustomerId == request.CustomerId).FirstOrDefaultAsync();
                if (request.Payment == "PaymentOnDelivery")
                {
                    if (checkCustomer == null)
                    {
                        var createCustomer = new EN_Customer()
                        {
                            CustomerId = request.CustomerId,
                            CompleteName = request.CompleteName,
                            ProvinceId = request.ProvinceId,
                            DistrictId = request.DistrictId,
                            WardId = request.WardId,
                            Phone = request.Phone,
                            Address = request.Address,
                            Status = true,
                        };
                        _datacontext.EN_Customer.Add(createCustomer);

                        var createOrderHeader = new EN_OrderHeader()
                        {
                            OrderHeaderId = OrderHeaderId,
                            CreationDate = DateTime.Now,
                            CustomerId = request.CustomerId,
                            TotalAmt = request.TotalAmt,
                            TransportFee = request.TransportFee,
                            IntoMoney = request.IntoMoney,
                            UserId = request.UserId,
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
                        };
                        _datacontext.EN_OrderHeader.Add(createOrderHeader);

                        await _datacontext.SaveChangesAsync();

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
                }
                else
                {
                    if (checkCustomer == null)
                    {
                        var createCustomer = new EN_Customer()
                        {
                            CustomerId = request.CustomerId,
                            CompleteName = request.CompleteName,
                            ProvinceId = request.ProvinceId,
                            DistrictId = request.DistrictId,
                            WardId = request.WardId,
                            Phone = request.Phone,
                            Address = request.Address,
                            Status = true,
                        };
                        _datacontext.EN_Customer.Add(createCustomer);

                        var createOrderHeader = new EN_OrderHeader()
                        {
                            OrderHeaderId = OrderHeaderId,
                            CreationDate = DateTime.Now,
                            CustomerId = request.CustomerId,
                            TotalAmt = request.TotalAmt,
                            TransportFee = request.TransportFee,
                            IntoMoney = request.IntoMoney,
                            UserId = request.UserId,
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
                        };
                        _datacontext.EN_OrderHeader.Add(createOrderHeader);

                        await _datacontext.SaveChangesAsync();

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

                        // su ly 
                        string finaltotal = request.IntoMoney.ToString();
                        string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                        string partnerCode = "MOMOOJOI20210710";
                        string accessKey = "iPXneGmrJH0G8FOP";
                        string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
                        string orderInfo = "Đơn hàng của  " + checkCustomer.CompleteName;
                        string returnUrl = "https://localhost:3000";
                        string notifyurl = "https://3d0f-2001-ee0-537b-1970-291a-94d2-a413-6c84.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

                        string amount = finaltotal;
                        string orderid = OrderHeaderId; //mã đơn hàng
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
                }
                await _datacontext.SaveChangesAsync();
                result.Message = url != "" ? url : "";
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
    }
}
