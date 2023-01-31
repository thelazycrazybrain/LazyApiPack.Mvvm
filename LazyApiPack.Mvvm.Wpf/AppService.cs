using System.Diagnostics.CodeAnalysis;

namespace LazyApiPack.Mvvm.Wpf
{
    public class AppService : IDisposable
    {
        private readonly Type _interfaceType;
        /// <summary>
        /// The service interface this service implements.
        /// </summary>
        public Type InterfaceType { get => _interfaceType; }

        private readonly bool _isSingleton;
        /// <summary>
        /// If the service is a singleton, the service object is cached in this class once it is needed the first time.
        /// </summary>
        public bool IsSingleton { get => _isSingleton; }
        /// <summary>
        /// Creates a instance of the service with a specified method.
        /// </summary>
        private readonly Func<object> _createInstance;

        private object? _singletonInstance;

        private readonly Type? _implementationType;

        public AppService([NotNull] Type interfaceType, bool isSingleton,
                          [NotNull] Type implementationType)
        {
            _interfaceType=interfaceType;
            _isSingleton = isSingleton;
            _implementationType=implementationType;
            _createInstance = CreateServiceInstance;
        }

        public AppService([NotNull] Type interfaceType, bool isSingleton,
                          [NotNull] Func<object> createInstance)
        {
            _interfaceType = interfaceType;
            _isSingleton = isSingleton;
            _createInstance = createInstance;

        }

        public AppService([NotNull] Type interfaceType, [NotNull] object singletonInstance)
        {
            _interfaceType = interfaceType;
            _isSingleton = true;
            _createInstance = () => singletonInstance;

        }

        private object CreateServiceInstance()
        {
            if (_implementationType == null)
            {
                throw new InvalidOperationException(
                    $"Can not create a default instance of the service because the implementation type is unknown.");
            }

            return AppNavigation.Instance.CreateObjectWithDependencyInjection(_implementationType);

        }

        /// <summary>
        /// Gets a new instance of a service or the service instance, if the service is a signleton
        /// </summary>
        public object GetInstance()
        {
            if (_isSingleton)
            {
                return _singletonInstance ??= _createInstance();
            }
            else
            {
                return _createInstance();
            }
        }

        /// <summary>
        /// Gets a new instance of a service or the service instance, if the service is a signleton
        /// </summary>
        public TServiceType GetInstance<TServiceType>()
        {
            return (TServiceType)GetInstance();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~AppService()
        {
            Dispose(false);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (_singletonInstance is IDisposable sd)
            {
                sd.Dispose();
            }
            _singletonInstance = null;
        }
    }
}