using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Application.Configuration;
using LazyApiPack.Mvvm.Tests.Services;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Tests.Models;
using LazyApiPack.Mvvm.Wpf.Tests.ViewModels.Main;
using LazyApiPack.Mvvm.Wpf.Tests.WindowTemplates;
using System.IO;
using System.Reflection;

namespace LazyApiPack.Mvvm.Wpf.Tests {

    public class MvvmApp : MvvmApplication
    {
        protected override void OnSetup(MvvmApplicationConfiguration configuration)
        {
            base.OnSetup(configuration);// TODO:
        }
    }
    [Apartment(ApartmentState.STA)]
    public class MvvmTests {
        [SetUp]

        public void Setup() {
            var app = new MvvmApp();
            app.Setup();
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
            var alertService = MvvmApplication.Instance.GetService<IAlertService>();
            Assert.NotNull(alertService);

            var messageService = MvvmApplication.Instance.GetService<IMessageService>();
            Assert.NotNull(messageService);
            messageService.ShowMessage("Test", "Baum");
            var popupService = MvvmApplication.Instance.GetService<IPopupService>();
            Assert.NotNull(popupService);

            var logService = MvvmApplication.Instance.GetService<ILogService>();
            Assert.NotNull(logService);
            Assert.That(logService.GetLog() == "Baum\nTest\n");
            Assert.Pass();
        }


        [Test]
        public void TestMvvmClassResolving() {
            //var viewType = MvvmApplication.Instance.GetAssociatedView(typeof(MainViewModel));
            //Assert.NotNull(viewType);


            var viewModelInstance = MvvmApplication.Instance.CreateObjectWithDependencyInjection(typeof(MainViewModel));
            Assert.NotNull(viewModelInstance);
            //var viewInstance = MvvmApplication.Instance.CreateObjectWithDependencyInjection(viewType);
            //Assert.NotNull(viewInstance);


            var svc = MvvmApplication.Instance.GetService(typeof(ILogService));
            Assert.NotNull(svc);
            Assert.Pass();

        }
        [Test]
        public void TestNavigation() {
            var model = new MainModel();
            var parameter = "Hello world";
            var viewModel = MvvmApplication.Instance.NavigateTo<MainViewModel>("", model, parameter);
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