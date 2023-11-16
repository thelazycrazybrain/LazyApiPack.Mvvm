using LazyApiPack.Mvvm.Wpf.Application;
using LazyApiPack.Mvvm.Wpf.Regions;
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

namespace LazyApiPack.Mvvm.Wpf.Tests.WindowTemplates
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window, ISplashScreenWindowTemplate
    {
        public SplashScreen()
        {
            InitializeComponent();
        }
        /// <inheritdoc />
        public string? ProgressTitle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        /// <inheritdoc />
        public string? ProgressDescription { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        /// <inheritdoc />
        public double? ProgressPercentage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasProgress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
