using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.Chart;
using DaiPhucVinh.Shared.Common;
using DaiPhucVinh.Shared.Store;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace DaiPhucVinh.Services.MainServices.Chart_DashBoarb
{
    public interface IChartService
    {
        Task<BaseResponse<RevenueResponse>> TotalRevenue_Chart(ChartRequest request);
        Task<BaseResponse<PriceQuoteResponse>> TotalPriceQuote(ChartRequest request);
        Task<BaseResponse<RevenuestatisticsResponse>> TotalRevenueStatistics(StatisticRequest request);
        Task<BaseResponse<List<ItemFoodResponse>>> TakeProductStatistics(StatisticRequest request);
    }
    public class ChartService : IChartService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;
        public ChartService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }

        public async Task<BaseResponse<RevenueResponse>> TotalRevenue_Chart(ChartRequest request)
        {
            var result = new BaseResponse<RevenueResponse> { };
            try
            {
                var query = _datacontext.WMS_HopDongs.AsQueryable();
                if (request.FromDt.HasValue)
                {
                    query = query.Where(d => d.NgayKy > request.FromDt);
                }
                if (request.ToDt.HasValue)
                {
                    query = query.Where(d => d.NgayKy <= request.ToDt);
                }
                var data = await query.OrderBy(d => d.NgayKy).ToListAsync();
                result.Data = data.MapTo<RevenueResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<PriceQuoteResponse>> TotalPriceQuote (ChartRequest request)
        {
            var result = new BaseResponse<PriceQuoteResponse> { };
            try
            {
                var query = _datacontext.WMS_Quotations.AsQueryable();
                if (request.FromDt.HasValue)
                {
                    query = query.Where(d => d.Date > request.FromDt);
                }
                if (request.ToDt.HasValue)
                {
                    query = query.Where(d => d.Date <= request.ToDt);
                }
                var data = await query.ToListAsync();
                result.Data = data.MapTo<PriceQuoteResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }

        public async Task<BaseResponse<RevenuestatisticsResponse>> TotalRevenueStatistics(StatisticRequest request)
        {
            var response = new BaseResponse<RevenuestatisticsResponse>();
            try
            {
                //DateTime now = DateTime.Now;
                var revenueStatisticsResponse = new RevenuestatisticsResponse();
                var itemStatisticsResponses = new List<ItemStatisticsResponse>();

                // Total number of customers in the system
                revenueStatisticsResponse.TotalCountCustomersystem = await _datacontext.EN_Customer.AsNoTracking().CountAsync();

                var queryableStores = _datacontext.EN_Store.Include(x => x.Province).AsQueryable();
                if (request.IdZone > 0)
                {
                    queryableStores = queryableStores.Where(x => x.ProvinceId == request.IdZone);
                }

                // Total number of stores in the system
                revenueStatisticsResponse.TotalCountStoresystem = await queryableStores.AsNoTracking().CountAsync();

                // Total revenue of the system
                double totalSystemRevenue = await _datacontext.EN_OrderHeader
                    .AsNoTracking()
                    .Where(x => (request.IdZone == 0 || x.EN_Store.ProvinceId == request.IdZone) &&
                                x.CreationDate >= request.FromDt && x.CreationDate <= request.ToDt &&
                                x.ShippingStatus == 3 &&
                                x.Status == true)
                    .SumAsync(x => (double?)x.IntoMoney) ?? 0;


                revenueStatisticsResponse.Totalsystemrevenue = totalSystemRevenue;

                var newquery = from order in _datacontext.EN_OrderHeader
                               join store in _datacontext.EN_Store on order.UserId equals store.UserId
                               join province in _datacontext.EN_Province on store.ProvinceId equals province.ProvinceId
                               where (request.IdZone == 0 || store.ProvinceId == request.IdZone)
                                   && order.Status == true
                                   && order.ShippingStatus == 3
                                   && (order.CreationDate >= request.FromDt && order.CreationDate <= request.ToDt)
                               group order by new { order.UserId, store.FullName, province.Name } into g
                               select new ItemStatisticsResponse
                               {
                                   IdZone = request.IdZone,
                                   NameZone = g.Key.Name,
                                   StoreId = g.Key.UserId,
                                   FullName = g.Key.FullName,
                                   Total = g.Sum(x => x.IntoMoney),
                                   Percentagerevenue = totalSystemRevenue != 0 ? Math.Round((g.Sum(x => x.IntoMoney) / totalSystemRevenue) * 100, 1) : 0
                               };
                revenueStatisticsResponse.ListItemStatisticsResponse = await  newquery.ToListAsync();
                response.Success = true;
                response.Message = "Operation successful";
                response.Item = revenueStatisticsResponse;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                _logService.InsertLog(ex);
            }

            return response;
        }

        public async Task<BaseResponse<List<ItemFoodResponse>>> TakeProductStatistics(StatisticRequest request)
        {
            var response = new BaseResponse<List<ItemFoodResponse>>();
            try
            {
                var newquery = (from odl in _datacontext.EN_OrderLine
                               join od in _datacontext.EN_OrderHeader on odl.OrderHeaderId equals od.OrderHeaderId
                               join store in _datacontext.EN_Store on od.UserId equals store.UserId
                               join province in _datacontext.EN_Province on store.ProvinceId equals province.ProvinceId
                               where (request.IdZone == 0 || store.ProvinceId == request.IdZone)
                                   && od.Status == true  // Da xac nhan va 
                                   && od.ShippingStatus == 3 // Da giao thanh cong
                                   && (od.CreationDate >= request.FromDt && od.CreationDate <= request.ToDt)
                               group odl by new { odl.FoodName, odl.FoodListId, province.Name } into g
                               select new ItemFoodResponse
                               {
                                   FoodListId = g.Key.FoodListId,
                                   FoodName = g.Key.FoodName,
                                   CountBuy = g.Sum(x => x.qty)
                               }).Take(50);
                response.Success = true;
                response.Message = "Operation successful";
                response.Item = await newquery.ToListAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                _logService.InsertLog(ex);
            }

            return response;
        }
    }
}
