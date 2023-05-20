using DaiPhucVinh.Services.Infrastructure;
using DaiPhucVinh.Services.MainServices.Product;
using FluentScheduler;
using SimpleInjector.Lifestyles;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DaiPhucVinh.Services.Tasks
{
    public class SimpleJob : IJob
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
                    // Dong bo hinh anh
                    using (AsyncScopedLifestyle.BeginScope(SimpleContainer.Container))
                    {
                        var service = SimpleContainer.Container.GetInstance<IProductService>();
                        if (service != null)
                            await service.ImagesSync();
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
