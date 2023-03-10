using LazyApiPack.Mvvm.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Tests.Services
{
    public class MockServiceMap : ServiceMap
    {
        public override void ConfigureServices(ServiceConfiguration configuration)
        {
            configuration.Map<ILogService, LogServiceMock>(true); // Singleton autogenerated instance
            configuration.Map<IAlertService>(false, () => new AlertServiceMock()); // Function instance
            configuration.Map<IMessageService, MessageServiceMock>(); // Mutliple instances dependent on ILogService
            configuration.Map<IPopupService>(new PopupServiceMock()); // Singleton with provided object
        }
    }
}
