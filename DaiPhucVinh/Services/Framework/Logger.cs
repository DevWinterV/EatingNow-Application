using Falcon.Web.Core.Log;
using DaiPhucVinh.Server.Data.Entity;
using DaiPhucVinh.Services.Database;
using System;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.Framework
{
    public class Logger : ILogger
    {
        private void InsertLog(LogLevel level, string shortMessage, string fullMessage, string context)
        {
            //fire and forget
            Task.Run(() =>
            {
                using (var db = new DataContext())
                {
                    db.Logs.Add(new Log
                    {
                        LogLevelId = (int)level,
                        //Context = context,
                        //CreatedAt = DateTime.Now,
                        //IpAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString(),
                        FullMessage = fullMessage,
                        ShortMessage = shortMessage,
                        CreatedOnUtc = DateTime.Now
                    });
                    db.SaveChanges();
                }
            });
        }
        public void Debug(string shortMessage, string fullMessage = "", string context = "")
        {
            InsertLog(LogLevel.Debug, shortMessage, fullMessage, context);
        }

        public void Info(string shortMessage, string fullMessage = "", string context = "")
        {
            InsertLog(LogLevel.Information, shortMessage, fullMessage, context);
        }

        public void Warning(string shortMessage, string fullMessage = "", string context = "")
        {
            InsertLog(LogLevel.Information, shortMessage, fullMessage, context);
        }

        public void Error(string shortMessage, string fullMessage = "", string context = "")
        {
            InsertLog(LogLevel.Error, shortMessage, fullMessage, context);
        }

        public void Fatal(string shortMessage, string fullMessage = "", string context = "")
        {
            InsertLog(LogLevel.Fatal, shortMessage, fullMessage, context);
        }
    }
}
