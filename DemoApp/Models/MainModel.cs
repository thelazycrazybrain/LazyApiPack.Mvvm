using LazyApiPack.Mvvm.Wpf.Model.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Models {
    public class MainModel : ModelBase{
        string? _content;
        public string? Content { get => _content; set => SetPropertyValue(ref _content, value); }
    }
}
