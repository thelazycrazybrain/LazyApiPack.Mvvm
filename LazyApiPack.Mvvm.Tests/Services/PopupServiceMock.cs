using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Tests.Services
{
    public interface IPopupService
    {
        void ShowPopup(string message);
    }
    public class PopupServiceMock : IPopupService
    {
        public void ShowPopup(string message)
        {
            Debug.WriteLine("PopupServiceMock: " + message);
        }
    }
}
