using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Application.Configuration;
using LazyApiPack.Mvvm.Wpf.Adapters;
using LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters;
using LazyApiPack.Mvvm.Wpf.Showcase;
using LazyApiPack.Mvvm.Wpf.Showcase.Module;

namespace LazyApiPack.Mvvm.Wpf.Showcase
{
    public class MvvmApp : MvvmApplication
    {
        public override string ModuleId => "Showcase Application";

        protected override void OnSetup(MvvmApplicationConfiguration configuration)
        {
            configuration
                .WithModule<ShowcaseModule>()
                .WithShellWindow<MainWindow>()
                .WithSplashWindow<SplashWindow>();


        }

        public override void OnModuleSetup(MvvmModuleConfiguration configuration)
        {
            configuration
                .WithRegionAdapter<MultiWindowRegionAdapter>()
                .WithRegionAdapter<SidebarControlRegionAdapter>()
                .WithRegionAdapter<ContentControlRegionAdapter>()
                //.WithRegionAdapter<TabControlRegionAdapter>()
                .WithRegionAdapter<CloseableTabControlRegionAdapter>();
        }

    }
}
