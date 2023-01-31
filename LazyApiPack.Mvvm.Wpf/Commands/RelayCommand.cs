using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LazyApiPack.Mvvm.Wpf.Commands
{
    /// <summary>
    /// Provides a Command for WPF binding.
    /// </summary>
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand([NotNull] Action<object?> execute)
            : this(execute, null)
        {
        }


        public RelayCommand([NotNull] Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        [DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }



        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }

    /// <summary>
    /// Provides a Command for WPF binding with a specific parameter type
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public class RelayCommand<TParameter> : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly Action<TParameter?> _execute;
        private readonly Predicate<TParameter?>? _canExecute;

        public RelayCommand([NotNull] Action<TParameter?> execute)
            : this(execute, null)
        {
        }


        public RelayCommand([NotNull] Action<TParameter?> execute, Predicate<TParameter?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }


        [DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            else
            {
                if (parameter is not TParameter p)
                {
                    return false;
                }
                else
                {
                    return _canExecute(p);
                }
            }
        }



        public void Execute(object? parameter)
        {
            if (parameter is not TParameter p)
            {
                return;
            }
            else
            {
                _execute(p);
            }
        }
    }

}
