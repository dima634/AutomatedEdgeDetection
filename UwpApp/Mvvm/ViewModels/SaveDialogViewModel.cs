using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UwpApp.Imaging;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Input;

namespace UwpApp.Mvvm.ViewModels
{
    class SaveDialogViewModel : NotifyPropertyChangedBase
    {
        public SoftwareBitmap Bitmap { get; set; }

        private string _imageName;

        public string ImageName
        {
            get { return _imageName; }
            set { _imageName = value; RaisePropertyChanged(nameof(ImageName)); }
        }


        private Gallery _gallery => DependencyInjection.Resolve<Gallery>();

        public ICommand SaveImageCommand => new Command((parameter) => _gallery.AddImageAsync(Bitmap, ImageName), (param) => ImageName != null, this);
    }
}
