using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UwpApp.Views;
using UwpApp.Mvvm.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
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
