using System.Windows;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    public class ContentControlRegionAdapter : RegionAdapter<ContentControl>
    {
        public override void AddView(object view, bool isModal, Type dialogType, UIElement presenter)
        {
            ((ContentControl)presenter).Content = view;
        }

        public override void RemoveView(object view, UIElement presenter)
        {
            if (view == null || ((ContentControl)presenter).Content == view)
            {
                ((ContentControl)presenter).Content = null;
            }
        }

    }

}
