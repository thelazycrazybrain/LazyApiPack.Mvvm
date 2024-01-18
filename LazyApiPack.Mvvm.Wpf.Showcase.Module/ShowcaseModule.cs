using LazyApiPack.Localization;
using LazyApiPack.Localization.Manager;
using LazyApiPack.Logger;
using LazyApiPack.Logger.Loggers;
using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Application.Configuration;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters;
using LazyApiPack.Mvvm.Services;
using LazyApiPack.Wpf.Controls.Navigation;
using LazyApiPack.Mvvm.Wpf.Adapters;
using LazyApiPack.Mvvm.Showcaseworks.Stores;
using LazyApiPack.Mvvm.Showcase.Module.Services;
using LazyApiPack.Mvvm.Wpf.Showcase.Interfaces;
using IronPython.Module;

namespace LazyApiPack.Mvvm.Wpf.Showcase.Module
{
    public class ShowcaseModule : MvvmModule
    {
        private string _moduleId = "net.thelazycrazybrain.LazyApiPack.Mvvm.Wpf.Showcase.Module.ShowcaseModule";
        public override string ModuleId { get => _moduleId; }

        public override void OnModuleSetup(MvvmModuleConfiguration configuration)
        {
            configuration
                .WithStore(new SolutionStore())
                .WithModule<IronPythonModule>()

                .WithViews("LazyApiPack.Mvvm.Wpf.Showcase.Module.Views", "LazyApiPack.Mvvm.Wpf.Showcase.Module.SidebarViews")
                .WithViewModels("LazyApiPack.Mvvm.Wpf.Showcase.Module.ViewModels", "LazyApiPack.Mvvm.Wpf.Showcase.Module.SidebarViewModels")

                .WithLocalizationNamespaces(typeof(ShowcaseModule).Assembly, new[] { "LazyApiPack.Mvvm.Wpf.Showcase.Module.Localizations" })

                .WithService<ILocalizationService, LocalizationService>()
                .WithService<ILogger, FileLogger>()
                .WithService<INetworkService, NetworkService>(true);

        }
        public override void OnModuleSetupComplete()
        {
            MvvmApplication.Instance.NavigateTo("InterpreterViewModel", "ModalRegion", null, null);
            MvvmApplication.Instance.NavigateTo("DebugViewModel", "Main", null, null);
            MvvmApplication.Instance.NavigateTo("DebugViewModel", "Main", null, null);
            MvvmApplication.Instance.NavigateTo("DebugViewModel", "Main", null, null);
            MvvmApplication.Instance.NavigateTo("DebugViewModel", "Main", null, null);

        }
    }
}