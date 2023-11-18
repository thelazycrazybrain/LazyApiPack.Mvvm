using LazyApiPack.Mvvm.Debug.Module.Models;

namespace LazyApiPack.Mvvm.Debug.Module.Services
{
    public interface IMvvmDiagnosticsService
    {
        IEnumerable<ModuleInfo> GetModules();
        IEnumerable<ServiceInfo> GetServices();
        IEnumerable<ViewInfo> GetViews();
        IEnumerable<RegionInfo> GetRegions();
    }
}
