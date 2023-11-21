using LazyApiPack.Mvvm.Application;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazyApiPack.Mvvm.Regions
{

    public interface IRegionManager
    {
        void Initialize(ref List<IRegionAdapter> regionAdapters);
        void NavigateTo(object view, string regionName, bool isModal);
    }
    
}
