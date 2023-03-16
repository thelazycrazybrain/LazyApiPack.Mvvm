using DemoApp.WindowTemplates;
using LazyApiPack.Localization.Wpf;
using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Navigation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LazyApiPack.Localization.Manager;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Services;
using System.Reflection;
using System.IO;
using DemoApp.ViewModels;
using DemoApp.Regions;
using DemoApp.Models;

namespace DemoApp {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvvmApplication {
        protected override void ConfigureRegions(RegionAdapterConfiguration configuration) {
            //throw new NotImplementedException();
        }

        protected override void ConfigureServices(ServiceConfiguration configuration) {
            configuration.Map<ILocalizationService, LocalizationService>(true);
        }

        protected override void OnLocalizationInitialized(ILocalizationService service) {
            LocalizerMarkupExtension.Initialize(service);

        }

        protected override void OnSetup(MvvmAppConfiguration configuration) {
            configuration
                .WithShellWindow<MainWindow>()
                .WithSplashWindow<SplashWindow>()
                .WithViews("DemoApp.Views")
                .WithViewModels("DemoApp.ViewModels")
                .WithWindowTemplates("DemoApp.WindowTemplates")
                .WithLocalizationDirectories(new EnumerationOptions() { RecurseSubdirectories = true }, "*.json", "DemoApp.Localizations");

        }

        protected override void OnStartupComplete() {
            MvvmApp.Navigation.NavigateTo<MainViewModel>(AppRegions.MainView, new MainModel() { Content = "Hi, I am the demo program." });
        }
    }

}
