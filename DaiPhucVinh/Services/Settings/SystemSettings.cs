using Falcon.Web.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.Settings
{
    public class SystemSettings : ISettings
    {
        public int CacheLong { get; set; }
        public int CacheNormal { get; set; }
        public int CacheShort { get; set; }
        public string Domain { get; set; }
        public string IotHubKey { get; set; }
        /// <summary>
        /// Thoi gian toi da luu lai command trong queue, minutes
        /// </summary>
        public int SkipIotCommandTime { get; set; }
        /// <summary>
        /// Thoi gian doi truoc khi retry lai command, minutes
        /// </summary>
        public int WaitBeforeRetryIotCommandTime { get; set; }
        /// <summary>
        /// Thoi gian doi giua 2 lan gui iot command, seconds
        /// </summary>
        public int CycleIotCommandTime { get; set; }
        /// <summary>
        /// Thoi gian doi giua 2 lan gui email, seconds
        /// </summary>
        public int CycleEmailSendingTime { get; set; }
        /// <summary>
        /// Thoi gian doi giua 2 lan gui mobile notification, seconds
        /// </summary>
        public int CycleMobileNotifyTime { get; set; }

        public int CacheSku { get; set; }

    }
}
