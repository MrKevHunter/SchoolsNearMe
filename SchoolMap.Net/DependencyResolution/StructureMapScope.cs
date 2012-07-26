using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace SchoolMap.Net.DependencyResolution
{
    public class StructureMapScope : IDependencyScope
    {
        protected IContainer Container;

        public StructureMapScope(IContainer container)
        {
            Container = container;
        }

        public void Dispose()
        {
            IDisposable disposable = Container;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            Container = null;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                    return Container.TryGetInstance(serviceType);

                return Container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}