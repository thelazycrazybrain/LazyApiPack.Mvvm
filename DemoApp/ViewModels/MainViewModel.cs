using DemoApp.Models;
using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Wpf.Model.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.ViewModels {
    public class MainViewModel : ViewModelBase<MainModel, object> {
        private readonly ILocalizationService _localizationService;
        public MainViewModel(ILocalizationService localizationService) {
            _localizationService = localizationService;
        }
        protected override void OnParameterChanged(object? parameter) {
            base.OnParameterChanged(parameter);
        }
    }
}
