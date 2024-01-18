using IronPython.Module.Models;
using IronPython.Module.Services;
using LazyApiPack.Mvvm.Application;
using LazyApiPack.Mvvm.Model.BaseImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronPython.Module.ViewModels
{
    public class InterpreterViewModel : ViewModelBase<PythonInterpreterModel, string>
    {
        IPythonInterpreterService _interpreter;
        public InterpreterViewModel(IPythonInterpreterService interpreter)
        {

            try
            {
                MvvmApplication.Instance.IsBusy["PythonInterpreterInitialization"] = true;
                _interpreter = interpreter;
            }
            finally
            {
                MvvmApplication.Instance.IsBusy["PythonInterpreterInitialization"] = false;
            }


        }
    }
}
