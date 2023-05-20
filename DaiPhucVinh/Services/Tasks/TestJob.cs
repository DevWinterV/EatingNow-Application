using DaiPhucVinh.Services.Infrastructure;
using Falcon.Web.Core.Log;
using FluentScheduler;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DaiPhucVinh.Services.Tasks
{
    public class TestJob : IJob
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
                    using (AsyncScopedLifestyle.BeginScope(SimpleContainer.Container))
                    {
                        var service = SimpleContainer.Container.GetInstance<ILogger>();
                        if (service != null)
                            service.Info("Test job " + DateTime.Now.ToString("HH:mm:ss"));

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
