using LazyApiPack.Mvvm.Wpf.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemoApp.WindowTemplates
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window, ISplashScreenWindowTemplate
    {
        public SplashWindow()
        {
            var asm = Assembly.GetEntryAssembly();
            Copyright = asm?.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            Version = asm?.GetCustomAttribute<AssemblyVersionAttribute>()?.Version;

            InitializeComponent();
        }


        public static readonly DependencyProperty ProgressTitleProperty = DependencyProperty.Register(nameof(ProgressTitle), typeof(string), typeof(SplashWindow), new PropertyMetadata(""));
        public string? ProgressTitle
        {
            get { return (string?)GetValue(ProgressTitleProperty); }
            set { SetValue(ProgressTitleProperty, value); }
        }


        public static readonly DependencyProperty ProgressDescriptionProperty = DependencyProperty.Register(nameof(ProgressDescription), typeof(string), typeof(SplashWindow), new PropertyMetadata(""));
        public string? ProgressDescription
        {
            get { return (string?)GetValue(ProgressDescriptionProperty); }
            set { SetValue(ProgressDescriptionProperty, value); }
        }


        public static readonly DependencyProperty ProgressPercentageProperty = DependencyProperty.Register(nameof(ProgressPercentage), typeof(double), typeof(SplashWindow), new PropertyMetadata(null));
        public double? ProgressPercentage
        {
            get { return (double?)GetValue(ProgressPercentageProperty); }
            set { SetValue(ProgressPercentageProperty, value); }
        }


        public static readonly DependencyProperty CopyrightProperty = DependencyProperty.Register(nameof(Copyright), typeof(string), typeof(SplashWindow), new PropertyMetadata(""));
        public string? Copyright
        {
            get { return (string?)GetValue(CopyrightProperty); }
            set { SetValue(CopyrightProperty, value); }
        }


        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(nameof(Version), typeof(string), typeof(SplashWindow), new PropertyMetadata(""));
        public string? Version
        {
            get { return (string?)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

    }
}
