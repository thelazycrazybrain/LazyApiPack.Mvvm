using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    public class MultiWindowRegionAdapter : RegionAdapter<Window>
    {
        public MultiWindowRegionAdapter(Window presenterControl) : base(presenterControl)
        {
        }

        public override void AddView(object view)
        {
            throw new NotImplementedException();
        }

        public override void RemoveView(object view)
        {
            throw new NotImplementedException();
        }
    }

}
