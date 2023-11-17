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
        private record RegionMapping(IRegionAdapter RegionAdapter, UIElement UIElement, Type DialogPresenter);
        private static Dictionary<string, RegionMapping> _activeRegions = new();
        private static List<IRegionAdapter> _regionAdapters;
        internal static void Initialize(ref List<IRegionAdapter> regionAdapters)
        {
            _regionAdapters = regionAdapters;
        }

        public static void NavigateTo(object view, string regionName, bool isModal)
        {
            if (_activeRegions.ContainsKey(regionName))
            {
                _activeRegions[regionName].RegionAdapter.AddView(view, isModal, GetDialogWindowType(regionName), _activeRegions[regionName].UIElement);
            }
            _activeRegions[regionName].RegionAdapter.AddView(view, isModal, GetDialogWindowType(regionName), _activeRegions[regionName].UIElement);
        }

        public static readonly DependencyProperty RegionNameProperty =
            DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(RegionManager),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, OnRegionNameChanged));

        public static readonly DependencyProperty DialogWindowTypeProperty =
            DependencyProperty.RegisterAttached(
            "DialogWindowType",
            typeof(Type),
            typeof(RegionManager),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnDialogWindowTypeChanged));

        private static void OnDialogWindowTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(sender)) return;
            var target = (UIElement)sender;
            UpdateNameAndDialogType(target);

        }

        private static void OnRegionNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(sender)) return;
            UpdateNameAndDialogType((UIElement)sender);

        }

        private static void UpdateNameAndDialogType(UIElement target)
        {
            var senderType = target.GetType();
            var region = GetRegionName(target);
            if (string.IsNullOrEmpty(region)) return;
            var adapter = _regionAdapters.FirstOrDefault(r => r.PresenterControlType == senderType);
            if (adapter == null)
            {
                adapter = _regionAdapters.FirstOrDefault(r => r.PresenterControlType.IsAssignableFrom(senderType));
            }
            if (adapter == null)
            {
                throw new RegionAdapterNotFoundException($"Region adapter for region type {senderType} was not found.");
            }

            if (!_activeRegions.ContainsKey(region))
            {
                _activeRegions.Add(region, new RegionMapping(adapter, target, GetDialogWindowType(target)));
            } else
            {
                //var kvp = _activeRegions[region];
                _activeRegions.Remove(region);
                _activeRegions.Add(region, new RegionMapping(adapter, target, GetDialogWindowType(target)));
            }


        }

        public static string GetRegionName(UIElement target) =>
            (string)target.GetValue(RegionNameProperty);


        public static void SetRegionName(UIElement target, string regionName) =>
            target.SetValue(RegionNameProperty, regionName);

        public static Type GetDialogWindowType(UIElement target) =>
            (Type)target.GetValue(DialogWindowTypeProperty);

        public static void SetDialogWindowType(UIElement target, Type dialogWindowType) =>
            target.SetValue(DialogWindowTypeProperty, dialogWindowType);

        public static Type GetDialogWindowType(string region) => _activeRegions[region].DialogPresenter;
    }

}
