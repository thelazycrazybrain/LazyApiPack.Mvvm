using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Windows;
using LazyApiPack.Collections.Extensions;
using LazyApiPack.Mvvm.Regions;
using LazyApiPack.Mvvm.Services;
using LazyApiPack.Mvvm.Stores;

namespace LazyApiPack.Mvvm.Application.Configuration
{
    /// <summary>
    /// Extends the MvvmModuleConfiguration for easy setup.
    /// </summary>
    public static class MvvmModuleConfigurationExtensions
    {
        /// <summary>
        /// Adds a list of namespaces to look for viewmodels
        /// </summary>
        public static MvvmModuleConfiguration WithViewModels(this MvvmModuleConfiguration config, [DisallowNull] params string[] viewModelNamespaces)
        {
            return config.WithViewModels(viewModelNamespaces.AsEnumerable());
        }

        /// <summary>
        /// Adds a list of namespaces to look for viewmodels
        /// </summary>
        public static MvvmModuleConfiguration WithViewModels(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> viewModelNamespaces)
        {
            config.ViewModelNamespaces.Upsert(viewModelNamespaces);
            return config;
        }

        /// <summary>
        /// Adds a list of namespaces to look for views
        /// </summary>
        public static MvvmModuleConfiguration WithViews(this MvvmModuleConfiguration config, [DisallowNull] params string[] viewNamespaces)
        {
            return config.WithViews(viewNamespaces.AsEnumerable());
        }

        /// <summary>
        /// Adds a list of namespaces to look for views
        /// </summary>
        public static MvvmModuleConfiguration WithViews(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> viewNamespaces)
        {
            config.ViewNamespaces.Upsert(viewNamespaces);
            return config;
        }

        /// <summary>
        /// Adds a list of localization files.
        /// </summary>
        public static MvvmModuleConfiguration WithLocalizationFiles(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> localizationFiles)
        {
            config.LocalizationFiles.Upsert(localizationFiles);
            return config;
        }

        /// <summary>
        /// Adds a list of namespaces to look for localization files (Embedded Resources).
        /// </summary>
        /// <param name="assembly">The assembly containing the localization files.</param>
        /// <param name="localizationNamespaces">The namespace of the localization files.</param>
        /// <returns></returns>
        public static MvvmModuleConfiguration WithLocalizationNamespaces(this MvvmModuleConfiguration config, Assembly assembly, [DisallowNull] IEnumerable<string> localizationNamespaces)
        {
            foreach (var localizationNamespace in localizationNamespaces)
            {
                config.LocalizationNamespaces.Add(new(assembly, localizationNamespace));
            }
            return config;

        }

        /// <summary>
        /// Adds a submodule to the application.
        /// </summary>
        /// <exception cref="InvalidOperationException">The module is not derived from MvvmModule.</exception>
        public static MvvmModuleConfiguration WithModule(this MvvmModuleConfiguration config, [DisallowNull] Type moduleType)
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
        /// Adds a submodule to the application.
        /// </summary>
        /// <exception cref="InvalidOperationException">The module is not derived from MvvmModule.</exception>
        public static MvvmModuleConfiguration WithModule<TModule>(this MvvmModuleConfiguration config) where TModule : class
        {
            return config.WithModule(typeof(TModule));
        }

        /// <summary>
        /// Adds a service to the application.
        /// </summary>
        /// <param name="interfaceType">Interface, used for dependency injection.</param>
        /// <param name="serviceType">Implementation of the service.</param>
        /// <param name="asSingleton">Indicates that the service has only one instance.</param>
        public static MvvmModuleConfiguration WithService(this MvvmModuleConfiguration config, Type interfaceType, Type serviceType, bool asSingleton = false)
        {
            config.ServiceMappings.Add(interfaceType, new AppService(interfaceType, asSingleton, serviceType));
            return config;
        }

        /// <summary>
        /// Adds a service to the application.
        /// </summary>
        /// <typeparam name="TInterfaceType">Interface, used for dependency injection.</typeparam>
        /// <typeparam name="TServiceType">Implementation of the service.</typeparam>
        /// <param name="asSingleton">Indicates that the service has only one instance.</param>
        public static MvvmModuleConfiguration WithService<TInterfaceType, TServiceType>(this MvvmModuleConfiguration config, bool asSingleton = false)
        {
            return config.WithService(typeof(TInterfaceType), typeof(TServiceType), asSingleton);
        }

        /// <summary>
        /// Creates a service that is a singleton with a provided instance of the service
        /// </summary>
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="singleton">The singleton instance</param>
        public static MvvmModuleConfiguration WithService<TInterfaceType>(this MvvmModuleConfiguration config, [DisallowNull] TInterfaceType singletonInstance)
        {
            config.ServiceMappings.Add(typeof(TInterfaceType),
                new AppService(typeof(TInterfaceType), singletonInstance));
            return config;
        }

        /// <summary>
        /// Creates a service mapping that is not a singleton with a method that generates the service.
        /// </summary>
        /// <typeparam name="IInterfaceType">Type of the service.</typeparam>
        /// <param name="createInstance">Method that returns the instance of the service.</param>
        public static MvvmModuleConfiguration WithService<TInterfaceType>(this MvvmModuleConfiguration config, bool asSingleton, Func<object> createInstance)
        {
            config.ServiceMappings.Add(typeof(TInterfaceType),
                new AppService(typeof(TInterfaceType), asSingleton, createInstance));
            return config;
        }

        /// <summary>
        /// Adds a store to the application.
        /// </summary>
        /// <typeparam name="TStore">Type of the store.</typeparam>
        public static MvvmModuleConfiguration WithStore<TStore>(this MvvmModuleConfiguration config) where TStore : class
        {
            config.Stores.Add(new Store<TStore>());
            return config;
        }
        /// <summary>
        /// Adds a store to the application.
        /// </summary>
        /// <typeparam name="TStore">Type of the store.</typeparam>
        /// <param name="singleton">The concrete store object.</param>
        public static MvvmModuleConfiguration WithStore<TStore>(this MvvmModuleConfiguration config, TStore singleton) where TStore : class
        {
            config.Stores.Add(new Store<TStore>(singleton));
            return config;
        }

        /// <summary>
        /// Adds a region adapter to the application.
        /// </summary>
        public static MvvmModuleConfiguration WithRegionAdapter
            <TAdapter>(this MvvmModuleConfiguration config) where TAdapter : IRegionAdapter
        {
            config.RegionAdapters.Add(typeof(TAdapter));
            return config;
        }

    }
}