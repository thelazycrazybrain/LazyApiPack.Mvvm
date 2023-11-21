using LazyApiPack.Collections;
using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Application.Configuration;
using LazyApiPack.Mvvm.Exceptions;
using LazyApiPack.Mvvm.Model;
using LazyApiPack.Mvvm.Regions;
using LazyApiPack.Mvvm.Services;
using LazyApiPack.Mvvm.Stores;
using System.Reflection;
using System.Windows;

namespace LazyApiPack.Mvvm.Application
{
    /// <summary>
    /// Contains the logic of the mvvm application.
    /// </summary>
    public class MvvmApplication : IDisposable
    {
        private MvvmApplication()
        {

        }
        /// <summary>
        /// The current shell window instance.
        /// </summary>
        private IWindowTemplate? _shellWindow;
        /// <summary>
        /// The current splash screen window instance.
        /// </summary>
        private ISplashScreenWindowTemplate? _splashScreenWindow;
        /// <summary>
        /// All loaded modules.
        /// </summary>
        private List<MvvmModule> _modules = new();
        /// <summary>
        /// All known viewmodels.
        /// </summary>
        private Dictionary<Type, ViewModelTypeInfo> _viewModels = new();
        /// <summary>
        /// All known views.
        /// </summary>
        private Dictionary<Type, ViewTypeInfo> _views = new();
        /// <summary>
        /// All known stores (Interface / Instance).
        /// </summary>
        private Dictionary<Type, Store> _stores = new();
        /// <summary>
        /// All known application services (Interface / Instance)
        /// </summary>
        private Dictionary<Type, AppService> _appServices = new();
        /// <summary>
        /// All known region adapters.
        /// </summary>
        private List<IRegionAdapter> _regionAdapters = new();

        protected record ViewModelTypeInfo(string RelativeName, string FullName);
        protected record ViewTypeInfo(string RelativeName, string FullName);

        /// <summary>
        /// Gets the current application instance.
        /// </summary>
        public static new MvvmApplication Instance { get; private set; }
        private IRegionManager _regionManager;
        public IRegionManager RegionManager
        {
            get => _regionManager; set
            {
                _regionManager = value;
                _regionManager.Initialize(ref _regionAdapters);
            }
        }

        /// <summary>
        /// True, if the application is set up
        /// </summary>
        private static bool _isSetUp;
        private IWindowTemplate SetupInternal()
        {
            var config = new MvvmApplicationConfiguration();
            OnSetup(config);
            var loadedModules = new Dictionary<Type, MvvmModule>();
            GetModulesRecursive(config.Modules, null, ref loadedModules);

            RegionManager.Initialize(ref _regionAdapters);
            if (config.SplashScreen == null)
            {
                throw new ArgumentNullException(nameof(config.ShellWindow));
            }

            _splashScreenWindow = CreateObjectWithDependencyInjection(config.SplashScreen) as ISplashScreenWindowTemplate
                ?? throw new InvalidCastException($"Can not convert SplashScreen of type {config.SplashScreen.FullName} to {typeof(ISplashScreenWindowTemplate).FullName}.");

            ShowSplashScreen("Loading modules", null);

            foreach (var module in loadedModules.Values)
            {
                LoadModule(module);
            }


            var loc = GetService<ILocalizationService>();

            // Initialize service with embedded resources.
            loc.AddLocalizations(typeof(MvvmApplication).Assembly, new[] {
                "LazyApiPack.Mvvm.Localizations.Localization.de.json",
                "LazyApiPack.Mvvm.Localizations.Localization.en.json" });

            loc.AddLocalizations(config.LocalizationFiles);

            ShowSplashScreen(loc.GetTranslation("Captions", "LoadingModules") ?? "Loading modules...", loc.GetTranslation("Descriptions", "InitializeLocalization"));
            OnLocalizationInitialized(loc);

            ShowSplashScreen("Loading application", loc.GetTranslation("Descriptions", "InitializeUi"));
            _shellWindow = CreateObjectWithDependencyInjection(config.ShellWindow) as IWindowTemplate
               ?? throw new InvalidCastException($"Can not convert ShellWindow of type {config.ShellWindow.FullName} to {typeof(IWindowTemplate).FullName}.");

            HideSplashScreen();
            OnSetupComplete();
            foreach (var module in loadedModules.Values)
            {
                module.OnSetupComplete();
            }

            OnSetupComplete();

            _shellWindow.Show();
            return _shellWindow;
        }
        /// <summary>
        /// Scaffolds the application and runs it.
        /// </summary>
        public static IWindowTemplate Setup()
        {
            if (_isSetUp)
            {
                throw new InvalidOperationException("Application is already set up");
            }
            Instance = new MvvmApplication();
            return Instance.SetupInternal();

        }

        /// <summary>
        /// Fetches the configuration for the scaffolding process.
        /// </summary>
        protected virtual void OnSetup(MvvmApplicationConfiguration configuration) { }
        /// <summary>
        /// Indicates that the application is fully loaded and ready.
        /// </summary>
        protected virtual void OnSetupComplete() { }
        /// <summary>
        /// Indicates that the localization system is initialized and ready.
        /// </summary>
        protected virtual void OnLocalizationInitialized(ILocalizationService service) { }
        /// <summary>
        /// Is called when another module messages the application.
        /// </summary>
        /// <param name="sender">Module that sent the message.</param>
        /// <param name="moduleId">Id of the calling module.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="message">The actual payload.</param>
        protected virtual void OnMessageReceived(MvvmModule? sender, string? moduleId, string? messageId, object? message) { }


        /// <summary>
        /// Loads a single module into the application.
        /// </summary>
        private void LoadModule(MvvmModule module)
        {
            _modules.Add(module);
            foreach (var service in module.Configuration.ServiceMappings)
            {
                _appServices.Add(service.Key, service.Value);
            }

            foreach (var view in GetTypeDictionary(module.Configuration.ViewNamespaces, (type, relativeNs) => new ViewTypeInfo(relativeNs, type.FullName ?? type.Name)))
            {
                _views.Add(view.Key, view.Value);
            }

            foreach (var viewModel in GetTypeDictionary(module.Configuration.ViewModelNamespaces,
                (type, relativeNs) => new ViewModelTypeInfo(relativeNs, type.FullName ?? type.Name)))
            {
                _viewModels.Add(viewModel.Key, viewModel.Value);
            }

            foreach (var adapter in module.Configuration.RegionAdapters)
            {
                _regionAdapters.Add((IRegionAdapter)Activator.CreateInstance(adapter)
                    ?? throw new NullReferenceException($"Creation of adapter {adapter.FullName} for module {module.ModuleId} returned null."));
            }

            module.OnModuleLoaded();
            module.OnActivated();
        }

        /// <summary>
        /// Gets all modules and submodules that are added to the application.
        /// </summary>
        private void GetModulesRecursive(List<Type> moduleTypes, MvvmModule? parentModule, ref Dictionary<Type, MvvmModule> loadedModules)
        {
            loadedModules = loadedModules ?? new Dictionary<Type, MvvmModule>();
            // if parentmodule is null, the module is part of an application
            // so if the application does not contain any modules, throw an exception.
            if (parentModule == null && (moduleTypes == null || moduleTypes.Count == 0))
            {
                throw new InvalidOperationException("This application does not contain any modules.");
            }

            foreach (var module in moduleTypes)
            {
                // ensure, that the module is not loaded twice but is in relation with its parent module
                // unload module only if there are no parents including NULL (Root Module).
                if (loadedModules.ContainsKey(module))
                {
                    if (!loadedModules[module].ParentModules.Contains(parentModule))
                    {
                        loadedModules[module].ParentModules.Add(parentModule);
                    }
                    continue;
                }
                var inst = Activator.CreateInstance(module) as MvvmModule
                    ?? throw new NullReferenceException($"Module {module.FullName} could not be instantiated.");
                loadedModules.Add(module, inst);
                inst.Setup(this, parentModule);
                GetModulesRecursive(inst.Configuration.Modules, inst, ref loadedModules);
            }
        }

        #region Navigation
        /// <summary>
        /// Navigates to the viewmodel.
        /// </summary>
        /// <typeparam name="TViewModel">The viewmodel to navigate to.</typeparam>
        /// <param name="region">The region in which the view should be displayed.</param>
        /// <param name="parameter">The parameter for the viewmodel.</param>
        /// <param name="model">The model for the viewmodel.</param>
        /// <param name="isModal">Indicates, if the application should wait for the view to be closed.</param>
        /// <returns>The created viewmodel.</returns>
        public TViewModel NavigateTo<TViewModel>(string region, object? parameter = null, object? model = null, bool isModal = false)
        {
            return (TViewModel)NavigateTo(typeof(TViewModel), region, parameter, model, isModal);
        }

        /// <summary>
        /// Navigates to the viewmodel.
        /// </summary>
        /// <param name="viewModelName">The viewmodel to navigate to.</param>
        /// <param name="region">The region in which the view should be displayed.</param>
        /// <param name="parameter">The parameter for the viewmodel.</param>
        /// <param name="model">The model for the viewmodel.</param>
        /// <param name="isModal">Indicates, if the application should wait for the view to be closed.</param>
        /// <returns>The created viewmodel.</returns>
        public object NavigateTo(string viewModelName, string region, object? parameter = null, object? model = null, bool isModal = false)
        {
            if (!_viewModels.Any(m => string.Compare(m.Value.RelativeName, viewModelName) == 0))
            {
                throw new ViewModelNotFoundException(viewModelName);
            }
            var viewModelItems = _viewModels.Where(m => string.Compare(m.Value.RelativeName, viewModelName) == 0);
            if (viewModelItems.Count() > 1)
            {
                // If duplicate model names, use full namespace
                viewModelItems = _viewModels.Where(m => string.Compare(m.Value.FullName, viewModelName) == 0);
            }
            var viewModel = CreateObjectWithDependencyInjection(viewModelItems.First().Key);

            var viewItems = _views.Where(v => v.Value.RelativeName == viewModelName.Substring(0, viewModelName.Length - "Model".Length));
            if (viewItems.Count() > 1)
            {
                // If duplicate model names, use full namespace
                viewItems = _views.Where(v => v.Value.FullName == GetViewNamespace(viewModelName));
            }

            var view = CreateObjectWithDependencyInjection(viewItems.First().Key);
            return NavigateTo(viewModel, view, region, model, parameter, isModal);

        }

        /// <summary>
        /// Navigates to the viewmodel.
        /// </summary>
        /// <param name="viewType">The view that should be used for the viewmodel.</param>
        /// <param name="viewModelType">The viewmodel to navigate to</param>
        /// <param name="region">THe region in which the view should be displayed.</param>
        /// <param name="parameter">The parameter for the viewmodel.</param>
        /// <param name="model">The model for the viewmodel.</param>
        /// <param name="isModal">Indicates, if the application should wait for the view to be closed.</param>
        /// <returns>The created viewmodel.</returns>
        public object NavigateTo(Type viewModelType, Type viewType, string region, object? model = null, object? parameter = null, bool isModal = false)
        {
            var viewModel = CreateObjectWithDependencyInjection(viewModelType);
            var view = CreateObjectWithDependencyInjection(viewType);
            return NavigateTo(viewModel, view, region, model, parameter, isModal);
        }

        /// <summary>
        /// Navigates to the viewmodel.
        /// </summary>
        /// <param name="viewModel">The viewmodel to navigate to.</param>
        /// <param name="view">The view that should be used.</param>
        /// <param name="region">THe region in which the view should be displayed.</param>
        /// <param name="parameter">The parameter for the viewmodel.</param>
        /// <param name="model">The model for the viewmodel.</param>
        /// <param name="isModal">Indicates, if the application should wait for the view to be closed.</param>
        /// <returns>The created viewmodel.</returns>
        private object NavigateTo(object viewModel, object view, string region, object? model, object? parameter, bool isModal)
        {
            if (viewModel is ISupportModel m)
            {
                m.Model = model;
            }
            if (viewModel is ISupportParameter p)
            {
                p.Parameter = parameter;
            }

            if (view is IView fe)
            {
                fe.DataContext = viewModel;
            }
            else
            {

                var prop = view.GetType().GetProperty("DataContext", BindingFlags.Instance | BindingFlags.Public);
                if (prop != null)
                {
                    prop.SetValue(view, viewModel);
                }
            }
            RegionManager.NavigateTo(view, region, isModal);
            return viewModel;
        }
        /// <summary>
        /// Navigates to the viewmodel.
        /// </summary>
        /// <param name="viewModelType">The viewmodel to navigate to.</param>
        /// <param name="region">THe region in which the view should be displayed.</param>
        /// <param name="parameter">The parameter for the viewmodel.</param>
        /// <param name="model">The model for the viewmodel.</param>
        /// <param name="isModal">Indicates, if the application should wait for the view to be closed.</param>
        /// <returns>The created viewmodel.</returns>
        public object NavigateTo(Type viewModelType, string region, object? parameter = null, object? model = null, bool isModal = false)
        {
            var view = _views.First(v => v.Value.FullName == viewModelType.FullName.Substring(0, viewModelType.FullName.Length - "Model".Length));
            return NavigateTo(viewModelType, view, region, model, parameter, isModal);

        }
        /// <summary>
        /// Gets the namespace of a view for a given viewmodel
        /// </summary>
        /// <param name="viewModelNamespace">The viewmodel namespace</param>
        /// <example>My.Application.ViewModels.MyViewModel results in My.Application.Views.MyView</example>
        private string GetViewNamespace(string viewModelNamespace)
        {
            var viewModelName = viewModelNamespace.Substring(viewModelNamespace.LastIndexOf(".") + 1);
            var viewName = viewModelName.Substring(0, viewModelName.Length - "Model".Length);
            var viewNamespace = viewModelNamespace.Substring(0, viewModelNamespace.LastIndexOf("ViewModels")) + "Views";

            return $"{viewNamespace}.{viewName}";

        }

        /// <summary>
        /// Shows the application splash screen.
        /// </summary>
        /// <param name="progressTitle">Title (eg. Starting Application)</param>
        /// <param name="progressDescription">Description (eg. Loading Modules)</param>
        /// <param name="progressPercentage">Progress in percent (0 - 1)</param>
        public void ShowSplashScreen(string progressTitle, string progressDescription, double? progressPercentage = null)
        {
            if (!_splashScreenWindow.IsVisible)
            {
                _splashScreenWindow.Show();
            }
            _splashScreenWindow.ProgressTitle = progressTitle;
            _splashScreenWindow.ProgressDescription = progressDescription;
            _splashScreenWindow.ProgressPercentage = progressPercentage;
        }

        /// <summary>
        /// Hides the application splash screen.
        /// </summary>
        public void HideSplashScreen()
        {
            if (_splashScreenWindow.IsVisible)
            {
                _splashScreenWindow.Close();
            }
        }

        /// <summary>
        /// Sends a message to a module.
        /// </summary>
        /// <param name="sender">Module that sent the message.</param>
        /// <param name="moduleId">Id of the receiving module.</param>
        /// <param name="messageId">Id of the message (custom).</param>
        /// <param name="message">Payload</param>
        /// <returns>True, if the message was sent, false, if the module was not found. (Does not indicate whether the receiving module has processed the message!)</returns>
        public bool SendMessage(MvvmModule sender, string moduleId, string messageId, object message)
        {
            if (moduleId != null)
            {
                var target = _modules.FirstOrDefault(m => m.ModuleId == moduleId);
                if (target == null) return false;
                target.OnMessageReceived(sender, moduleId, messageId, message);
            }
            else
            {
                foreach (var module in _modules)
                {
                    module.OnMessageReceived(sender, moduleId, messageId, message);
                }
                if (sender != null)
                {
                    this.OnMessageReceived(sender, moduleId, messageId, message);
                }
            }
            return true;
            //OnSendMessage(moduleId, messageId, message);
        }
        #endregion

        #region Mvvm Object creation
        /// <summary>
        /// Gets a service for a specific service interface.
        /// </summary>
        /// <typeparam name="TServiceType">Interface for that service.</typeparam>
        /// <returns>An instance of the service implementation.</returns>
        public TServiceType GetService<TServiceType>()
        {
            return (TServiceType)GetService(typeof(TServiceType));
        }

        /// <summary>
        /// Gets a service for a specific service interface.
        /// </summary>
        /// <param name="serviceType">Interface for that service.</param>
        /// <returns>An instance of the service implementation.</returns>
        public object GetService(Type serviceType)
        {
            return _appServices[serviceType].GetInstance()
               ?? throw new NullReferenceException(
                   $"Service implementation {serviceType.FullName} ist not registered.");
        }
        /// <summary>
        /// Creates an object with dependency injection.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns>An instance of an object that uses dependency injection.</returns>
        /// <exception cref="NotSupportedException">The object has more than one or no public constructors.</exception>
        /// <exception cref="InvalidOperationException">The object can not be created because not all services are known by the application.</exception>
        public object CreateObjectWithDependencyInjection(Type type)
        {
#if DEBUG
            var ctors = type.GetConstructors().Where(c => !c.IsStatic && !c.IsAbstract && c.IsPublic).ToList();
            if (ctors.Count > 1)
            {
                throw new NotSupportedException($"The object {type.FullName} has more than one public instance constructor. Please make sure to use only one public instance constructor with DependencyInjection.");
            }
            if (ctors.Count == 0)
            {
                throw new NotSupportedException($"The object {type.FullName} has no public instance constructors. Please make sure to use a public instance constructor with DependencyInjection.");
            }
#endif
            var ctor = type.GetConstructors().First(c => !c.IsStatic && !c.IsAbstract && c.IsPublic);

            var cparams = ctor.GetParameters();
            var instances = new object[cparams.Length];
            for (int i = 0; i < cparams.Length; i++)
            {
                if (cparams[i].ParameterType.IsInterface)
                {
                    try
                    {
                        instances[i] = GetService(cparams[i].ParameterType);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(
                            $"Can not create an instance of the service '{type.FullName}' because one of its dependent services ('{cparams[i].ParameterType.FullName}') could not be created.", ex);

                    }
                }
                else
                {
                    instances[i] = _stores[cparams[i].ParameterType].GetInstance();
                }

            }

            return ctor.Invoke(instances);

        }

        #endregion

        #region Reflection
        /// <summary>
        /// Gets the types from a specific namespace within the app domain.
        /// </summary>
        /// <typeparam name="ClassType">Types that are within the namespace.</typeparam>
        /// <param name="namespace">Namespace to be searched.</param>
        /// <returns>The types that are within the given namespace.</returns>
        private static IEnumerable<Type> GetTypesFrom<ClassType>(string? @namespace = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                 .Where(a => !a.IsDynamic)
                 .SelectMany(a => a.GetExportedTypes()
                     .Where(t => !t.IsAbstract && !t.IsInterface &&
                            (@namespace == null || (t.Namespace?.StartsWith(@namespace) ?? false)) &&
                            typeof(ClassType).IsAssignableFrom(t)));
        }

        /// <summary>
        /// Gets a list of types from the given namespaces grouped by a key value (Relative Namespace and Type)
        /// </summary>
        /// <typeparam name="TValue">Type of the key</typeparam>
        /// <param name="namespaces">List of namespaces to be looked in.</param>
        /// <param name="getValue">Method that converts the relative namespace and type into a value</param>
        /// <returns></returns>
        private Dictionary<Type, TValue> GetTypeDictionary<TValue>(IEnumerable<string> namespaces, Func<Type, string, TValue> getValue)
        {
            return new Dictionary<Type, TValue>(
                new Dictionary<Type, TValue>(namespaces.SelectMany(n =>
                    GetTypesFrom<object>(n)
                    .Select(t => new KeyValuePair<Type, TValue>(t, getValue(t, t.Name)))))
                );
        }

        #endregion

        /// <summary>
        /// Signals if the application is currently loading and a loading screen should be displayed.
        /// </summary>
        public BoolList IsBusy = new BoolList();

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MvvmApplication()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            throw new NotImplementedException();
            //foreach (var service in _appServices)
            //{
            //    service.Value?.Dispose();
            //}
        }
        #endregion
    }


}

