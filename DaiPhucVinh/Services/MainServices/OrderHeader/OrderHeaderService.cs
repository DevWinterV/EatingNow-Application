using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.CustomerDto;
using DaiPhucVinh.Shared.OrderHeaderResponse;
using DaiPhucVinh.Shared.OrderLineReponse;
using DaiPhucVinh.Shared.OrderLineResponse;
using DaiPhucVinh.Shared.Province;
using Microsoft.AspNet.SignalR;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.OrderHeader
{
    public interface IOrderHeaderService
    {
        Task<BaseResponse<OrderHeaderResponse>> CreateOrderHeader(OrderHeaderRequest request);
        Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(int UserId);

    }
    public class OrderHeaderService : IOrderHeaderService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public OrderHeaderService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<OrderHeaderResponse>> CreateOrderHeader(OrderHeaderRequest request)
        {
            var result = new BaseResponse<OrderHeaderResponse> { };
            try
            {
                var orderHeader = new EN_OrderHeader
                {
                    CreationDate = DateTime.Now,
                    CustomerId = request.CustomerId,
                    TotalAmt = request.TotalAmt,
                    TransportFee= request.TransportFee,
                    IntoMoney = request.IntoMoney,
                    UserId = request.UserId,
                };
                _datacontext.EN_OrderHeader.Add(orderHeader);
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

    

        public async Task<BaseResponse<OrderHeaderResponse>> TakeOrderHeaderByStoreId(int UserId)
        {
            var result = new BaseResponse<OrderHeaderResponse> { };
            try
            {
                var query = _datacontext.EN_OrderHeader.Where(x => x.UserId == UserId).AsQueryable();
                query = query.OrderBy(d => d.OrderHeaderId);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<OrderHeaderResponse>();
                result.Success = true;
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
