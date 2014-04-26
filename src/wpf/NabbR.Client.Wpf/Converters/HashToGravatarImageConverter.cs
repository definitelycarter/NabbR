using System;
using System.Globalization;
using System.Windows.Data;

namespace NabbR.Converters
{
    public class HashToGravatarImageConverter : IValueConverter
    {
        private Uri gravitarUri = new Uri("https://secure.gravatar.com/");
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            String hash = value as String;
            String size = parameter as String;

            String relativeUri = String.Format("avatar/{0}?s={1}&d=mm", hash, size);

            return new Uri(gravitarUri, relativeUri);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
