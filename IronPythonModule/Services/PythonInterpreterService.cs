using LazyApiPack.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPython.Module.Services
{
    public interface IPythonInterpreterService
    {
        string ExecuteCommand(string command);
    }
    public class PythonInterpreterService : IPythonInterpreterService
    {
        private readonly ILogger _logger;
        public PythonInterpreterService(ILogger logger)
        {
            _logger = logger;
        }
        public string ExecuteCommand(string command)
        {
            return "Dummy";
        }
    }
}
