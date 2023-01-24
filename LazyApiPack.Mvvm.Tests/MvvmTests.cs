using Brainstorm.Mvvm;
using LazyApiPack.Mvvm.Tests.Services;
using LazyApiPack.Mvvm.Wpf;

namespace LazyApiPack.Mvvm.Tests
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
            var popupService = AppNavigation.Instance.GetService<IPopupService>();
            Assert.NotNull(popupService);

            var logService = AppNavigation.Instance.GetService<ILogService>();
            Assert.NotNull(logService);

            Assert.Pass();
        }
    }
}