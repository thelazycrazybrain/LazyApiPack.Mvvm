using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Application.Configuration;
using LazyApiPack.Mvvm.Wpf.Regions;

namespace Reporting
{
    public class ReportingModule : MvvmModule
    {
        private string _moduleId = "net.thelazycrazybrain.ReportingModule";
        public override string ModuleId { get => _moduleId; }

        public override void OnModuleSetup(MvvmModuleConfiguration configuration)
        {
            //throw new System.NotImplementedException();
        }

    }
}