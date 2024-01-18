using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Showcase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            var app = new MvvmApp();
            MainWindow = (Window)app.Setup();
        }
    }
}
