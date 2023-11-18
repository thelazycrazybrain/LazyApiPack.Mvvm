using LazyApiPack.Mvvm.Debug.Module.Services;
using LazyApiPack.Mvvm.Wpf.Application;
using LazyApiPack.Mvvm.Wpf.Model.BaseImpl;
using LazyApiPack.Wpf.Utils.Commands;

namespace LazyApiPack.Mvvm.Debug.Module.Models
{
    public class DiagnosticsViewModel : ViewModelBase<DiagnosticsModel, object>
    {
        private readonly IMvvmDiagnosticsService _diagnosticsService;
        public DiagnosticsViewModel(IMvvmDiagnosticsService diagnosticsService)
        {
            _diagnosticsService = diagnosticsService;
            RefreshDiagnostics();
        }

        RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand { get => _refreshCommand ??= new RelayCommand(OnRefreshCommand_Execute); }
        protected void OnRefreshCommand_Execute(object? parameter)
        {
            RefreshDiagnostics();
        }

        private void RefreshDiagnostics()
        {
            try
            {
                MvvmApplication.Navigation.ShowSplashScreen("Loading diagnostics", null);

                if (Model == null)
                {
                    Model = new();
                }
                Model.Services = _diagnosticsService.GetServices();
                Model.Regions = _diagnosticsService.GetRegions();
                Model.Views = _diagnosticsService.GetViews();
                Model.Modules = _diagnosticsService.GetModules();
            }
            finally
            {
                MvvmApplication.Navigation.HideSplashScreen();
            }
        }
    }
}
