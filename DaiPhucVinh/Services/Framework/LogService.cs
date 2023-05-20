using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using System;
using System.Net;

namespace DaiPhucVinh.Services.Framework
{
    public interface ILogService
    {
        void InsertLog(Exception exception);
    }

    public class LogService : ILogService
    {
        private readonly DataContext _dataContext;

        public LogService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void InsertLog(Exception exception)
        {
            _dataContext.Logs.Add(new Log
            {
                LogLevelId = (int)LogLevel.Error,
                LogLevel = LogLevel.Error,
                IpAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(),
                CreatedOnUtc = DateTime.Now,
                ShortMessage = exception.Message,
                FullMessage = exception.ToString()
            });
            _dataContext.SaveChanges();
        }
        public void InsertWorkLog(string description)
        {
            _dataContext.FUV_WorkLogs.Add(new FUV_WorkLog
            {
                UserId = TokenHelper.CurrentIdentity().UserId,
                Description = description,
                CreatedAt = DateTime.Now,
            });
            _dataContext.SaveChanges();
        }
        //        public async Task<BaseResponse<LogsDto>> SearchAllLog(LogSystemRequest request)
        //        {
        //            var result = new BaseResponse<LogsDto> { Success = false };
        //            try
        //            {
        //                var _posContext = TokenHelper.PosContext();
        //                var query = _posContext.Logs.AsQueryable();
        //                if (request.Content.HasValue())
        //                {
        //                    query = query.Where(x => x.LogLevelId >= (int)LogLevel.Warning
        //                                        && (x.ShortMessage.Contains(request.Content)
        //                                        || x.FullMessage.Contains(request.Content)));
        //                }
        //                if (request.FromDt.HasValue)
        //                {
        //                    DateTime fromDate = new DateTime(request.FromDt.Value.Year, request.FromDt.Value.Month, request.FromDt.Value.Day, 0, 0, 0);
        //                    query = query.Where(cc => cc.CreatedOnUtc >= fromDate);
        //                }
        //                if (request.ToDt.HasValue)
        //                {
        //                    DateTime toDate = new DateTime(request.ToDt.Value.Year, request.ToDt.Value.Month, request.ToDt.Value.Day, 23, 59, 59);
        //                    query = query.Where(cc =>  cc.CreatedOnUtc <= toDate);
        //                }
        //                result.DataCount = await query.CountAsync();
        //                var data = await query.OrderByDescending(x => x.Id).Skip(request.Page * request.PageSize).Take(request.PageSize).ToListAsync();
        //                result.Datas = data.MapTo<LogsDto>();
        //                result.Success = true;
        //                _posContext.Dispose();
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                result.Message = ex.ToString();
        //#else
        //                result.Message = ex.Message;
        //#endif
        //            }
        //            return result;
        //        }
    }
}
