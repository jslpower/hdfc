using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.InterceptionExtension;
namespace EyouSoft.Component.Factory
{
    /// <summary>
    /// 对象创建工厂
    /// </summary>
    public class ComponentFactory
    {
        /// <summary>
        /// 创建数据层对象
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <returns></returns>
        public static TInterface CreateDAL<TInterface>()
        {
            IUnityContainer container = GetConatiner();
            return container.Resolve<TInterface>();
        }

        public static IUnityContainer GetConatiner()
        {
            return (IUnityContainer)System.Web.HttpContext.Current.Application["container"];
        }
    }
}
