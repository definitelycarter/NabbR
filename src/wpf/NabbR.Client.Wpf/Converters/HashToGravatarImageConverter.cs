using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace NabbR.Converters
{
    public class HashToGravatarImageConverter : IValueConverter
    {
        private Uri gravitarUri = new Uri("https://secure.gravatar.com/");

        private static float dpiScaleFactor;

        static HashToGravatarImageConverter()
        {
            var dpiProperty = typeof(SystemParameters).GetProperty("Dpi", BindingFlags.NonPublic | BindingFlags.Static);
            var dpi = (int)dpiProperty.GetValue(null, null);

            dpiScaleFactor = ((float)dpi) / 96f;
        }

        private static string DpiAdjusted(string size)
        {
            float number;
            if (float.TryParse(size, out number))
            {
                return Math.Floor(number * dpiScaleFactor).ToString();
            }
            return size;
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var hash = value as String;
            var size = parameter as String;

            size = DpiAdjusted(size);

            var relativeUri = String.Format("avatar/{0}?s={1}&d=mm", hash, size);

            return new Uri(gravitarUri, relativeUri);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
