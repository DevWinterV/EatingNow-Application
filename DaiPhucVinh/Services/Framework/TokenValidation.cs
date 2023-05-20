using Falcon.Web.Core.Security;
using Falcon.Web.Core.Settings;

namespace DaiPhucVinh.Services.Framework
{
    public class TokenValidation : ITokenValidation
    {
        private readonly ISettingService _settingService;
        public TokenValidation(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public string GetEncryptKey()
        {
            return _settingService.LoadSetting<SecuritySettings>().EncryptionKey;
        }

        public bool IsUnique(int userId, string token)
        {
            //TODO IsUnique token
            return true;
        }
    }
}
