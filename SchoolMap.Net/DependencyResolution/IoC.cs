using SchoolMap.Net.Services;
using StructureMap;

namespace SchoolMap.Net.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(delegate(IInitializationExpression x)
                                         {
                                             x.Scan(scan =>
                                                        {
                                                            scan.TheCallingAssembly();
                                                            scan.WithDefaultConventions();
                                                        });

                                             x.For<ILocationService>().Use<HostIpLocationService>();
                                             x.For<ISchoolQuery>().Use<SchoolQuery>();
                                         });
            return ObjectFactory.Container;
        }
    }
}