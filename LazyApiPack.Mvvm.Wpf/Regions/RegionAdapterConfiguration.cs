using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Regions
{
    /// <summary>
    /// Region Adapter used by the mvvm framework.
    /// </summary>
    public interface IRegionAdapter
    {
        /// <summary>
        /// The host control type (eg. StackPanel, TabControl etc.)
        /// </summary>
        Type PresenterControlType { get; }
        /// <summary>
        /// Adds a view to the control.
        /// </summary>
        /// <param name="view">The generated view.</param>
        /// <param name="isModal">Indicates, if the view locks the rest of the application.</param>
        /// <param name="dialogType">If used as a window, the window template type.</param>
        /// <param name="presenter">The host control that displayes the content.</param>
        void AddView(object view, bool isModal, Type dialogType, UIElement presenter);
        /// <summary>
        /// Removes a view from the presenter control.
        /// </summary>
        /// <param name="view">The view that is to be closed.</param>
        /// <param name="presenter">The host control that displays the content.</param>
        void RemoveView(object view, UIElement presenter);

    }

    /// <summary>
    /// Base class for a Region Adapter used by the mvvm framework.
    /// </summary>
    public abstract class RegionAdapter<TPresenter> : IRegionAdapter
                                                        where TPresenter : UIElement
    {
        /// <inheritdoc/>
        public Type PresenterControlType { get => typeof(TPresenter); }
        /// <inheritdoc/>
        public abstract void AddView(object view, bool isModal, Type dialogType, UIElement presenter);
        /// <inheritdoc/>
        public abstract void RemoveView(object view, UIElement presenter);

    }




}
