using LazyApiPack.Mvvm.Localization;
using LazyApiPack.Mvvm.Regions;
using LazyApiPack.Wpf.Controls;
using LazyApiPack.Wpf.Controls.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Adapters
{
    public class SidebarControlRegionAdapter : RegionAdapter<Sidebar>
    {
        public override void AddView(object view, bool isModal, Type dialogType, object presenter)
        {
            var title = CaptionHelper.GetMvvmCaption(view);
            ((Sidebar)presenter).Items.Add(new SidebarItem(view) { Title = title });
        }

        public override void RemoveView(object view, object presenter)
        {
            foreach (SidebarItem item in ((Sidebar)presenter).Items.ToList())
            {
                if (item.Content == view)
                {
                    ((Sidebar)presenter).Items.Remove(item);
                    return;
                }
            }
        }
    }
}
