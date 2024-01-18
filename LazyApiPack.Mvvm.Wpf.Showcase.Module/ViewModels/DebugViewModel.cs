using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Model.BaseImpl;
using LazyApiPack.Wpf.Utils.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Showcase.Module.ViewModels
{
    public class DebugViewModel : ViewModelBase
    {
        public DebugViewModel()
        {
        
        }


        string _title = "Debug view";
        public string Title
        {
            get => _title;
            set => SetPropertyValue(ref _title, value);
        }


        RelayCommand _sendMessageCommand;
        public RelayCommand SendMessageCommand { get => _sendMessageCommand ??= new RelayCommand(OnSendMessageCommand_Execute); }
        protected void OnSendMessageCommand_Execute(object? parameter)
        {
            MvvmApplication.Instance.SendMessage(null, "net.thelazycrazybrain.IronPythonModule", "MSG", "This is a message");
        }
    }
}
