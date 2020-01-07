using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpApp.Mvvm.ViewModels
{
    class ViewModelBase : NotifyPropertyChangedBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(nameof(IsLoading)); }
        }

        private string _loadingLable;

        public string LoadingLabel
        {
            get { return _loadingLable; }
            set { _loadingLable = value; RaisePropertyChanged(nameof(LoadingLabel)); }
        }
    }
}
