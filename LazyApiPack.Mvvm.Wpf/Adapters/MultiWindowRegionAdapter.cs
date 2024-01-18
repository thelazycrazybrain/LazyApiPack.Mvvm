using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Regions;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions.StandardAdapters
{
    /// <summary>
    /// Provides a region adapter to display windows and modal dialogues.
    /// </summary>
    /// <example><Window rgn:RegionManager.RegionName="ModalRegion" rgn:RegionManager.DialogWindowType="{x:Type local:DialogWindow}"></Window>.</example>
    /// <remarks>This class requires the DialogWindowType</remarks>
    public class MultiWindowRegionAdapter : RegionAdapter<Window>
    {
        Dictionary<object, IWindowTemplate> _activeWindows = new();
        public override void AddView(object view, bool isModal, Type dialogType, object presenter)
        {
            if (dialogType == null)
            {
                throw new NullReferenceException("To use the MultiWindowRegionAdapter, you must specify a DialogType");
            }
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

        public override void RemoveView(object view, object presenter)
        {
            var wdw = _activeWindows[view];
            wdw.Close();
            _activeWindows.Remove(view);
        }
    }

}
