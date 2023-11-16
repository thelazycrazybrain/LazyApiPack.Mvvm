using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Stores
{
    public class Store
    {
        public Store(Type storeType)
        {
            IsSingleton = false;
            StoreType = storeType;
        }
        public Store([DisallowNull] object singleton)
        {
            IsSingleton = true;
            _singleton = new WeakReference<object?>(singleton);
            StoreType = singleton.GetType();
        }

        protected Type StoreType { get; set; }
        protected WeakReference<object?> _singleton;
        protected bool _isSingleton;
        public bool IsSingleton
        {
            get => _isSingleton;
            set
            {
                if (_isSingleton && !value)
                {
                    _singleton = null;
                }
            }
        }

        public virtual object GetInstance()
        {
            WeakReference<object?> instance = null;
            if (IsSingleton)
            {
                if (_singleton == null)
                {
                    instance = _singleton = new WeakReference<object?>(Activator.CreateInstance(StoreType));
                }
                else
                {
                    instance = _singleton;
                }
            }
            else
            {
                instance = new WeakReference<object?>(Activator.CreateInstance(StoreType));
            }

            if (instance.TryGetTarget(out var result))
            {
                return result;
            }
            else
            {
                throw new ObjectDisposedException("The Store has already been disposed.");
            }
        }
    }
    public class Store<T> : Store
        where T : class
    {
        public Store() : base(typeof(T)) { }
        public Store([DisallowNull] T singleton) : base(singleton) { }

        public override T GetInstance()
        {
            return (T)base.GetInstance();
        }

    }
}
