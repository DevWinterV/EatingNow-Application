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
        private static List<UserConnection> userConnections = new List<UserConnection>();

        public void SendOrderNotification(string orderMessage)
        {
            try
            {
                // Broadcast the order notification to all connected clients
                Clients.All.ReceiveOrderNotification(orderMessage);
            }
            catch(Exception ex) { 
            
            }

        }
        public void SetCustomerId(string customerId)
        {
            try
            {
                var checkuserConnection = userConnections.FirstOrDefault(x => x.UserId == customerId);
                if (checkuserConnection != null)
                {
                    userConnections.Remove(checkuserConnection);
                }
                // Đăng ký người dùng bằng CustomerId và ConnectionId
                var userConnection = new UserConnection
                {
                    UserId = customerId,
                    ConnectionId = Context.ConnectionId
                };
                userConnections.Add(userConnection);
            }
            catch (Exception ex)
            {

            }

        }
        public void SetUserId(string userId)
        {
            try
            {
                var checkuserConnection = userConnections.FirstOrDefault(x => x.UserId == userId);
                if (checkuserConnection != null)
                {
                    userConnections.Remove(checkuserConnection);
                }
                // Đăng ký người dùng bằng CustomerId và ConnectionId
                var userConnection = new UserConnection
                {
                    UserId = userId,
                    ConnectionId = Context.ConnectionId
                };
                userConnections.Add(userConnection);
            }
            catch (Exception ex)
            {

            }

        }
        public void RemoveUserConnection(string username)
        {
            try
            {
                // Xóa thông tin UserConnection của người dùng khi đăng xuất hoặc thoát ứng dụng
                var userConnection = userConnections.FirstOrDefault(x => x.UserId == username);
                if (userConnection != null)
                {
                    userConnections.Remove(userConnection);
                }
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Gui thông báo tới username đã đăng ký trên SignalR
        /// </summary>
        /// <param name="orderMessage"></param>
        /// <param name="username"></param>
        public void SendOrderNotificationToUser(string orderMessage, string username)
        {

            try
            {
                if (userConnections != null && userConnections.Any())
                {
                    var userConnection = userConnections.Where(x => x.UserId == username).ToList();
                    if (userConnection != null)
                    {
                        foreach (var connection in userConnection)
                        {
                            Clients.Client(connection.ConnectionId).ReceiveOrderNotificationOfUser(orderMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
    public class UserConnection
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}
