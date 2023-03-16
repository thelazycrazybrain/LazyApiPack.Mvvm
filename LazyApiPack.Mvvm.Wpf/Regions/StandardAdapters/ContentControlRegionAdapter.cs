using System.Windows.Controls;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters {
    public class ContentControlRegionAdapter : RegionAdapter<ContentControl> {
        public ContentControlRegionAdapter(ContentControl presenterControl) : base(presenterControl) {
        }

        public override void AddView(object view) {
            PresenterControl.Content=view;
        }

        public override void RemoveView(object view) {
            if (view == null || PresenterControl.Content == view) {
                PresenterControl.Content=null;
            }
        }

    }

}
