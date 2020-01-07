using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UwpApp.Imaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UwpApp.Controls
{
    public sealed partial class PixelImage : UserControl
    {
        public static readonly DependencyProperty PixelsProperty =
                                       DependencyProperty.Register(
                                          nameof(Pixels),
                                          typeof(Pixel[]),
                                          typeof(PixelImage),
                                          new PropertyMetadata(default(object), OnCurrentReadingChanged));

        private static void OnCurrentReadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pxImage = d as PixelImage;

            pxImage.Grid.Children.Clear();
            pxImage.Grid.ColumnDefinitions.Clear();
            pxImage.Grid.RowDefinitions.Clear();

            for (int i = 0; i < pxImage.PixelWidth; i++)
            {
                pxImage.Grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < pxImage.PixelHeight; i++)
            {
                pxImage.Grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0, row = 0, col = 0; i < pxImage.Pixels.Length; i++, col++)
            {
                var rect = new Rectangle()
                {
                    Fill = new SolidColorBrush(Color.FromArgb(255, pxImage.Pixels[i].R, pxImage.Pixels[i].G, pxImage.Pixels[i].B))
                };
                pxImage.Grid.Children.Add(rect);
                Grid.SetColumn(rect, col);
                Grid.SetRow(rect, row);

                if (col == pxImage.PixelWidth - 1)
                {
                    col = -1;
                    row++;
                }
            }
        }

        public Pixel[] Pixels
        {
            get { return (Pixel[])GetValue(PixelsProperty); }
            set { SetValue(PixelsProperty, value); }
        }

        public static readonly DependencyProperty PixelHeightProperty =
                                       DependencyProperty.Register(
                                          nameof(PixelHeight),
                                          typeof(int),
                                          typeof(PixelImage),
                                          new PropertyMetadata(default(object)));

        public int PixelWidth
        {
            get { return (int)GetValue(PixelWidthProperty); }
            set { SetValue(PixelWidthProperty, value); }
        }

        public static readonly DependencyProperty PixelWidthProperty =
                                       DependencyProperty.Register(
                                          nameof(PixelWidth),
                                          typeof(int),
                                          typeof(PixelImage),
                                          new PropertyMetadata(default(object)));

        public int PixelHeight
        {
            get { return (int)GetValue(PixelHeightProperty); }
            set { SetValue(PixelHeightProperty, value); }
        }

        public PixelImage()
        {
            this.InitializeComponent();

            
        }
    }
}
