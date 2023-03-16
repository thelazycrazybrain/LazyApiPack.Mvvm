using LazyApiPack.Mvvm.Wpf.Localization;
using LazyApiPack.Mvvm.Wpf.Navigation;
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
        public TabControlRegionAdapter(TabControl presenterControl) : base(presenterControl) { }

        public override void AddView(object view)
        {
            var title = CaptionHelper.GetMvvmCaption(view);
            PresenterControl.Items.Add(new TabItem() { Content = view, Header = title });
        }

        public override void RemoveView(object view)
        {
            var tab = PresenterControl.Items.OfType<TabItem>().FirstOrDefault(i => i.Content == view);
            if (tab != null)
            {
                PresenterControl.Items.Remove(tab);
            }
        }
    }

}
