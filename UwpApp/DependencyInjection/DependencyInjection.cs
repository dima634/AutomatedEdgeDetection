using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace UwpApp
{
    class DependencyInjection
    {
        private static Lazy<UnityContainer> _unityContainer = new Lazy<UnityContainer>();

        public static UnityContainer UnityContainer => _unityContainer.Value;

        static DependencyInjection()
        {
            new UnityConfiguration().RegisterTypes(UnityContainer);
        }

        public static T Resolve<T>(string name = null)
        {
            return name == null ? _unityContainer.Value.Resolve<T>() : _unityContainer.Value.Resolve<T>(name);
        }
    }
}
