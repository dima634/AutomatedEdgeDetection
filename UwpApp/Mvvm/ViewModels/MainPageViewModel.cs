using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using UwpApp.Views;
using UwpApp.Mvvm.Views;

namespace UwpApp.ViewModels
{
    class MainPageViewModel
    {
        public void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            switch ((args.SelectedItem as NavigationViewItem)?.Tag)
            {
                case "EdgeDetection":
                    (sender.Content as Frame).Navigate(typeof(EdgeDetectionPage));
                    break;
                case "DatasetGeneration":
                    (sender.Content as Frame).Navigate(typeof(DatasetGenerationPage));
                    break;
                case "Gallery":
                    (sender.Content as Frame).Navigate(typeof(GalleryPage));
                    break;
                default:
                    break;
            }
        }
    }
}
