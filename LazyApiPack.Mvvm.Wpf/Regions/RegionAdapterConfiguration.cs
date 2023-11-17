using LazyApiPack.Collections.Extensions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;

namespace LazyApiPack.Mvvm.Wpf.Regions
{
    public interface IRegionAdapter
    {
        Type PresenterControlType { get; }
        void AddView(object view, bool isModal, Type dialogType, UIElement presenter);
        void RemoveView(object view, UIElement presenter);

    }
    public abstract class RegionAdapter<TPresenter> : IRegionAdapter
                                                        where TPresenter : UIElement
    {
        public Type PresenterControlType { get => typeof(TPresenter); }
        public abstract void AddView(object view, bool isModal, Type dialogType, UIElement presenter);
        public abstract void RemoveView(object view, UIElement presenter);

    }




}
