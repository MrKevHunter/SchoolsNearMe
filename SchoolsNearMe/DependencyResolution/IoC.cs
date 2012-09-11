using SchoolsNearMe.Services;
using StructureMap;

namespace SchoolsNearMe.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x => x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    x.For<ISchoolQuery>().Use<SchoolQuery>().EnrichWith(
                        (context, target) => new SchoolQueryLogger(target));
                }));

            return ObjectFactory.Container;
        }
    }
}