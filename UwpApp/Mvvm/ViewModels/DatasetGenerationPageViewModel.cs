using System;
using System.Windows.Input;
using UwpApp.Imaging;
using UwpApp.Mvvm;
using UwpApp.ViewModels.Converters;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using UwpApp.Imaging.Extensions;
using UwpApp.MachineLearning.Datasets;
using Windows.UI.Xaml.Shapes;
using Windows.Storage;
using System.Threading.Tasks;
using UwpApp.MachineLearning.Predictors;
using Unity;
using System.Linq;
using UwpApp.Mvvm.ViewModels;

namespace UwpApp.ViewModels
{
    class DatasetGenerationPageViewModel : ViewModelBase
    {
        private StorageFile _imageFile;
        public StorageFile ImageFile
        {
            get => _imageFile;
            set
            {
                _imageFile = value;
                ImageChanged();
                RaisePropertyChanged(nameof(ImageFile));
            }
        }

        public async void ImageChanged()
        {
            _image = await Utils.LoadAndResizeImageAsync(ImageFile, SlidingWindowSize);
            ImageSource = await _image.ToSoftwareBitmapSourceAsync();
            _imageSplitter = new ImageSplitter(_image, SlidingWindowSize);
        }

        public Pixel[] SelectedArea
        {
            get => _selectedArea;
            set
            {
                _selectedArea = value;
                RaisePropertyChanged(nameof(SelectedArea));
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                RaisePropertyChanged(nameof(ImageSource));
            }
        }
        
        private bool _autoPickOnAddPattern = true;

        public bool AutoPick
        {
            get { return _autoPickOnAddPattern; }
            set { _autoPickOnAddPattern = value; RaisePropertyChanged(nameof(AutoPick)); }
        }

        private int _datasetSize = new Dataset().Load().Count;

        public int DatasetSize
        {
            get { return _datasetSize; }
            set { _datasetSize = value; RaisePropertyChanged(nameof(DatasetSize)); }
        }

        private Random _random = new Random();
        private ImageSplitter _imageSplitter;
        private SoftwareBitmap _image;
        private Pixel[] _selectedArea;
        private Dataset _dataset = new Dataset();

        public int SlidingWindowSize { get; set; } = Dataset.ImageSize;

        public ICommand ImageTappedCommand => new Command((parameter) =>
        {
            var eventCommandParameter = parameter as ImageTappedEventCommandParameter;
            var args = eventCommandParameter.EventCommandParameter.Args as PointerRoutedEventArgs;
            var sender = eventCommandParameter.EventCommandParameter.Sender as Image;
            var rect = eventCommandParameter.Rectangle;

            var pointerPosRelativeToImage = args.GetCurrentPoint(sender).Position;
            var pointerPosRelativeToCanvas = args.GetCurrentPoint(eventCommandParameter.Canvas).Position;
            int x = (int)(pointerPosRelativeToImage.X / sender.ActualWidth * _image.PixelWidth),
                y = (int)(pointerPosRelativeToImage.Y / sender.ActualHeight * _image.PixelHeight);

            int xOffset = x - SlidingWindowSize / 2, yOffset = y - SlidingWindowSize / 2;

            if (xOffset < 0 || yOffset < 0 || xOffset + SlidingWindowSize > _image.PixelWidth || yOffset + SlidingWindowSize > _image.PixelHeight)
            {
                return;
            }

            var scaleFactor = sender.ActualWidth / _image.PixelWidth;
            rect.Width = SlidingWindowSize * scaleFactor;
            rect.Height = SlidingWindowSize * scaleFactor;
            rect.Visibility = Windows.UI.Xaml.Visibility.Visible;
            rect.SetValue(Canvas.LeftProperty, pointerPosRelativeToCanvas.X - SlidingWindowSize / 2 * scaleFactor);
            rect.SetValue(Canvas.TopProperty, pointerPosRelativeToCanvas.Y - SlidingWindowSize / 2 * scaleFactor);
            SelectedArea = _imageSplitter.GetRect(xOffset, yOffset);
            //SelectedArea = await _selectedArea.ToSoftwareBitmapSourceAsync(SlidingWindowSize, SlidingWindowSize);
        });

        public ICommand AddPatternCommand => new Command((parameter) =>
        {
            _dataset.Add(SelectedArea, (EdgeType)parameter);
            DatasetSize++;
            _dataset.SaveData();

            if (AutoPick)
            {
                SelectedArea = _imageSplitter.GetRect(_random.Next(_image.PixelWidth - SlidingWindowSize),
                                                      _random.Next(_image.PixelHeight - SlidingWindowSize));
            }
        }, (parameter) => SelectedArea != null, this);

        public ICommand RetrainModelsCommand => new AsyncCommand((param) =>
        {
            RunOnUIThreadAsync(() => IsLoading = true);

            var models = DependencyInjection.UnityContainer.ResolveAll<IPredictor>().OfType<IRetrain>();
            foreach (var model in models)
            {
                model.Retrain();
            }

            RunOnUIThreadAsync(() => IsLoading = false);
        });
    }
}
