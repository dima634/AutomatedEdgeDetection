using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UwpApp.Imaging;
using UwpApp.Imaging.Extensions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UwpApp.Mvvm.ViewModels
{
    class GalleryPageViewModel : ViewModelBase
    {
        public GalleryPageViewModel()
        {
            RefreshCommand.Execute(null);
        }

        private GalleryViewModel _gallery;

        public GalleryViewModel Gallery
        {
            get => _gallery;
            set
            {
                _gallery = value;
                RaisePropertyChanged(nameof(Gallery));
            }
        }

        private bool _isImagePicked;

        public bool IsImagePicked
        {
            get { return _isImagePicked; }
            set { _isImagePicked = value; RaisePropertyChanged(nameof(IsImagePicked)); }
        }


        private ImageSource imageSource;

        public ImageSource PickedImage
        {
            get { return imageSource; }
            set { imageSource = value; RaisePropertyChanged(nameof(PickedImage)); }
        }


        public ICommand RefreshCommand => new AsyncCommand(async (parameter) =>
        {
            RunOnUIThreadAsync(() => IsLoading = true);

            var images = await new Gallery().LoadImagesAsync();

            var gallery = new List<ImageSource>();


            RunOnUIThreadAsync(async () =>
            {
                foreach (var image in images)
                {
                    gallery.Add(await image.ToSoftwareBitmapSourceAsync());
                }

                Gallery = new GalleryViewModel() { Images = gallery };
                IsLoading = false;
            }); 
        });

        public ICommand PickImageCommand => new Command((parameter) =>
        {
            var arg = parameter as ItemClickEventArgs;
            PickedImage = (ImageSource)arg.ClickedItem;
            IsImagePicked = true;
        });

        public ICommand UnPickImageCommand => new Command((parameter) =>
        {
            IsImagePicked = false;
        });
    }
}
