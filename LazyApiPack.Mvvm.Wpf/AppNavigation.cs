using Brainstorm.Mvvm;
using System;

namespace LazyApiPack.Mvvm.Wpf
{
    public class AppNavigation : MvvmNavigation
    {
        protected AppNavigation(ServiceMap serviceMap, string[] viewNamespaces, string[] viewModelNamespaces) :
            base(serviceMap, viewNamespaces, viewModelNamespaces)
        {
        }

        public static void Start(ServiceMap serviceMap, string[] viewNamespaces, string[] viewModelNamespaces)
        {
            Instance = new AppNavigation(serviceMap, viewNamespaces, viewModelNamespaces);
        }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public static AppNavigation Instance
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            get => (AppNavigation)MvvmNavigation.Instance;
            protected set => MvvmNavigation.Instance = value;
        }

    }
}
