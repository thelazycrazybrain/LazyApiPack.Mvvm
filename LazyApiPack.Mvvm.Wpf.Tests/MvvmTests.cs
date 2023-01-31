using LazyApiPack.Mvvm.Tests.Services;
using LazyApiPack.Mvvm.Wpf.Navigation;

namespace LazyApiPack.Mvvm.Wpf.Tests
{
    public class MvvmTests
    {
        [SetUp]
        public void Setup()
        {
            AppNavigation.Start(new MockServiceMap(),
                new[] { "LazyApiPack.Mvvm.Tests.Views" },
                new[] { "LazyApiPack.Mvvm.Tests.ViewModels" });
        }

        [Test]
        public void TestServiceInjectionWithDependencies()
        {
            var alertService = AppNavigation.Instance.GetService<IAlertService>();
            Assert.NotNull(alertService);
          
            var messageService = AppNavigation.Instance.GetService<IMessageService>();
            Assert.NotNull(messageService);
            messageService.ShowMessage("Test", "Baum");
            var popupService = AppNavigation.Instance.GetService<IPopupService>();
            Assert.NotNull(popupService);

            var logService = AppNavigation.Instance.GetService<ILogService>();
            Assert.NotNull(logService);
            Assert.That(logService.GetLog() == "Baum\nTest\n");
            Assert.Pass();
        }
    }
}