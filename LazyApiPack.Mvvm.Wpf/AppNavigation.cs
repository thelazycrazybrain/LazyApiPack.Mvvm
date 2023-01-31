using System.Linq;
using System.Reflection;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf
{
    public class AppNavigation : IDisposable
    {
        protected Dictionary<Type, AppService> _appServices;
        /// <summary>
        /// ViewModel types with relative namespace
        /// </summary>
        protected Dictionary<Type, string> _viewModels;
        /// <summary>
        /// View types with relative namespace
        /// </summary>
        protected Dictionary<Type, string> _views;

        protected AppNavigation(ServiceMap serviceMap,
                                 string[] viewNamespaces,
                                 string[] viewModelNamespaces)
        {
            // TODO: Register IWindowTemplates (Tabbed, Single Page, Multiple, Dialogues)
            // TODO: Register Controllers
            // TODO: Register Instance of Sidebars
            // TODO: Register Menues / Ribbons / Toolbars
            // TODO: Register Statusbars
            var serviceConfiguration = new ServiceConfiguration();
            serviceMap.ConfigureServices(serviceConfiguration);
            _appServices = new Dictionary<Type, AppService>(serviceConfiguration.GetServices());

            _views = GetTypeDictionary(viewNamespaces);
            _viewModels = GetTypeDictionary(viewModelNamespaces);
        }

        Dictionary<Type, string> GetTypeDictionary(string[] namespaces)
        {
            return new Dictionary<Type, string>(
                namespaces.SelectMany(n =>
                    GetTypesFrom<object>(n)
                    .Select(t => new KeyValuePair<Type, string>(
                           t, t.Namespace?.Substring(n.Length).Trim('.') ?? ""))));
        }

        protected static IEnumerable<Type> GetTypesFrom<ClassType>(string? @namespace = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                 .SelectMany(a => a.GetExportedTypes()
                     .Where(t => !t.IsAbstract && !t.IsInterface &&
                            (@namespace == null || (t.Namespace?.StartsWith(@namespace) ?? false)) &&
                            typeof(ClassType).IsAssignableFrom(t)));
        }

        public static void Start(ServiceMap serviceMap, string[] viewNamespaces, string[] viewModelNamespaces)
        {
            Instance = new AppNavigation(serviceMap, viewNamespaces, viewModelNamespaces);
        }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static AppNavigation Instance { get; protected set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


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


        public Type GetAssociatedView(Type viewModel)
        {
            var viewModelNs = _viewModels[viewModel];
            return _views.FirstOrDefault(v => v.Value == viewModelNs).Key;
        }
        public object NavigateTo(Type viewModel, Type view, string region, object? parameter = null)
        {
            var viewInstance = Activator.CreateInstance(view);
            var viewModelInstance = CreateObjectWithDependencyInjection(viewModel);
            if (viewModelInstance is ISupportParameter p)
            {
                p.Parameter = parameter;
            }

            if (viewInstance is FrameworkElement fx)
            {
                fx.DataContext = viewModel;
            }

            // TODO: Create Controllers
            // TODO: Create Instance of Sidebars
            // TODO: Create Menues / Ribbons / Toolbars
            // TODO: Create Statusbars

            // TODO: Navigate to region (Multiple tabs, single page (reusable, modal, multiple windows)
            return viewModelInstance;

        }

        public object CreateObjectWithDependencyInjection(Type type)
        {
            var ctor = type.GetConstructors().First();
            var cparams = ctor.GetParameters();
            var instances = new object[cparams.Length];
            for (int i = 0; i < cparams.Length; i++)
            {
                try
                {
                    instances[i] = AppNavigation.Instance.GetService(cparams[i].ParameterType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Can not create an instance of the service '{_implementationType.FullName}' because one of its dependent services ('{cparams[i].ParameterType.FullName}') could not be created.", ex);

                }
            }

            return ctor.Invoke(instances);

        }
        public TViewModel NavigateTo<TViewModel>(string region, object? parameter = null)
        {
            var view = GetAssociatedView(typeof(TViewModel));

            return NavigateTo(typeof(TViewModel), view, region, parameter);

        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AppNavigation()
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