using IronPython.Module.Services;
using LazyApiPack.Localization;
using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Application.Configuration;
using LazyApiPack.Mvvm.Wpf.Regions;
using Reporting;
using System;
using System.Windows;

namespace IronPython.Module
{
    public class IronPythonModule : MvvmModule
    {
        private string _moduleId = "net.thelazycrazybrain.IronPythonModule";
        public override string ModuleId { get =>_moduleId; }

        public override void OnModuleSetup(MvvmModuleConfiguration configuration)
        {
            configuration.WithModule<ReportingModule>()
                .WithViews("IronPython.Module.Views")
                .WithViewModels("IronPython.Module.ViewModels")
                .WithService<IPythonInterpreterService, PythonInterpreterService>();
        }

        public override void OnMessageReceived(MvvmModule sender, string? moduleId, string messageId, object message)
        {
            if (messageId == "MSG")
            {
                MessageBox.Show(message.ToString());
            }
        }
    }
}