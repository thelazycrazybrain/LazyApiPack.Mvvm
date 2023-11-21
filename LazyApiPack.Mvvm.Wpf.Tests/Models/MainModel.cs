using LazyApiPack.Mvvm.Model.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Tests.Models
{
    public class MainModel : ModelBase
    {
        private string _title = "Main Model";
        public string Title { get => _title; set => SetPropertyValue(ref _title, value); }
    }
}
