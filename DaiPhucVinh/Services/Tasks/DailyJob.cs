using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Infrastructure;
using DaiPhucVinh.Services.MainServices.Attendances;
using DaiPhucVinh.Services.MainServices.Product;
using Falcon.Core.Infrastructure;
using Falcon.Web.Core.Log;
using FluentScheduler;
using SimpleInjector.Lifestyles;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.Tasks
{
    public class DailyJob : IJob
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        private bool _shuttingDown;
        public void Execute()
        {
            if (_shuttingDown)
                return;
            Task.Run(async () =>
            {
                try
                {
                    // kiểm tra chấm công hằng ngày
                    using (AsyncScopedLifestyle.BeginScope(SimpleContainer.Container))
                    {
                        var serviceLog = SimpleContainer.Container.GetInstance<ILogger>();
                        if (serviceLog != null)
                            serviceLog.Info("Test job " + DateTime.Now.ToString("HH:mm:ss"));
                        var service = SimpleContainer.Container.GetInstance<IAttendancesService>();
                        if (service != null)
                            await service.DailyCheck();
                    }
                }
                catch (Exception e)
                {
                    //Logger.Error(e);
                }
            }).Wait();
            GC.Collect();
        }
        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            SemaphoreSlim.Wait();
            _shuttingDown = true;
            SemaphoreSlim.Release();
        }
    }
}
