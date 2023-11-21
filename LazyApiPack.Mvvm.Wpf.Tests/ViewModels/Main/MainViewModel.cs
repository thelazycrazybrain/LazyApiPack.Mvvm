using LazyApiPack.Mvvm.Model.BaseImpl;
using LazyApiPack.Mvvm.Wpf.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Tests.ViewModels.Main
{
    public class MainViewModel : ViewModelBase<MainModel, string>
    {
        protected override void OnModelChanged(MainModel? model)
        {
        }

        protected override void OnParameterChanged(string? parameter)
        {
            
        }

    }
}
