namespace LazyApiPack.Mvvm.Wpf.Application
{
    /// <summary>
    /// Specifies a window as a WindowTemplate
    /// </summary>
    public interface IWindowTemplate
    {
        /// <summary>
        /// Raised when the window was closed.
        /// </summary>
        event EventHandler Closed;
        /// <summary>
        /// True, if the window is currently displaying.
        /// </summary>
        public bool IsVisible { get; }
        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close();
        /// <summary>
        /// Shows the window.
        /// </summary>
        public void Show();
        /// <summary>
        /// Shows the window as modal dialog.
        /// </summary>
        /// <returns></returns>
        public bool? ShowDialog();
        /// <summary>
        /// The view that is displayed within the window.
        /// </summary>
        public object Content { get; set; }
        
    }
}
