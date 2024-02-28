using DaiPhucVinh.Server.Data.DaiPhucVinh;
using DaiPhucVinh.Services.Database;
using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Helper;
using DaiPhucVinh.Shared.AccountType;
using DaiPhucVinh.Shared.Common;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.MainServices.Province
{
    public interface IAccountTypeService
    {
        Task<BaseResponse<AccountTypeResponse>> TakeAllAccountType(AccountTypeRequest request);
        Task<BaseResponse<bool>> CreateNewAccountType(AccountTypeRequest request);
        Task<BaseResponse<bool>> UpdateNewAccountType(AccountTypeRequest request);
        Task<BaseResponse<bool>> DeleteAccountType(AccountTypeRequest request);
        Task<BaseResponse<AccountTypeResponse>> SearchAccountType(string nameProvince);
        Task<BaseResponse<AccountTypeResponse>> TakeAccountTypeById(int Id);
    }
    public class AccountTypeService : IAccountTypeService
    {
        private readonly DataContext _datacontext;
        private readonly ILogService _logService;

        public AccountTypeService(DataContext datacontext, ILogService logService)
        {
            _datacontext = datacontext;
            _logService = logService;
        }
        public async Task<BaseResponse<AccountTypeResponse>> TakeAllAccountType(AccountTypeRequest request)
        {
            var result = new BaseResponse<AccountTypeResponse> { };
            try
            {
                var query = _datacontext.EN_AccountType.AsQueryable();
                if (!string.IsNullOrEmpty(request.Term))
                {
                    query = query.Where(x => x.Name.Contains(request.Term));
                }
                query = query.OrderBy(d => d.Name);
                result.DataCount = await query.CountAsync();
                var data = await query.ToListAsync();
                result.Data = data.MapTo<AccountTypeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<AccountTypeResponse>> TakeAccountTypeById(int Id)
        {
            var result = new BaseResponse<AccountTypeResponse> { };
            try
            {
                var data = await _datacontext.EN_AccountType.FindAsync(Id);
                result.Item = data.MapTo<AccountTypeResponse>();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
                _logService.InsertLog(ex);
            }
            return result;
        }
        public async Task<BaseResponse<bool>> CreateNewAccountType(AccountTypeRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id == 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var province = new EN_AccountType
                    {
                        Name = request.Name,
                        Status = request.Status,
                    };
                    _datacontext.EN_AccountType.Add(province);
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

        public async Task<BaseResponse<bool>> UpdateNewAccountType(AccountTypeRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                if (request.Id > 0)
                {
                    if (request.Name == "")
                    {
                        request.Name = null;
                    }
                    var province = await _datacontext.EN_AccountType.FindAsync(request.Id);
                    {
                        province.Name = request.Name;
                    };
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
        public async Task<BaseResponse<bool>> DeleteAccountType(AccountTypeRequest request)
        {
            var result = new BaseResponse<bool> { };
            try
            {
                var province = await _datacontext.EN_AccountType.FirstOrDefaultAsync(x => x.Id == request.Id);
                _datacontext.EN_AccountType.Remove(province);
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
        public async Task<BaseResponse<AccountTypeResponse>> SearchAccountType(string provinceName)
        {
            var result = new BaseResponse<AccountTypeResponse> { };
            try
            {
                var query = await _datacontext.EN_AccountType.SingleOrDefaultAsync(x => x.Name == provinceName);
                result.Item = query.MapTo<AccountTypeResponse>();
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
