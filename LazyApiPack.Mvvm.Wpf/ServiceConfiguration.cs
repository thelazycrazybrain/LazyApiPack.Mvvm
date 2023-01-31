using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace LazyApiPack.Mvvm.Wpf
{
    public class ServiceConfiguration
    {
        private readonly Dictionary<Type, AppService> _mappings = new Dictionary<Type, AppService>();

       
        public ReadOnlyDictionary<Type, AppService> GetServices() => 
            new ReadOnlyDictionary<Type, AppService>(_mappings);

        public void Map(Type interfaceType, Type serviceType, bool asSingleton = false)
        {
            _mappings.Add(interfaceType,
                new AppService(interfaceType, asSingleton, serviceType));
        }

        public void Map<TInterfaceType, TServiceType>(bool asSingleton = false)
        {
            Map(typeof(TInterfaceType), typeof(TServiceType), asSingleton);
        }

        /// <summary>
        /// Creates a service that is a singleton with a provided instance of the service
        /// </summary>
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="singleton">The singleton instance</param>
        public void Map<TInterfaceType>([NotNull] TInterfaceType singletonInstance)
        {
            _mappings.Add(typeof(TInterfaceType),
                new AppService(typeof(TInterfaceType), singletonInstance));
        }

        /// <summary>
        /// Creates a service mapping that is not a singleton with a method that generates the service.
        /// </summary>
        /// <typeparam name="IInterfaceType">Type of the service.</typeparam>
        /// <param name="createInstance">Method that returns the instance of the service.</param>
        public void Map<TInterfaceType>(bool asSingleton, Func<object> createInstance)
        {
            _mappings.Add(typeof(TInterfaceType),
                new AppService(typeof(TInterfaceType), asSingleton, createInstance));
        }


    }
}