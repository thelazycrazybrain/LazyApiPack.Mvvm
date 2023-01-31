using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Tests.Services
{
    public interface ILogService
    {
        void Log(string message);
        string GetLog();
    }

    public class LogServiceMock : ILogService
    {
        public LogServiceMock()
        {

        }

        string _log;
        public void Log(string message)
        {
            _log += message + "\n";
        }

        public string GetLog()
        {
            return _log;
        }
    }
}
