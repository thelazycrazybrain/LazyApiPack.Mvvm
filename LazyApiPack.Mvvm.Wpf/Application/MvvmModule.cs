using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Application.Configuration;

namespace LazyApiPack.Mvvm.Wpf.Application
{
    public abstract class MvvmModule
    {
        /// <summary>
        /// Id of the module (eg. net.thelazycrazybrain.MyModule)
        /// </summary>
        public abstract string ModuleId { get; }
        /// <summary>
        /// Instance of the application.
        /// </summary>
        public MvvmApplication Application { get; private set; }
        /// <summary>
        /// The configuration (Initialized at OnSetup)
        /// </summary>
        internal MvvmModuleConfiguration Configuration { get; private set; }
        /// <summary>
        /// Contains a list of modules, this module is used by. A null entry means, this module is part of the application itself (Root Module).
        /// </summary>
        internal List<MvvmModule?> ParentModules { get; private set; } = new List<MvvmModule?>();

        /// <summary>
        /// Scaffolds the module.
        /// </summary>
        /// <param name="application">Current application.</param>
        /// <param name="parentModule">Parent module.</param>
        internal void Setup(MvvmApplication application, MvvmModule? parentModule)
        {
            Application = application;
            ParentModules.Add(parentModule);
            Configuration = new MvvmModuleConfiguration();
            OnSetup(Configuration);
        }

        /// <summary>
        /// Indicates that the module is completely loaded, but the application is not.
        /// </summary>
        public virtual void OnModuleLoaded() { }
        /// <summary>
        /// Indicates that the module was activated.
        /// </summary>
        public virtual void OnActivated() { }
        ///// <summary>
        ///// Indicates that the module was deactivated.
        ///// </summary>
        //public virtual void OnDeactivated() { }
        ///// <summary>
        ///// Indicates that the module was unloaded and should not do any work anymore.
        ///// </summary>
        //public virtual void OnModuleUnloaded() { }
        /// <summary>
        /// Scaffolds the module.
        /// </summary>
        public abstract void OnSetup(MvvmModuleConfiguration configuration);
        /// <summary>
        /// Indicates that the application setup has completed and the application is ready.
        /// </summary>
        public virtual void OnSetupComplete() { }
        /// <summary>
        /// Indicates that the module can now use localization.
        /// </summary>
        public virtual void OnLocalizationInitialized(ILocalizationService service) { }
        /// <summary>
        /// Is invoked when another module or the application sends a message to this module.
        /// </summary>
        /// <param name="sender">Sender of the message.</param>
        /// <param name="moduleId">Module Id of the sender.</param>
        /// <param name="messageId">Id of the message (custom).</param>
        /// <param name="message">Payload.</param>
        public virtual void OnMessageReceived(MvvmModule sender, string? moduleId, string messageId, object message) { }
    }
}