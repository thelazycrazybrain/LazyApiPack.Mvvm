using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Services;
using LazyApiPack.Mvvm.Wpf.Stores;
using System.Reflection;

namespace LazyApiPack.Mvvm.Wpf.Application
{
    public sealed class MvvmModuleConfiguration
    {
        public List<string> ViewModelNamespaces { get; internal set; } = new();
        public List<string> ViewNamespaces { get; internal set; } = new();
        public List<string> WindowTemplateNamespaces { get; internal set; } = new();
        public List<string> LocalizationFiles { get; internal set; } = new();
        public List<Tuple<Assembly, string>> LocalizationNamespaces { get; internal set; } = new();
        public List<Type> Modules { get; internal set; } = new();
        public Dictionary<Type, AppService> ServiceMappings { get; internal set; } = new();
        public List<Store> Stores { get; internal set; } = new();
        public List<Type> RegionAdapters { get; internal set; } = new();
    }
}