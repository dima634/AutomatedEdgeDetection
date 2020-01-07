using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpApp.Controls
{
    public sealed partial class FilePicker : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
                                       DependencyProperty.Register(
                                          nameof(Header),
                                          typeof(object),
                                          typeof(FilePicker),
                                          new PropertyMetadata(default(object)));

        public object Header
        {
            get { return (object)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
                                       DependencyProperty.Register(
                                          nameof(File),
                                          typeof(StorageFile),
                                          typeof(FilePicker),
                                          new PropertyMetadata(default(object)));

        public StorageFile File
        {
            get { return (StorageFile)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty FileTypesProperty =
                                       DependencyProperty.Register(
                                          nameof(FileTypes),
                                          typeof(string),
                                          typeof(FilePicker),
                                          new PropertyMetadata(default(object)));

        public string FileTypes
        {
            get { return (string)GetValue(FileTypesProperty); }
            set { SetValue(FileTypesProperty, value); }
        }

        public FilePicker()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            foreach (var type in FileTypes.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                picker.FileTypeFilter.Add(type);
            }
            var file = await picker.PickSingleFileAsync();
            
            if(file != null)
            {
                File = file;
                FilePath.Text = file.Path;
            }
        }

    }
    class StorageFileToPathConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as StorageFile)?.Path ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
