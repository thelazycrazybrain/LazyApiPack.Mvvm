using LazyApiPack.Mvvm.Debug.Module.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Debug.Module.Services
{
    public interface IMvvmDiagnosticsService
    {
        IEnumerable<ModuleInfo> GetModules();
        IEnumerable<ServiceInfo> GetServices();
        IEnumerable<ViewInfo> GetViews();
        IEnumerable<RegionInfo> GetRegions();
    }

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
