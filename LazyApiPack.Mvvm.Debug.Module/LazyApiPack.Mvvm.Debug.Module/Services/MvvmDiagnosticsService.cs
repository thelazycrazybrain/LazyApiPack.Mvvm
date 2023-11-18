using LazyApiPack.Mvvm.Debug.Module.Models;

namespace LazyApiPack.Mvvm.Debug.Module.Services
{
    public class MvvmDiagnosticsService : IMvvmDiagnosticsService
    {
        public MvvmDiagnosticsService()
        {

        }

        public IEnumerable<ServiceInfo> GetServices()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ModuleInfo> GetModules()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ViewInfo> GetViews()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RegionInfo> GetRegions()
        {
            throw new NotImplementedException();
        }
    }
}
