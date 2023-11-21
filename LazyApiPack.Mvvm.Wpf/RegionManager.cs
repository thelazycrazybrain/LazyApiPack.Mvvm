using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Regions;
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
    /// <summary>
    /// Provides functionality to navigate with regions.
    /// </summary>
    public class RegionManager : DependencyObject, IRegionManager
    {
        private record RegionMapping(IRegionAdapter RegionAdapter, UIElement UIElement, Type DialogPresenter);
        private static Dictionary<string, RegionMapping> _activeRegions = new();
        private static List<IRegionAdapter> _regionAdapters;
        static RegionManager()
        {
            MvvmApplication.Instance.RegionManager = new RegionManager();
        }
        /// <summary>
        /// Passes the region adapters from the application by reference.
        /// </summary>
        /// <param name="regionAdapters">The list of region adapters as a reference.</param>
        public void Initialize(ref List<IRegionAdapter> regionAdapters)
        {
            _regionAdapters = regionAdapters;
        }
        /// <summary>
        /// Navigates to a specific region.
        /// </summary>
        /// <param name="view">The view that is navigated to.</param>
        /// <param name="regionName">The region name that the view is displayed in.</param>
        /// <param name="isModal">Indicates if the view blocks the rest of the application.</param>
        /// <remarks>Not to be used by the user. Use MvvmApplication.Navigation.NavigateTo() instead.</remarks>
        public void NavigateTo(object view, string regionName, bool isModal)
        {
            if (_activeRegions.ContainsKey(regionName))
            {
                _activeRegions[regionName].RegionAdapter.AddView(view, isModal, GetDialogWindowType(regionName), _activeRegions[regionName].UIElement);
            }
        }

        /// <summary>
        /// Name of the region.
        /// </summary>
        public static readonly DependencyProperty RegionNameProperty =
            DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(RegionManager),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender, OnRegionNameChanged));

        /// <summary>
        /// Type of the window that is displayed when navigated to.
        /// </summary>
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
        /// <summary>
        /// Name of the region.
        /// </summary>
        public static string GetRegionName(UIElement target) =>
            (string)target.GetValue(RegionNameProperty);

        /// <summary>
        /// Name of the region.
        /// </summary>
        public static void SetRegionName(UIElement target, string regionName) =>
            target.SetValue(RegionNameProperty, regionName);

        /// <summary>
        /// Type of the window that is displayed when navigated to.
        /// </summary>
        public static Type GetDialogWindowType(UIElement target) =>
            (Type)target.GetValue(DialogWindowTypeProperty);

        /// <summary>
        /// Type of the window that is displayed when navigated to.
        /// </summary>
        public static void SetDialogWindowType(UIElement target, Type dialogWindowType) =>
            target.SetValue(DialogWindowTypeProperty, dialogWindowType);

        /// <summary>
        /// Gets the dialog window type.
        /// </summary>
        public static Type GetDialogWindowType(string region) => _activeRegions[region].DialogPresenter;
    }

}
