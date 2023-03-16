namespace LazyApiPack.Mvvm.Wpf.Navigation
{
    public interface ISplashScreenWindowTemplate : IWindowTemplate
    {
        /// <summary>
        /// Title of the progress (eg. Loading Modules)
        /// </summary>
        string? ProgressTitle { get; set; }
        /// <summary>
        /// Description of the progress (eg. ModuleA or Loading x of y)
        /// </summary>
        string? ProgressDescription { get; set; }

        /// <summary>
        /// Gets or sets the progress in percent. If null, no progress status is known.
        /// </summary>
        double? ProgressPercentage { get; set; }
    }
}
