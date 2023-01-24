using System.Linq;
using System.Reflection;

namespace Brainstorm.Mvvm
{
    public abstract class MvvmNavigation : IDisposable
    {
        protected Dictionary<Type, AppService> _appServices;
        protected MvvmNavigation(ServiceMap serviceMap,
                                 string[] viewNamespaces,
                                 string[] viewModelNamespaces)
        {
            var serviceConfiguration = new ServiceConfiguration();
            serviceMap.ConfigureServices(serviceConfiguration);
            _appServices = new Dictionary<Type, AppService>(serviceConfiguration.GetServices());

        }

        protected static IEnumerable<Type> GetTypesFrom<ClassType>(string? @namespace = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                 .SelectMany(a => a.GetExportedTypes()
                     .Where(t => !t.IsAbstract && !t.IsInterface &&
                            (@namespace == null || (t.Namespace?.StartsWith(@namespace) ?? false)) &&
                            typeof(ClassType).IsAssignableFrom(t)));
        }

        public TServiceType GetService<TServiceType>()
        {
            return (TServiceType)GetService(typeof(TServiceType));
        }
        public object GetService(Type serviceType)
        {
            return _appServices[serviceType].GetInstance()
               ?? throw new NullReferenceException(
                   $"Service implementation {serviceType.FullName} ist not registered.");
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static MvvmNavigation Instance { get; protected set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
   
        
        public void Dispose()
        {
            Dispose(true);
        }

        ~MvvmNavigation()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            Instance = null;

            foreach (var service in _appServices)
            {
                service.Value?.Dispose();
            }
            _appServices.Clear();
        }

    }
}