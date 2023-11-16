using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Application;

namespace LazyApiPack.Mvvm.Debug.Module
{
    public class MvvmDebugModule : MvvmModule
    {
        public override string ModuleId => throw new NotImplementedException();

        public override void OnActivated()
        {
            throw new NotImplementedException();
        }

        public override void OnDeactivated()
        {
            throw new NotImplementedException();
        }

        public override void OnLocalizationInitialized(ILocalizationService service)
        {
            throw new NotImplementedException();
        }

        public override void OnMessageReceived(MvvmModule sender, string? moduleId, string messageId, object message)
        {
            throw new NotImplementedException();
        }

        public override void OnModuleLoaded()
        {
            throw new NotImplementedException();
        }

        public override void OnModuleUnloaded()
        {
            throw new NotImplementedException();
        }

        public override void OnSetup(MvvmModuleConfiguration configuration)
        {
            throw new NotImplementedException();
        }
    }
}