using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Shapes;

namespace UwpApp.ViewModels.Converters
{
    class ImageTappedEventCommandParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var param = parameter as ImageTappedEventInputConverterParameter;

            return new ImageTappedEventCommandParameter
            {
                EventCommandParameter = new EventCommandParameter
                {
                    Args = value as RoutedEventArgs,
                    Sender = param.Sender
                },
                Rectangle = param.Rectangle,
                Canvas = param.Canvas
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class ImageTappedEventInputConverterParameter
    {
        public object Sender { get; set; }

        public Rectangle Rectangle { get; set; }

        public Canvas Canvas { get; set; }
    }

    class ImageTappedEventCommandParameter
    {
        public EventCommandParameter EventCommandParameter { get; set; }

        public Rectangle Rectangle { get; set; }

        public Canvas Canvas { get; set; }
    }
}
