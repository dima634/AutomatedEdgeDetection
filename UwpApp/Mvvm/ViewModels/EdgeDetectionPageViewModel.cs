using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System;
using UwpApp.Imaging;
using UwpApp.Imaging.Extensions;
using UwpApp.MachineLearning.Datasets;
using UwpApp.MachineLearning.Predictors;
using UwpApp.Mvvm;
using UwpApp.Mvvm.ViewModels;
using UwpApp.Mvvm.Views;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace UwpApp.ViewModels
{
    class EdgeDetectionPageViewModel : ViewModelBase
    {
        private ImageSource _image;

        public ImageSource Image
        {
            get { return _image; }
            set
            {
                _image = value;
                RaisePropertyChanged(nameof(Image));
            }
        }

        private SoftwareBitmap RestoredImage { get; set; }

        private ImageSource _restoredImage;

        public ImageSource RestoredImageSource
        {
            get { return _restoredImage; }
            set { _restoredImage = value; RaisePropertyChanged(nameof(RestoredImageSource)); }
        }

        public ObservableCollection<string> AvailableAlgorithms => new ObservableCollection<string>()
        {
            "Neural network",
            "Conv neural network",
            "Sdca",
            "Lbfgs"
        };

        private string _selectedAlgorithm;

        public string SelectedAlgorithm
        {
            get { return _selectedAlgorithm; }
            set { _selectedAlgorithm = value; RaisePropertyChanged(nameof(SelectedAlgorithm)); }
        }

        public ObservableCollection<string> SplitingStep => new ObservableCollection<string>()
        {
            "Single",
            "Sliding window size"
        };

        private string _selectedSplitingStep;

        public string SelectedSplitingStep
        {
            get { return _selectedSplitingStep; }
            set { _selectedSplitingStep = value; RaisePropertyChanged(nameof(SelectedAlgorithm)); }
        }

        private StorageFile file;
        public StorageFile File
        {
            get => file;
            set
            {
                file = value;
                RaisePropertyChanged(nameof(File));
            }
        }

        public ICommand RunCommand => new AsyncCommand(async (parameter) =>
        {
            RunOnUIThreadAsync(() =>
            {
                IsLoading = true;
                LoadingLabel = "Loading image...";
            });

            var image = await Utils.LoadAndResizeImageAsync(file, Dataset.ImageSize);

            RunOnUIThreadAsync(async () =>
            {
                Image = await image.ToSoftwareBitmapSourceAsync();
                LoadingLabel = "Spliting...";
            });

            var imageFactory = DependencyInjection.Resolve<IConstructingAlgorithm>(SelectedSplitingStep);
            var useSingleStepSpliting = SelectedSplitingStep == "Single" ? true : false;
            var splittedImage = new ImageSplitter(image, Dataset.ImageSize, useSingleStepSpliting)
                .Split()
                .Select(i => new ImageData()
                {
                    Pixels = i.ToBgr8Array().Select(j => (float)j).ToArray()
                })
                .ToArray();

            RunOnUIThreadAsync(() =>
            {
                LoadingLabel = "Predicting...";
            });

            var predictor = DependencyInjection.Resolve<IPredictor>(SelectedAlgorithm);
            var predictions = predictor.PredictBatch(splittedImage);

            RunOnUIThreadAsync(async () =>
            {
                LoadingLabel = "Drawing...";
                RestoredImage = imageFactory.CreateImage(predictions, Dataset.ImageSize, image.PixelHeight, image.PixelWidth);
                RestoredImageSource = await RestoredImage.ToSoftwareBitmapSourceAsync();
                IsLoading = false;
            });
        },
        (parameter) =>
        {
            return File == null || SelectedAlgorithm == null || SelectedSplitingStep == null ? false : true;
        },
        this);

        private Gallery _gallery = DependencyInjection.Resolve<Gallery>();

        public ICommand SaveImageCommand => new Command(async (parameter) =>
        {
            var dataContext = new SaveDialogViewModel()
            {
                Bitmap = RestoredImage
            };

            var dialog = new SaveImageDialog()
            {
                DataContext = dataContext,
                PrimaryButtonCommand = dataContext.SaveImageCommand
            };

            await dialog.ShowAsync();
        }, (param) => RestoredImage != null, this);
    }
}
