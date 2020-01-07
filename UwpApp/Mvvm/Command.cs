using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;

namespace UwpApp.Mvvm
{
    class Command : ICommand
    {
        protected Action<object> _executeAction;
        private Func<object, bool> _canExecute;

        public Command(Action<object> executeAction, Func<object, bool> canExecute = null, INotifyPropertyChanged notifyPropertyChanged = null)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;

            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged += delegate
                {
                    CanExecuteChanged?.Invoke(this, null);
                };
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public virtual void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }

    class AsyncCommand : Command, ICommand
    {
        public AsyncCommand(Action<object> executeAction, Func<object, bool> canExecute = null, INotifyPropertyChanged notifyPropertyChanged = null) 
            : base(executeAction, canExecute, notifyPropertyChanged)
        {

        }

        public override void Execute(object parameter)
        {
            ThreadPool.QueueUserWorkItem((i) => _executeAction(parameter));
        }
    }
}
