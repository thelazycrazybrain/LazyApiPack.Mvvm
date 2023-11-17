using LazyApiPack.Collections;
using LazyApiPack.Collections.Extensions;
using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Model;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters;
using LazyApiPack.Mvvm.Wpf.Services;
using LazyApiPack.Mvvm.Wpf.Stores;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Application
{

    public abstract class MvvmApplication : System.Windows.Application, IDisposable
    {
        public static MvvmApplication Navigation { get => (MvvmApplication)Current; }
        //private class MvvmApplicationMessageEventArgs : EventArgs
        //{
        //    public MvvmApplicationMessageEventArgs(MvvmModule sender, string moduleId, string messageId, object message)
        //    {
        //        Sender = sender;
        //        ModuleId = moduleId;
        //        MessageId = messageId;
        //        Message = message;
        //    }
        //   public MvvmModule Sender { get; private set; }
        //    public string ModuleId { get; private set; }

        //    public string MessageId { get; private set; }
        //    public object Message { get; private set; }
        //}
        //private delegate void MvvmApplicationMessageDelegate(object sender,  MvvmApplicationMessageEventArgs e);
        //private event MvvmApplicationMessageDelegate OnApplicationMessageReceived;
        private IWindowTemplate? _shellWindow;
        private ISplashScreenWindowTemplate? _splashScreenWindow;
        private List<IRegionAdapter> _regionAdapters = new();

        private Dictionary<Type, AppService> _appServices = new();
        private Dictionary<Type, Store> _stores = new();
        private List<MvvmModule> _modules = new();
        private Dictionary<Type, ViewTypeInfo> _views = new();
        private Dictionary<Type, ViewModelTypeInfo> _viewModels = new();
        private Dictionary<Type, IWindowTemplate> _openWindows = new();


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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
                "LazyApiPack.Mvvm.Wpf.Localizations.Localization.de.json",
                "LazyApiPack.Mvvm.Wpf.Localizations.Localization.en.json" });

            loc.AddLocalizations(config.LocalizationFiles);

            ShowSplashScreen(loc.GetTranslation("Captions", "LoadingModules") ?? "Loading modules...", loc.GetTranslation("Descriptions", "InitializeLocalization"));
            OnLocalizationInitialized(loc);

            ShowSplashScreen("Loading application", loc.GetTranslation("Descriptions", "InitializeUi"));
            _shellWindow = CreateObjectWithDependencyInjection(config.ShellWindow) as IWindowTemplate
               ?? throw new InvalidCastException($"Can not convert ShellWindow of type {config.ShellWindow.FullName} to {typeof(IWindowTemplate).FullName}.");

            HideSplashScreen();
            OnStartupComplete();
            MvvmApplication.Current.MainWindow = (Window)_shellWindow;
            _shellWindow.Show();

        }
        private void LoadModule(MvvmModule module)
        {
            _modules.Add(module);
            foreach (var service in module.Configuration.ServiceMappings)
            {
                _appServices.Add(service.Key, service.Value);
            }

            foreach(var view in GetTypeDictionary(module.Configuration.ViewNamespaces, (type, relativeNs) => new ViewTypeInfo(relativeNs)))
            {
                _views.Add(view.Key, view.Value);
            }

            foreach (var viewModel in GetTypeDictionary(module.Configuration.ViewModelNamespaces, (type, relativeNs) =>
            {
                var interfaces = type.GetInterfaces();
                return new ViewModelTypeInfo(
                                        relativeNs,
                                        interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISupportParameter<>)),
                                        interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISupportModel<>)));
            }))
            {
                _viewModels.Add(viewModel.Key, viewModel.Value);

            }

            foreach (var adapter in module.Configuration.RegionAdapters)
            {
                _regionAdapters.Add((IRegionAdapter)Activator.CreateInstance(adapter));
            }

            //_regions.Add(module, module.Configuration.Regions); // Obsolete

            //ConfigureRegionsInternal(adapterConfiguration);
            //ConfigureRegions(adapterConfiguration);
            //_controlToRegionAdapterMap = adapterConfiguration.GetRegionAdapters();
            // TODO: Register IWindowTemplates (Tabbed, Single Page, Multiple, Dialogues)








            module.OnModuleLoaded();
            module.OnActivated();
        }
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
        #region Abstract Handlers
        protected abstract void OnStartupComplete();
        protected abstract void OnSetup(MvvmApplicationConfiguration configuration);
        protected abstract void OnLocalizationInitialized(ILocalizationService service);
        protected abstract void OnMessageReceived(MvvmModule? sender, string? moduleId, string? messageId, object? message);
        #endregion

        #region Navigation
        public TViewModel NavigateTo<TViewModel>(string region, object? parameter = null, object? model = null, bool isModal = false)
        {
            return (TViewModel)NavigateTo(typeof(TViewModel), region, parameter, model, isModal);
        }

        public object NavigateTo(string viewModelName, string region, object? parameter = null, object? model = null, bool isModal = false)
        {
            if (!_viewModels.Any(m => string.Compare(m.Value.RelativeNamespace, viewModelName) == 0))
            {
                throw new ViewModelNotFoundException(viewModelName);
            }
            var viewModelItem = _viewModels.First(m => string.Compare(m.Value.RelativeNamespace, viewModelName) == 0);

            var viewModel = CreateObjectWithDependencyInjection(viewModelItem.Key);

            var viewItem = _views.First(v => v.Value.RelativeNamespace == viewModelName.Substring(0, viewModelName.Length - "Model".Length));
            var view = CreateObjectWithDependencyInjection(viewItem.Key);
            return NavigateTo(viewModel, view, region, model, parameter, isModal);

        }

        public object NavigateTo(Type viewModelType, string region, object? parameter = null, object? model = null, bool isModal = false)
        {
            var view = GetAssociatedView(viewModelType) ?? throw new ViewNotFoundException($"The view for the model {viewModelType.FullName} was not found.");
            return NavigateTo(viewModelType, view, region, model, parameter, isModal);

        }
        public object NavigateTo(Type viewModelType, Type viewType, string region, object? model = null, object? parameter = null, bool isModal = false)
        {
            var viewModel = CreateObjectWithDependencyInjection(viewModelType);
            var view = CreateObjectWithDependencyInjection(viewType);
            return NavigateTo(viewModel, view, region, model, parameter, isModal);
        }

        private object NavigateTo(object viewModel, object view, string region, object? model, object? parameter, bool isModal)
        {
            if (viewModel is ISupportModel m) {
                m.Model = model;
            }
            if (viewModel is ISupportParameter p)
            {
                p.Parameter = parameter;
            }

            if (view is FrameworkElement fe)
            {
                fe.DataContext = viewModel;
            }
            RegionManager.NavigateTo(view, region, isModal);
            return viewModel;
        }

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

        public void HideSplashScreen()
        {
            if (_splashScreenWindow.IsVisible)
            {
                _splashScreenWindow.Close();
            }
        }

        //public void UpdateSplashScreen(string progressTitle, string progressDescription, double? progressPercentage = null)
        //{
        //    ShowSplashScreen(progressTitle, progressDescription, progressPercentage);
        //}

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
        public Type GetAssociatedView(Type viewModel)
        {
            throw new NotImplementedException();
            //var viewModelNs = _viewModels[viewModel];
            //return _views.FirstOrDefault(v => v.Value.RelativeNamespace == viewModelNs.RelativeNamespace).Key;
        }
        #endregion

        //#region Internal Configuration
        //private void ConfigureRegionsInternal(RegionAdapterConfiguration configuration)
        //{
        //    configuration.Map<ContentControl, ContentControlRegionAdapter>();
        //    configuration.Map<TabControl, TabControlRegionAdapter>();
        //}
        //#endregion

        #region Reflection
        private static IEnumerable<Type> GetTypesFrom<ClassType>(string? @namespace = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                 .Where(a => !a.IsDynamic)
                 .SelectMany(a => a.GetExportedTypes()
                     .Where(t => !t.IsAbstract && !t.IsInterface &&
                            (@namespace == null || (t.Namespace?.StartsWith(@namespace) ?? false)) &&
                            typeof(ClassType).IsAssignableFrom(t)));
        }
        private Dictionary<Type, TKey> GetTypeDictionary<TKey>(IEnumerable<string> namespaces, Func<Type, string, TKey> getKey)
        {
            return new Dictionary<Type, TKey>(
                new Dictionary<Type, TKey>(namespaces.SelectMany(n =>
                    GetTypesFrom<object>(n)
                    .Select(t => new KeyValuePair<Type, TKey>(t, getKey(t, GetRelativeNamespace(t, n))))))
                );
        }
        private string GetRelativeNamespace(Type type, string fullNamespace) => type.Name;

        #endregion

        /// <summary>
        /// Signals if the application is currently loading and a loading screen should be displayed.
        /// </summary>
        public BoolList IsBusy = new BoolList();

        #region Helper classes
        protected class ViewModelTypeInfo
        {
            public readonly string RelativeNamespace;
            public readonly bool SupportsParameter;
            public readonly bool SupportsModel;

            public ViewModelTypeInfo(string relativeNamespace, bool supportsParameter, bool supportsModel)
            {
                RelativeNamespace = relativeNamespace;
                SupportsParameter = supportsParameter;
                SupportsModel = supportsModel;
            }
        }

        protected class ViewTypeInfo
        {
            public readonly string RelativeNamespace;
            public ViewTypeInfo(string relativeNamespace)
            {
                RelativeNamespace = relativeNamespace;
            }
        }
        #endregion

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


    [Serializable]
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string viewName) : base($"View {viewName} not found.")
        {

        }
        public ViewNotFoundException(string viewName, string message) : base($"View {viewName} not found.", new Exception(message))
        {
        }
        public ViewNotFoundException(string viewName, string message, Exception inner) : base($"View {viewName} not found.", new Exception(message, inner)) { }
        protected ViewNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ViewModelNotFoundException : Exception
    {
        public ViewModelNotFoundException(string viewName) : base($"ViewModel {viewName} not found.")
        {

        }
        public ViewModelNotFoundException(string viewName, string message) : base($"ViewModel {viewName} not found.", new Exception(message))
        {
        }
        public ViewModelNotFoundException(string viewName, string message, Exception inner) : base($"ViewModel {viewName} not found.", new Exception(message, inner)) { }
        protected ViewModelNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


}

