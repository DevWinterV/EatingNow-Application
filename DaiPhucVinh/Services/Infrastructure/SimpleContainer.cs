using System;
using System.Diagnostics;
using Falcon.Web.Core.Infrastructure;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DaiPhucVinh.Services.Infrastructure
{
    public class SimpleContainer : IContainer
    {
        public SimpleContainer(Container container)
        {
            Container = container;
        }

        public static Container Container { get; private set; }

        public T Resolve<T>() where T : class
        {
            try
            {
                return Container.GetInstance<T>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return null;
        }
        public T ResolveWithNewScope<T>() where T : class
        {
            using (AsyncScopedLifestyle.BeginScope(Container))
            {
                return Container.GetInstance<T>();
            }
        }
    }
}