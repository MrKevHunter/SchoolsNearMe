using System.Web.Http.Dependencies;
using StructureMap;

namespace SchoolMap.Net.DependencyResolution
{
    /// <summary>
    /// Thanks to https://github.com/kenstone/WebApiStructureMapDemo / http://notebookheavy.com/2012/06/25/setup-structuremap-with-asp-net-web-api-release-candidate/
    /// for guiding with the setting up of StrutureMap with web api
    /// </summary>
    public class SmDependencyResolver : StructureMapScope, IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private IContainer _container;

        public SmDependencyResolver(IContainer container)
            : base(container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            _container = IoC.Initialize();
            return new StructureMapScope(_container);
        }
    }
}