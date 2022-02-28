using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace HealthMate.Helpers
{
    public class StateToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var boolVal = (bool)value;
            if (boolVal)
            {
                return ImageSource.FromFile("mic.png");

            }
            else
            {
                return ImageSource.FromFile("mic_muted.png");
            }
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
