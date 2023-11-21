using LazyApiPack.Mvvm.Regions;
using System.Windows;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    /// <summary>
    /// Provides a region adapter for a content control.
    /// </summary>
    public class ContentControlRegionAdapter : RegionAdapter<ContentControl>
    {
        public override void AddView(object view, bool isModal, Type dialogType, object presenter)
        {
            ((ContentControl)presenter).Content = view;
        }

        public override void RemoveView(object view, object presenter)
        {
            if (view == null || ((ContentControl)presenter).Content == view)
            {
                ((ContentControl)presenter).Content = null;
            }
        }

    }

}
