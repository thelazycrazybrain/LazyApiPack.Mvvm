using LazyApiPack.Mvvm.Wpf.Showcase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Showcase.Module.Services
{
    public class NetworkService : INetworkService
    {
        public bool IsAlive => true;
    }
}
