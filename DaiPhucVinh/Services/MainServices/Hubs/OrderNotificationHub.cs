using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.MainServices.Hubs
{
    [HubName("OrderNotificationHub")]
    public class OrderNotificationHub : Hub
    {
        public async Task NewMessage(string message)
        {
            await Clients.All.SendAsync("messageReceived", message);
        }
    }

}
