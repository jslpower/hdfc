using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace EyouSoft.Services.BackgroundServices
{
    /// <summary>
    /// 启动服务的对象
    /// </summary>
    public enum BackgroundServicesItem
    {
        /// <summary>
        /// 当前系统
        /// </summary>
        当前系统 = 0,
        /// <summary>
        /// 短信中心
        /// </summary>
        短信中心 = 1
    }

    public class BackgroundServicesExecutor
    {
        private IUnityContainer _container;
        private readonly List<BackgroundServiceExecutor> _executors;

        public BackgroundServicesExecutor(IUnityContainer container, BackgroundServicesItem servicesItem)
        {
            _container = container;

            //TODO: (erikpo) Once we have a plugin framework in place to load types from different assemblies, get rid of the below hardcoded values and load up all background services dynamically
            if (servicesItem == BackgroundServicesItem.当前系统)
            {
                
            }
            else if (servicesItem == BackgroundServicesItem.短信中心)
            {
                _executors = new List<BackgroundServiceExecutor>(2)
                    {
                        new BackgroundServiceExecutor(container, typeof(SmsTimer), 1),
                        new BackgroundServiceExecutor(container, typeof(CaringSmsTimer), 2)
                    };
            }
        }

        public void Start()
        {
            foreach (BackgroundServiceExecutor executor in _executors)
            {
                executor.Start();
            }
        }

        public void Stop()
        {
            foreach (BackgroundServiceExecutor executor in _executors)
            {
                executor.Stop();
            }
        }
    }
}
