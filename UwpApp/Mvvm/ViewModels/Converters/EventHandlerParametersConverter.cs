using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UwpApp.ViewModels.Converters
{
    class EventHandlerParametersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new EventCommandParameter()
            {
                Args = value as RoutedEventArgs,
                Sender = parameter
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    class EventCommandParameter
    {
        public object Sender { get; set; }

        public RoutedEventArgs Args { get; set; }
    }
}
