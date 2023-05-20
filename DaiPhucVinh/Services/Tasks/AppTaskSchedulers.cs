using DaiPhucVinh.Services.Framework;
using DaiPhucVinh.Services.Infrastructure;
using Falcon.Core.Infrastructure;
using Falcon.Web.Core.Log;
using FluentScheduler;

namespace DaiPhucVinh.Services.Tasks
{
    
    public class AppTaskSchedulers : Registry
    {
        public AppTaskSchedulers()
        {
            //Chạy SimpleJob 02 mỗi giờ
            Schedule<SimpleJob>().ToRunNow().AndEvery(2).Hours();
            //Schedule<DailyJob>().ToRunEvery(0).Days().At(08, 00);

            }
    }
}
