using Falcon.Web.Core.Settings;

namespace DaiPhucVinh.Services.Settings
{
    public class MetadataSettings : ISettings
    {
        public string SecretKey { get; set; }
        public string HashChat { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Photomaxwidth { get; set; }
        public string CdnServer { get; set; }
    }
}
