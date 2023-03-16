using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions
{
    public class RegionManager : DependencyObject
    {
        private static Dictionary<string, UIElement> _regions = new Dictionary<string, UIElement>();
        public static readonly DependencyProperty RegionNameProperty =
            DependencyProperty.RegisterAttached(
          "RegionName",
          typeof(string),
          typeof(RegionManager),
          new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, OnRegionNameChanged)
        );

        private static void OnRegionNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (UIElement)sender;

            var oldValue = (string)e.OldValue;
            if (!string.IsNullOrWhiteSpace(oldValue) && _regions.ContainsKey(oldValue))
            {
                _regions.Remove(oldValue);
            }

            var newValue = (string)e.NewValue;
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                _regions.Add(newValue, target);
            }

        }

        public static string GetRegionName(UIElement target) =>
            (string)target.GetValue(RegionNameProperty);


        public static void SetRegionName(UIElement target, string regionName) =>
            target.SetValue(RegionNameProperty, regionName);

        public static bool RegionAvailable(string regionName)
        {
            return _regions.ContainsKey(regionName);
        }
        public static UIElement GetNavigationControl(string regionName)
        {
            return _regions[regionName];
        }
       
    }
}
