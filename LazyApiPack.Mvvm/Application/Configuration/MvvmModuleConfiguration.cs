using LazyApiPack.Mvvm.Services;
using LazyApiPack.Mvvm.Stores;
using System.Reflection;
namespace LazyApiPack.Mvvm.Application.Configuration
{
    public sealed class MvvmModuleConfiguration
    {
        /// <summary>
        /// List of namespaces to look for viewmodels.
        /// </summary>
        public List<string> ViewModelNamespaces { get; internal set; } = new();
        /// <summary>
        /// List of namespaces to look for views.
        /// </summary>
        public List<string> ViewNamespaces { get; internal set; } = new();
        /// <summary>
        /// List of localization files.
        /// </summary>
        public List<string> LocalizationFiles { get; internal set; } = new();
        /// <summary>
        /// List of namespaces to look for localization files (Embedded Resources).
        /// </summary>
        public List<LocalizationNamespace> LocalizationNamespaces { get; internal set; } = new();
        /// <summary>
        /// List of submodules.
        /// </summary>
        public List<Type> Modules { get; internal set; } = new();
        /// <summary>
        /// List of serivce mappings (Interface / Implementation)
        /// </summary>
        public Dictionary<Type, AppService> ServiceMappings { get; internal set; } = new();
        /// <summary>
        /// List of stores.
        /// </summary>
        public List<Store> Stores { get; internal set; } = new();
        /// <summary>
        /// List of region adapters.
        /// </summary>
        public List<Type> RegionAdapters { get; internal set; } = new();
    }
}