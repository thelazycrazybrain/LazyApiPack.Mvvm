using LazyApiPack.Mvvm.Wpf.Application;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    public class MultiWindowRegionAdapter : RegionAdapter<Window>
    {
        Dictionary<object, IWindowTemplate> _activeWindows = new();
        public override void AddView(object view, bool isModal, Type dialogType, UIElement presenter)
        {
            var wdw = (IWindowTemplate)Activator.CreateInstance(dialogType);
            wdw.Content = view;
            wdw.Closed += Wdw_Closed;
            _activeWindows.Add(view, wdw);
            if (isModal)
            {
                wdw.ShowDialog();
            }
            else
            {
                wdw.Show();
            }
        }

        private void Wdw_Closed(object? sender, EventArgs e)
        {
            var view = _activeWindows.First(w => w.Value == sender);
            _activeWindows.Remove(view);
        }

        public override void RemoveView(object view, UIElement presenter)
        {
            var wdw = _activeWindows[view];
            wdw.Close();
            _activeWindows.Remove(view);
        }
    }

}
