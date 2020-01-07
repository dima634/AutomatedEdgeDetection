using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace UwpApp.Mvvm
{
    class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        protected CoreDispatcher Dispatcher => Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected async void RunOnUIThreadAsync(DispatchedHandler action)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }
    }
}
