using LazyApiPack.Mvvm.Wpf.Localization;
using LazyApiPack.Mvvm.Wpf.Application;
using LazyApiPack.Mvvm.Wpf.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters {
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
