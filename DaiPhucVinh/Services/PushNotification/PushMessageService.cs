using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.PushNotification
{
    public interface IPushMessageService
    {
        Task<string> Push(string title, string message, List<string> deviceToken);
        Task<string> PushToChannel(string title, string message);
    }
    public class PushMessageService : IPushMessageService
    {
        //private FcmServiceNET.ExpoService expoService;
        private FcmServiceNET.FcmService fcmService;
        private readonly string applicationId;
        private readonly string senderId;
        private readonly string channel;
        private readonly string expo;
        public PushMessageService()
        {
            //expoService = new FcmServiceNET.ExpoService();

            applicationId = "AAAANRNqefI:APA91bFlQvZ7Iw2wVwaDtwmv_v5s_aLFeeTBExxRQzbTH71XPgeMClLI7OuW_tdQMpWi8gGF_0zpJ5xYVUoIAJjTPgri0elqFMptkNe-dkfP1VRX9USCklb0Ri2HPoifMBONzLgI50u4";
            senderId = "227959011826";
            channel = "fuvico_default";
            expo = "@angiangpro211@gmail.com/fuvico";
            fcmService = new FcmServiceNET.FcmService(applicationId, senderId, channel);
        }
        public async Task<string> Push(string title, string message, List<string> deviceToken)
        {
            return await fcmService.PushFCM(title, message, deviceToken);
        }
        public async Task<string> PushToChannel(string title, string message)
        {
            return await fcmService.PushFCM(title, message, new List<string> { });
        }
    }
}
