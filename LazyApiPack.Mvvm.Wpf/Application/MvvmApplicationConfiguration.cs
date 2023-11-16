using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LazyApiPack.Mvvm.Wpf.Application
{
    public sealed class MvvmApplicationConfiguration
    {

        public Type ShellWindow { get; internal set; }
        public Type SplashScreen { get; internal set; }
        public List<Type> Modules { get; internal set; } = new();
        public List<string> LocalizationFiles { get; internal set; } = new();
        public List<Tuple<Assembly, string>> LocalizationNamespaces { get; internal set; } = new();
       
    }
}