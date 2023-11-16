using LazyApiPack.Mvvm.Wpf.Application;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions
{
    public class RegionManager : DependencyObject
    {
        private static Dictionary<string, RegionAdapterInstance> _activeRegions = new Dictionary<string, RegionAdapterInstance>();
        private static Dictionary<Type, Type> _regionAdapters;
        internal static void Initialize(ref Dictionary<Type, Type> regionAdapters)
        {
            _regionAdapters = regionAdapters;
        }

        public static void NavigateTo(UIElement view, string regionName, bool isModal)
        {
            if (_activeRegions.ContainsKey(regionName))
            {
                _activeRegions[regionName].Instance.AddView(view);
            }
            throw new RegionNotFoundException(regionName);
        }

        public static readonly DependencyProperty RegionNameProperty =
            DependencyProperty.RegisterAttached(
          "RegionName",
          typeof(string),
          typeof(RegionManager),
          new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, OnRegionNameChanged)
        );

        private static void OnRegionNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(sender)) return;
            var target = (UIElement)sender;
            var senderType = sender.GetType();
            if (!_regionAdapters.ContainsKey(senderType))
            {
                throw new RegionAdapterNotFoundException($"Region adapter for region type {senderType} was not found.");
            }
            var adapterType = _regionAdapters[senderType];

            var adapter = new RegionAdapterInstance(target, adapterType);
            var oldValue = (string)e.OldValue;
            if (!string.IsNullOrWhiteSpace(oldValue) && _activeRegions.ContainsKey(oldValue))
            {
                _activeRegions.Remove(oldValue);
            }

            var newValue = (string)e.NewValue;
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                _activeRegions.Add(newValue, adapter);
            }

        }

        public static string GetRegionName(UIElement target) =>
            (string)target.GetValue(RegionNameProperty);


        public static void SetRegionName(UIElement target, string regionName) =>
            target.SetValue(RegionNameProperty, regionName);
    }

}
