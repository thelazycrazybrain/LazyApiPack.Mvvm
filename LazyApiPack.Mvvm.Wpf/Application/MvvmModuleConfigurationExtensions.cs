using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Windows;
using LazyApiPack.Collections.Extensions;
using LazyApiPack.Mvvm.Wpf.Regions;
using LazyApiPack.Mvvm.Wpf.Services;
using LazyApiPack.Mvvm.Wpf.Stores;

namespace LazyApiPack.Mvvm.Wpf.Application
{
    public static class MvvmModuleConfigurationExtensions
    {
        public static MvvmModuleConfiguration WithViewModels(this MvvmModuleConfiguration config, [DisallowNull] params string[] viewModelNamespaces)
        {
            return WithViewModels(config, viewModelNamespaces.AsEnumerable());
        }

        public static MvvmModuleConfiguration WithViewModels(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> viewModelNamespaces)
        {
            config.ViewModelNamespaces.Upsert(viewModelNamespaces);
            return config;
        }

        public static MvvmModuleConfiguration WithViews(this MvvmModuleConfiguration config, [DisallowNull] params string[] viewNamespaces)
        {
            return WithViews(config, viewNamespaces.AsEnumerable());
        }

        public static MvvmModuleConfiguration WithViews(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> viewNamespaces)
        {
            config.ViewNamespaces.Upsert(viewNamespaces);
            return config;
        }

        public static MvvmModuleConfiguration WithWindowTemplates(this MvvmModuleConfiguration config, [DisallowNull] params string[] windowTemplateNamespaces)
        {
            return WithWindowTemplates(config, windowTemplateNamespaces.AsEnumerable());

        }

        public static MvvmModuleConfiguration WithWindowTemplates(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> windowTemplateNamespaces)
        {
            config.WindowTemplateNamespaces.Upsert(windowTemplateNamespaces);
            return config;
        }
        /// <summary>
        /// Adds localization directories.
        /// </summary>
        public static MvvmModuleConfiguration WithLocalizationFiles(this MvvmModuleConfiguration config, [DisallowNull] IEnumerable<string> localizationFiles)
        {
            config.LocalizationFiles.Upsert(localizationFiles);
            return config;
        }

        public static MvvmModuleConfiguration WithLocalizationNamespaces(this MvvmModuleConfiguration config, Assembly assembly, [DisallowNull] IEnumerable<string> localizationNamespaces)
        {
            foreach (var localizationNamespace in localizationNamespaces)
            {
                config.LocalizationNamespaces.Add(new Tuple<Assembly, string>(assembly, localizationNamespace)) ;
            }
            return config;
          
        }
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
        public static MvvmModuleConfiguration WithModule<TModule>(this MvvmModuleConfiguration config) where TModule : class
        {
            return WithModule(config, typeof(TModule));
        }


        public static MvvmModuleConfiguration WithService(this MvvmModuleConfiguration config, Type interfaceType, Type serviceType, bool asSingleton = false)
        {
            config.ServiceMappings.Add(interfaceType, new AppService(interfaceType, asSingleton, serviceType));
            return config;
        }

        public static MvvmModuleConfiguration WithService<TInterfaceType, TServiceType>(this MvvmModuleConfiguration config, bool asSingleton = false)
        {
            return WithService(config, typeof(TInterfaceType), typeof(TServiceType), asSingleton);
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

        public static MvvmModuleConfiguration WithStore<T>(this MvvmModuleConfiguration config) where T : class
        {
            config.Stores.Add(new Store<T>());
            return config;
        }

        public static MvvmModuleConfiguration WithStore<T>(this MvvmModuleConfiguration config, T singleton) where T : class
        {
            config.Stores.Add(new Store<T>(singleton));
            return config;
        }

        public static MvvmModuleConfiguration WithRegionAdapter
            <TAdapter>(this MvvmModuleConfiguration config) where TAdapter : IRegionAdapter
        {
            config.RegionAdapters.Add( typeof(TAdapter));
            return config;
        }

    }
}