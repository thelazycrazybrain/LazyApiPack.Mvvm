using Brainstorm.Mvvm;
using Brainstorm.Mvvm.Wpf;
using LazyApiPack.Mvvm.Tests.Services;

namespace LazyApiPack.Mvvm.Wpf.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            AppNavigation.Start(new MockServiceMap(),
                new[] { "LazyApiPack.Mvvm.Tests.Views" },
                new[] { "LazyApiPack.Mvvm.Tests.ViewModels" });
        }

        [Test]
        public void Test1()
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