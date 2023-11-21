using LazyApiPack.Mvvm.Localization;
using LazyApiPack.Mvvm.Regions;
using LazyApiPack.Wpf.Controls.Navigation;
using System.Windows;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    /// <summary>
    /// Provides an adapter to create closeable tabs on a tabcontrol.
    /// </summary>
    public class CloseableTabControlRegionAdapter : RegionAdapter<TabControl>
    {
        public override void AddView(object view, bool isModal, Type dialogType, object presenter)
        {
            var title = CaptionHelper.GetMvvmCaption(view);
            ((TabControl)presenter).Items.Add(new CloseableTabItem() { Content = view, Header = title });
        }

        public override void RemoveView(object view, object presenter)
        {
            foreach (CloseableTabItem item in ((TabControl)presenter).Items)
            {
                if (item.Content == view)
                {
                    item.Close();
                    return;
                }
            }
        }
    }

}
