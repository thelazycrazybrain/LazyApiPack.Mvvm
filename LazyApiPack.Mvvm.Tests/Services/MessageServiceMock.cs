using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Tests.Services
{
    public interface IMessageService
    {
        void ShowMessage(string message, string caption);
    }
    public class MessageServiceMock : IMessageService
    {
        private readonly ILogService _logService;
        public MessageServiceMock(ILogService logService)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public void ShowMessage(string message, string caption)
        {
            _logService.Log(caption);
            _logService.Log(message);

        }
    }
}
