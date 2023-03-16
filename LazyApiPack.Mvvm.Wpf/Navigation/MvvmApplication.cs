using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Exceptions;
using LazyApiPack.Mvvm.Wpf.Localization;
using LazyApiPack.Mvvm.Wpf.Model;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters;
using LazyApiPack.Mvvm.Wpf.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LazyApiPack.Mvvm.Wpf.Navigation {

    public static class MvvmAppConfigurationExtensions {
        public static MvvmAppConfiguration WithShellWindow<TShellWindow>(this MvvmAppConfiguration config) where TShellWindow : IWindowTemplate {
            config.ShellWindow = typeof(TShellWindow);
            return config;
        }

        public static MvvmAppConfiguration WithSplashWindow<TSplashWindow>(this MvvmAppConfiguration config) where TSplashWindow : ISplashScreenWindowTemplate {
            config.SplashScreen = typeof(TSplashWindow);
            return config;
        }

        public static MvvmAppConfiguration WithViewModels(this MvvmAppConfiguration config, params string[] viewModelNamespaces) {
            config.ViewModelNamespaces = viewModelNamespaces;
            return config;
        }

        public static MvvmAppConfiguration WithViews(this MvvmAppConfiguration config, params string[] viewNamespaces) {
            config.ViewNamespaces = viewNamespaces;
            return config;
        }

        public static MvvmAppConfiguration WithWindowTemplates(this MvvmAppConfiguration config, params string[] windowTemplatesNamespaces) {
            config.WindowTemplateNamespaces = windowTemplatesNamespaces;
            return config;
        }

        /// <summary>
        /// Adds localization directories.
        /// </summary>
        public static MvvmAppConfiguration WithLocalizationDirectories(this MvvmAppConfiguration config, string? searchPattern, EnumerationOptions? options, params string[] localizationDirectories) {
            config.LocalizationDirectories = localizationDirectories;
            config.LocalizationSearchPattern = searchPattern;
            config.LocalizationOptions = options;
            return config;
        }
        /// <summary>
        /// Adds localization directories with filter *.*
        /// </summary>
        public static MvvmAppConfiguration WithLocalizationDirectories(this MvvmAppConfiguration config, EnumerationOptions? options, params string[] localizationDirectories) {
            config.LocalizationDirectories = localizationDirectories;
            config.LocalizationSearchPattern = "*.*";
            config.LocalizationOptions = options;
            return config;
        }

        /// <summary>
        /// Adds localization directories with filter *.* and search options recursive.
        /// </summary>
        public static MvvmAppConfiguration WithLocalizationDirectories(this MvvmAppConfiguration config, params string[] localizationDirectories) {
            config.LocalizationDirectories = localizationDirectories;
            config.LocalizationSearchPattern = "*.*";
            config.LocalizationOptions = new EnumerationOptions() { RecurseSubdirectories = true };
            return config;
        }

    }
    /// <summary>
    /// Provides a convenient shortcut to access the current MvvmApplication throughout the application.
    /// </summary>
    public static class MvvmApp {
        /// <summary>
        /// Returns the application instance as an MvvmApplication.
        /// </summary>
        public static MvvmApplication Navigation { get => (MvvmApplication)Application.Current; }
    }

    public sealed class MvvmAppConfiguration {

        public Type ShellWindow { get; internal set; }
        public Type SplashScreen { get; internal set; }
        public string[] ViewModelNamespaces { get; internal set; }
        public string[] ViewNamespaces { get; internal set; }
        public string[] WindowTemplateNamespaces { get; internal set; }
        public string[] LocalizationDirectories { get; internal set; }
        public string? LocalizationSearchPattern { get; internal set; }
        public EnumerationOptions? LocalizationOptions { get; internal set; }
    }


    public class RegionAdapterInstance {
        public RegionAdapterInstance(UIElement uiControl, Type regionAdapter) {
            Instance = (IRegionAdapter?)Activator.CreateInstance(regionAdapter, new[] { uiControl }) ?? throw new NullReferenceException();

        }

        public IRegionAdapter Instance { get; set; }
    }
    public abstract class MvvmApplication : Application, IDisposable {

        protected Dictionary<Type, AppService> _appServices = new();
        /// <summary>
        /// ViewModel types with relative namespace
        /// </summary>
        protected Dictionary<Type, ViewModelTypeInfo> _viewModels = new();
        /// <summary>
        /// View types with relative namespace
        /// </summary>
        protected Dictionary<Type, ViewTypeInfo> _views = new();

        protected Dictionary<Type, Type> _controlToRegionAdapterMap = new();
        protected Dictionary<string, RegionAdapterInstance> _regionToRegionAdapterMap = new();

        protected Type _splashScreenWindow;
        protected Type _shellWindow;
        protected ISplashScreenWindowTemplate _currentSplashScreenWindow;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            var config = new MvvmAppConfiguration();
            OnSetup(config);



            _shellWindow = config.ShellWindow ?? throw new ArgumentNullException(nameof(config.ShellWindow));
            _splashScreenWindow =  config.SplashScreen ?? throw new ArgumentNullException(nameof(config.SplashScreen));

            // At this time, there are no services loaded. The method is used anyway to prevent code duplication
            if (CreateObjectWithDependencyInjection(_splashScreenWindow) is not ISplashScreenWindowTemplate splash) {
                throw new ArgumentException(nameof(config.SplashScreen), new InvalidCastException($"The splash screen is not assignable to {nameof(ISplashScreenWindowTemplate)}"));
            }

            _currentSplashScreenWindow = splash;

            splash.Show();
            _currentSplashScreenWindow.ProgressTitle = "Loading modules...";

            var serviceConfiguration = new ServiceConfiguration();
            ConfigureServices(serviceConfiguration);
            _appServices = serviceConfiguration.GetServices();

            var loc = GetService<ILocalizationService>();

            // Initialize service with embedded resources.
            loc.AddLocalizations(Assembly.GetExecutingAssembly(), new[] { "LazyApiPack.Mvvm.Wpf.Localizations" }, "Localization.*.json");

            loc.AddLocalizations(config.LocalizationDirectories, config.LocalizationSearchPattern, config.LocalizationOptions);



            _currentSplashScreenWindow.ProgressTitle = loc.GetTranslation("Captions", "LoadingModules") ?? "Loading modules...";
            _currentSplashScreenWindow.ProgressDescription = loc.GetTranslation("Descriptions", "InitializeLocalization");
            OnLocalizationInitialized(loc);



            _currentSplashScreenWindow.ProgressDescription = loc.GetTranslation("Descriptions", "InitializeUi");


            _views = GetTypeDictionary(config.ViewNamespaces, (type, relativeNs) => new ViewTypeInfo(relativeNs));

            _viewModels = GetTypeDictionary(config.ViewModelNamespaces, (type, relativeNs) => {
                var interfaces = type.GetInterfaces();
                return new ViewModelTypeInfo(
                                        relativeNs,
                                        interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISupportParameter<>)),
                                        interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISupportModel<>)));
            });


            var adapterConfiguration = new RegionAdapterConfiguration();
            ConfigureRegionsInternal(adapterConfiguration);
            ConfigureRegions(adapterConfiguration);
            _controlToRegionAdapterMap = adapterConfiguration.GetRegionAdapters();
            // TODO: Register IWindowTemplates (Tabbed, Single Page, Multiple, Dialogues)


            var shell = CreateObjectWithDependencyInjection(config.ShellWindow) as IWindowTemplate ?? throw new InvalidOperationException();
            shell.Show();

            splash.Close();
            OnStartupComplete();
        }

        protected abstract void OnStartupComplete();
        protected abstract void OnSetup(MvvmAppConfiguration configuration);

        protected abstract void ConfigureServices(ServiceConfiguration configuration);

        private void ConfigureRegionsInternal(RegionAdapterConfiguration configuration) {
            configuration.Map<ContentControl, ContentControlRegionAdapter>();
            configuration.Map<TabControl, TabControlRegionAdapter>();
        }
        protected abstract void ConfigureRegions(RegionAdapterConfiguration configuration);

        protected abstract void OnLocalizationInitialized(ILocalizationService service);

        public void RegisterWindowTemplate<TWindowTemplate>(string name, params string[] regions) where TWindowTemplate : IWindowTemplate {
            //_regionToWindowMap.Add(region, name, typeof(TWindowTemplate));
        }

        public void UnregisterWindowTemplate(string name) {
            //_regionToWindowMap.Remove( name);
        }

        Dictionary<Type, TKey> GetTypeDictionary<TKey>(string[] namespaces, Func<Type, string, TKey> getKey) {
            return new Dictionary<Type, TKey>(
                new Dictionary<Type, TKey>(namespaces.SelectMany(n =>
                    GetTypesFrom<object>(n)
                    .Select(t => new KeyValuePair<Type, TKey>(t, getKey(t, GetRelativeNamespace(t, n))))))
                );
        }

        protected static IEnumerable<Type> GetTypesFrom<ClassType>(string? @namespace = null) {
            return AppDomain.CurrentDomain.GetAssemblies()
                 .Where(a => !a.IsDynamic)
                 .SelectMany(a => a.GetExportedTypes()
                     .Where(t => !t.IsAbstract && !t.IsInterface &&
                            (@namespace == null || (t.Namespace?.StartsWith(@namespace) ?? false)) &&
                            typeof(ClassType).IsAssignableFrom(t)));
        }

        public TViewModel NavigateTo<TViewModel>(string region, object? model = null, object? parameter = null) {
            var view = GetAssociatedView(typeof(TViewModel)) ?? throw new ViewNotFoundException($"The view for the model {typeof(TViewModel).FullName} was not found.");

            return (TViewModel)NavigateTo(typeof(TViewModel), view, region, model, parameter);

        }

        public object NavigateTo(Type viewModel, Type view, string region, object? model = null, object? parameter = null) {
            var viewInstance = Activator.CreateInstance(view);
            var viewModelInstance = CreateObjectWithDependencyInjection(viewModel);
            var vmInfo = _viewModels[viewModel];
            if (model != null  && vmInfo.SupportsModel) {
                viewModel.GetProperty(nameof(ISupportModel<object>.Model), BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(viewModelInstance, model);

            }

            if (viewInstance is FrameworkElement fx) {
                fx.DataContext = viewModelInstance;
            }

            if (parameter != null &&vmInfo.SupportsParameter) {
                viewModel.GetProperty(nameof(ISupportParameter<object>.Parameter), BindingFlags.Public | BindingFlags.Instance)
                    ?.SetValue(viewModelInstance, parameter);

            }

            // TODO: Navigate to region (Multiple tabs, single page (reusable, modal, multiple windows)
            if (!_regionToRegionAdapterMap.ContainsKey(region)) {
                var control = RegionManager.GetNavigationControl(region);
                _regionToRegionAdapterMap.Add(region, new RegionAdapterInstance(control, _controlToRegionAdapterMap[control.GetType()]));
            }
            _regionToRegionAdapterMap[region].Instance.AddView(viewInstance);
            //_uiToRegionAdapterMap[control.GetType()];
            return viewModelInstance;

        }

        public TServiceType GetService<TServiceType>() {
            return (TServiceType)GetService(typeof(TServiceType));
        }
        public object GetService(Type serviceType) {
            return _appServices[serviceType].GetInstance()
               ?? throw new NullReferenceException(
                   $"Service implementation {serviceType.FullName} ist not registered.");
        }

        public Type GetAssociatedView(Type viewModel) {
            var viewModelNs = _viewModels[viewModel];
            return _views.FirstOrDefault(v => v.Value.RelativeNamespace == viewModelNs.RelativeNamespace).Key;
        }

        public object CreateObjectWithDependencyInjection(Type type) {
#if DEBUG
            var ctors = type.GetConstructors().Where(c => !c.IsStatic && !c.IsAbstract && c.IsPublic).ToList();
            if (ctors.Count > 1) {
                throw new NotSupportedException($"The object {type.FullName} has more than one public instance constructor. Please make sure to use only one public instance constructor with DependencyInjection.");
            }
            if (ctors.Count == 0) {
                throw new NotSupportedException($"The object {type.FullName} has more no public instance constructors. Please make sure to use a public instance constructor with DependencyInjection.");
            }
#endif
            var ctor = type.GetConstructors().First(c => !c.IsStatic && !c.IsAbstract && c.IsPublic);

            var cparams = ctor.GetParameters();
            var instances = new object[cparams.Length];
            for (int i = 0; i < cparams.Length; i++) {
                try {
                    instances[i] = GetService(cparams[i].ParameterType);
                } catch (Exception ex) {
                    throw new InvalidOperationException(
                        $"Can not create an instance of the service '{type.FullName}' because one of its dependent services ('{cparams[i].ParameterType.FullName}') could not be created.", ex);

                }
            }

            return ctor.Invoke(instances);

        }


        private string GetRelativeNamespace(Type type, string fullNamespace) => type.Namespace?.Substring(fullNamespace.Length).Trim('.') ?? "";

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvvmApplication() {
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing) {
            foreach (var service in _appServices) {
                service.Value?.Dispose();
            }
        }



        protected class ViewModelTypeInfo {
            public readonly string RelativeNamespace;
            public readonly bool SupportsParameter;
            public readonly bool SupportsModel;

            public ViewModelTypeInfo(string relativeNamespace, bool supportsParameter, bool supportsModel) {
                RelativeNamespace = relativeNamespace;
                SupportsParameter = supportsParameter;
                SupportsModel = supportsModel;
            }
        }

        protected class ViewTypeInfo {
            public readonly string RelativeNamespace;
            public ViewTypeInfo(string relativeNamespace) {
                RelativeNamespace = relativeNamespace;
            }
        }

    }
}