using LazyApiPack.Mvvm.Application;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LazyApiPack.Mvvm.Wpf.Showcase
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow : Window, ISplashScreenWindowTemplate
    {
        public SplashWindow()
        {
            InitializeComponent();
        }



        public static readonly DependencyProperty ProgressTitleProperty = DependencyProperty.Register(nameof(ProgressTitle), typeof(string), typeof(SplashWindow), new PropertyMetadata(null));
        public string? ProgressTitle
        {
            get { return (string?)GetValue(ProgressTitleProperty); }
            set { SetValue(ProgressTitleProperty, value); }
        }


        public static readonly DependencyProperty ProgressDescriptionProperty = DependencyProperty.Register(nameof(ProgressDescription), typeof(string), typeof(SplashWindow), new PropertyMetadata(null));
        public string? ProgressDescription
        {
            get { return (string?)GetValue(ProgressDescriptionProperty); }
            set { SetValue(ProgressDescriptionProperty, value); }
        }


        public static readonly DependencyProperty ProgressPercentageProperty = DependencyProperty.Register(nameof(ProgressPercentage), typeof(double?), typeof(SplashWindow), new PropertyMetadata(null));
        public double? ProgressPercentage
        {
            get { return (double?)GetValue(ProgressPercentageProperty); }
            set { SetValue(ProgressPercentageProperty, value); }
        }


        public static readonly DependencyProperty HasProgressProperty = DependencyProperty.Register(nameof(HasProgress), typeof(bool), typeof(SplashWindow), new PropertyMetadata(true));
        public bool HasProgress
        {
            get { return (bool)GetValue(HasProgressProperty); }
            set { SetValue(HasProgressProperty, value); }
        }

    }
}
