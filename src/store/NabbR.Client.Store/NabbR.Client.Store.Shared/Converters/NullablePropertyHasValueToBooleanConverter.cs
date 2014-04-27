using System;
using Windows.UI.Xaml.Data;

namespace NabbR.Converters
{
    public class NullablePropertyHasValueToBooleanConverter : IValueConverter
    {
        public Boolean BooleanValueForNull { get; set; }

        public Object Convert(Object value, Type targetType, Object parameter, String language)
        {
            return value == null ? BooleanValueForNull : !BooleanValueForNull;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, String language)
        {
            throw new NotImplementedException();
        }
    }
}
