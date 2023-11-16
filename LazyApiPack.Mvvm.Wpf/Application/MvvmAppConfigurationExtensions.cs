using LazyApiPack.Collections.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LazyApiPack.Mvvm.Wpf.Application
{
    public static class MvvmAppConfigurationExtensions
    {
        public static MvvmApplicationConfiguration WithShellWindow<TShellWindow>(this MvvmApplicationConfiguration config) where TShellWindow : IWindowTemplate
        {
            config.ShellWindow = typeof(TShellWindow);
            return config;
        }

        public static MvvmApplicationConfiguration WithSplashWindow<TSplashWindow>(this MvvmApplicationConfiguration config) where TSplashWindow : ISplashScreenWindowTemplate
        {
            config.SplashScreen = typeof(TSplashWindow);
            return config;
        }

        public static MvvmApplicationConfiguration WithModule(this MvvmApplicationConfiguration config, [DisallowNull] Type moduleType)
        {
            if (!typeof(MvvmModule).IsAssignableFrom(moduleType))
            {
                throw new InvalidOperationException($"Can not convert type {moduleType.FullName} to {typeof(MvvmModule).FullName}.");
            }
            if (!config.Modules.Contains(moduleType))
            {
                config.Modules.Add(moduleType);
            }
            return config;
        }
        public static MvvmApplicationConfiguration WithModule<TModule>(this MvvmApplicationConfiguration config) where TModule : class
        {
            return WithModule(config, typeof(TModule));
        }

        /// <summary>
        /// Adds localization directories.
        /// </summary>
        public static MvvmApplicationConfiguration WithLocalizationFiles(this MvvmApplicationConfiguration config, [DisallowNull] IEnumerable<string> localizationFiles)
        {
            config.LocalizationFiles.Upsert(localizationFiles);
            return config;
        }

        public static MvvmApplicationConfiguration WithLocalizationNamespaces(this MvvmApplicationConfiguration config, Assembly assembly, [DisallowNull] IEnumerable<string> localizationNamespaces)
        {
            foreach (var localizationNamespace in localizationNamespaces)
            {
                config.LocalizationNamespaces.Add(new Tuple<Assembly, string>(assembly, localizationNamespace));
            }
            return config;

        }


    }
}