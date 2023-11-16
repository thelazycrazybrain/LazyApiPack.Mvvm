using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Services;

namespace LazyApiPack.Mvvm.Wpf.Application
{
    public abstract class MvvmModule
    {
        public abstract string ModuleId { get; }
        public MvvmApplication Application { get; private set; }
        internal MvvmModuleConfiguration Configuration { get; private set; }
        /// <summary>
        /// Contains a list of modules, this module is used by. A null entry means, this module is part of the application itself (Root Module).
        /// </summary>
        internal List<MvvmModule?> ParentModules { get; private set; } = new List<MvvmModule?>();
        internal void Setup(MvvmApplication application, MvvmModule? parentModule)
        {
            Application = application;
            
            ParentModules.Add(parentModule);
            Configuration = new MvvmModuleConfiguration();
            OnSetup(Configuration);
            // TODO: Create module and pass information back to MvvmApplication
            // IMPORTANT: If module uses submodules, pass the submodule to MvvmApllication back
        }

        public abstract void OnModuleLoaded();
        public abstract void OnActivated();
        public abstract void OnDeactivated();
        public abstract void OnModuleUnloaded();
        public abstract void OnSetup(MvvmModuleConfiguration configuration);
        public abstract void OnLocalizationInitialized(ILocalizationService service);
        public abstract void OnMessageReceived(MvvmModule sender, string? moduleId, string messageId, object message);
    }
}