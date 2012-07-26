using SchoolMap.Net.Services;
using StructureMap;

namespace SchoolMap.Net.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                            x.For<ILocationService>().Use<HostIpLocationService>();
                        });
            return ObjectFactory.Container;
        }
    }
}