using LazyApiPack.Mvvm.Wpf.Localization;
using LazyApiPack.Wpf.Controls.Navigation;
using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    public class ClosableTabControlRegionAdapter : RegionAdapter<TabControl>
    {
        public ClosableTabControlRegionAdapter(TabControl presenterControl) : base(presenterControl)
        {

        }

        public override void AddView(object view)
        {
            var title = CaptionHelper.GetMvvmCaption(view);
            PresenterControl.Items.Add(new CloseableTabItem() { Content = view, Header = title });
        }

        public override void RemoveView(object view)
        {
            foreach (CloseableTabItem item in PresenterControl.Items)
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
