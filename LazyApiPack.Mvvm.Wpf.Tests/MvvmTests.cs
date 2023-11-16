using LazyApiPack.Mvvm.Tests.Services;
using LazyApiPack.Mvvm.Wpf.Application;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Tests.Models;
using LazyApiPack.Mvvm.Wpf.Tests.ViewModels.Main;
using LazyApiPack.Mvvm.Wpf.Tests.WindowTemplates;
using System.IO;
using System.Reflection;

namespace LazyApiPack.Mvvm.Wpf.Tests {
    [Apartment(ApartmentState.STA)]
    public class MvvmTests {
        [SetUp]

        public void Setup() {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var _lPath = Path.Combine(root, "Localizations");
            throw new NotImplementedException("This test needs rework since the bootstrapping logic of the Mvvm framework has changed with update 0.0.2.");
            //MvvmApplication.Start(new MockServiceMap(), 
            //    new MockRegionAdapterMap(), new[] { "LazyApiPack.Mvvm.Localizations"}, "en", null, 
            //    new[] { "LazyApiPack.Mvvm.Wpf.Tests.Views" },
            //    new[] { "LazyApiPack.Mvvm.Wpf.Tests.ViewModels" },
            //    new[] { "LazyApiPack.Mvvm.Wpf.Tests.WindowTemplates" },
            //    typeof(ApplicationShell), typeof(SplashScreen)
            //    );
        }

        [Test]
        public void TestServiceInjectionWithDependencies() {
            var alertService = MvvmApp.Navigation.GetService<IAlertService>();
            Assert.NotNull(alertService);

            var messageService = MvvmApp.Navigation.GetService<IMessageService>();
            Assert.NotNull(messageService);
            messageService.ShowMessage("Test", "Baum");
            var popupService = MvvmApp.Navigation.GetService<IPopupService>();
            Assert.NotNull(popupService);

            var logService = MvvmApp.Navigation.GetService<ILogService>();
            Assert.NotNull(logService);
            Assert.That(logService.GetLog() == "Baum\nTest\n");
            Assert.Pass();
        }


        [Test]
        public void TestMvvmClassResolving() {
            var viewType = MvvmApp.Navigation.GetAssociatedView(typeof(MainViewModel));
            Assert.NotNull(viewType);


            var viewModelInstance = MvvmApp.Navigation.CreateObjectWithDependencyInjection(typeof(MainViewModel));
            Assert.NotNull(viewModelInstance);
            var viewInstance = MvvmApp.Navigation.CreateObjectWithDependencyInjection(viewType);
            Assert.NotNull(viewInstance);


            var svc = MvvmApp.Navigation.GetService(typeof(ILogService));
            Assert.NotNull(svc);
            Assert.Pass();

        }
        [Test]
        public void TestNavigation() {
            var model = new MainModel();
            var parameter = "Hello world";
            var viewModel = MvvmApp.Navigation.NavigateTo<MainViewModel>("", model, parameter);
            Assert.NotNull(viewModel);
            Assert.That(viewModel.Parameter, Is.EqualTo(parameter));
            Assert.That(viewModel.Model, Is.EqualTo(model));
            Assert.Pass();
        }

        [Test]
        public void TestRegion() {
            //var wdw = new MainWindowTemplate();
            //wdw.Show();
            //RegionManager.DoStuff(wdw.TAB);

        }
    }
}