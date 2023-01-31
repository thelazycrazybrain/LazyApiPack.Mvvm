using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Tests.Services
{
    public interface IAlertService
    {
        void SendAlert(string alert);
    }
    public class AlertServiceMock : IAlertService
    {
        public AlertServiceMock()
        {

        }

        public void SendAlert(string alert)
        {
            Debug.WriteLine("AlertServiceMock: " + alert);
        }
    }
}
