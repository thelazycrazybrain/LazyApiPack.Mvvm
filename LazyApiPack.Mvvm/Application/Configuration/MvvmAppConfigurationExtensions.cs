using LazyApiPack.Collections.Extensions;
using LazyApiPack.Mvvm.Regions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LazyApiPack.Mvvm.Application.Configuration
{
    /// <summary>
    /// Extens the MvvmAppConfiguration for easy setup.
    /// </summary>
    public static class MvvmAppConfigurationExtensions
    {
        /// <summary>
        /// Adds a shell (main) window to the application. This window provides all regions for navigation.
        /// </summary>
        public static MvvmApplicationConfiguration WithShellWindow<TShellWindow>(this MvvmApplicationConfiguration config) where TShellWindow : IWindowTemplate
        {
            config.ShellWindow = typeof(TShellWindow);
            return config;
        }

        /// <summary>
        /// Adds a start screen (splash screen) to the application that is shown when the application starts.
        /// </summary>
        public static MvvmApplicationConfiguration WithSplashWindow<TSplashWindow>(this MvvmApplicationConfiguration config) where TSplashWindow : ISplashScreenWindowTemplate
        {
            config.SplashScreen = typeof(TSplashWindow);
            return config;
        }

        /// <summary>
        /// Adds a module to the application
        /// </summary>
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

        /// <summary>
        /// Adds a module to the application.
        /// </summary>
        public static MvvmApplicationConfiguration WithModule<TModule>(this MvvmApplicationConfiguration config) where TModule : class
        {
            return config.WithModule(typeof(TModule));
        }

        /// <summary>
        /// Adds directories which contain localization files to the application.
        /// </summary>
        public static MvvmApplicationConfiguration WithLocalizationFiles(this MvvmApplicationConfiguration config, [DisallowNull] IEnumerable<string> localizationFiles)
        {
            config.LocalizationFiles.Upsert(localizationFiles);
            return config;
        }

        /// <summary>
        /// Adds namespaces which contain localization files (Embedded Resources).
        /// </summary>
        public static MvvmApplicationConfiguration WithLocalizationNamespaces(this MvvmApplicationConfiguration config, Assembly assembly, [DisallowNull] IEnumerable<string> localizationNamespaces)
        {
            foreach (var localizationNamespace in localizationNamespaces)
            {
                config.LocalizationNamespaces.Add(new(assembly, localizationNamespace));
            }
            return config;

        }
    }
}