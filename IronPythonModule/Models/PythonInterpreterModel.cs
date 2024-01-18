using LazyApiPack.Mvvm.Model.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPython.Module.Models
{
    public class PythonInterpreterModel : ModelBase
    {
        public PythonInterpreterModel()
        {

        }
        private string? _title;
        public string? Title { get => _title; set => SetPropertyValue(ref _title, value); }

        private string? _history;
        public string? History { get => _history; set => SetPropertyValue(ref _history, value); }

        private string? _currentCommand;
        public string? CurrentCommand { get => _currentCommand; set => SetPropertyValue(ref _currentCommand, value); }

    }
}
