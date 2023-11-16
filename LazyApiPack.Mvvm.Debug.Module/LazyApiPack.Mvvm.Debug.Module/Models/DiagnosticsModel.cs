using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Debug.Module.Models
{
    public class DiagnosticsModel
    {
        public IEnumerable<ServiceInfo> Services { get; set; }
        public IEnumerable<ModuleInfo> Modules { get; set; }
        public IEnumerable<ViewInfo> Views { get; set; }
        public IEnumerable<RegionInfo> Regions { get; set; }
    }
}
