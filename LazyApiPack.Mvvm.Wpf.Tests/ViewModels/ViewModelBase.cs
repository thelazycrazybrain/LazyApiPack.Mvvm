using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.Tests.ViewModels
{
    public abstract class ViewModelBase : ISupportParameter, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private object? _parameter;
        public object? Parameter
        {
            get { return _parameter; }
            set
            {
                SetPropertyValue(ref _parameter, value);
                OnParameterChanged();
            }
        }


        protected virtual void OnParameterChanged()
        {

        }

        protected virtual void SetPropertyValue<TType>(ref TType backingField, TType value, [CallerMemberName] string propertyName = null)
        {
            backingField = value;
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
