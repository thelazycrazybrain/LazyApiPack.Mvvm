using LazyApiPack.Mvvm.Wpf.Localization;
using System.Windows;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters {
    /// <summary>
    /// Provides a region adapter for a tab control.
    /// </summary>
    public class TabControlRegionAdapter : RegionAdapter<TabControl>
    { 
        public override void AddView(object view, bool isModal, Type dialogType, UIElement presenter)
        {
            var title = CaptionHelper.GetMvvmCaption(view);
            ((TabControl)presenter).Items.Add(new TabItem() { Content = view, Header = title });
        }

        public override void RemoveView(object view, UIElement presenter)
        {
            var tab = ((TabControl)presenter).Items.OfType<TabItem>().FirstOrDefault(i => i.Content == view);
            if (tab != null)
            {
                ((TabControl)presenter).Items.Remove(tab);
            }
        }
    }

}
