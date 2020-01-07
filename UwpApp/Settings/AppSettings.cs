using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace UwpApp.Settings
{
    class AppSettings
    {
        public static IPropertySet Local => ApplicationData.Current.LocalSettings.Values;
    }
}
