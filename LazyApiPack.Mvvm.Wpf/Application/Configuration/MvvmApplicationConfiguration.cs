namespace LazyApiPack.Mvvm.Wpf.Application.Configuration
{
    public sealed class MvvmApplicationConfiguration
    {
        /// <summary>
        /// The application shell window type.
        /// </summary>
        public Type ShellWindow { get; internal set; }
        /// <summary>
        /// The application splash screen type.
        /// </summary>
        public Type SplashScreen { get; internal set; }
        /// <summary>
        /// Modules to be added to the application.
        /// </summary>
        public List<Type> Modules { get; internal set; } = new();
        /// <summary>
        /// Localization files to be added to the application.
        /// </summary>
        public List<string> LocalizationFiles { get; internal set; } = new();
        public List<LocalizationNamespace> LocalizationNamespaces { get; internal set; } = new();

    }
}