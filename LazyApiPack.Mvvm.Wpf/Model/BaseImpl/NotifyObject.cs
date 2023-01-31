using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LazyApiPack.Mvvm.Wpf.BaseImpl
{
    [DebuggerStepThrough]
    public abstract class NotifyObject : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;
        protected virtual bool SetPropertyValue<T>(ref T backingField, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                return false;
            }

            var oldValue = backingField;
            OnPropertyChanging(oldValue, newValue, propertyName);

            backingField = newValue;

            OnPropertyChanged(oldValue, newValue, propertyName);
            return true;
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(object? oldValue, object? newValue, [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanging(object? oldValue, object? newValue, [CallerMemberName] string? propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

        }


    }
}