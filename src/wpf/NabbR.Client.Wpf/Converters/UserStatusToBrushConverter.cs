using JabbR.Client.Models;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NabbR.Converters
{
    public class UserStatusToBrushConverter : IValueConverter
    {
        public Brush ActiveBrush { get; set; }
        public Brush InactiveBrush { get; set; }
        public Brush OfflineBrush { get; set; }

        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            UserStatus status = (UserStatus)value;

            switch(status)
            {
                case UserStatus.Active:
                    return this.ActiveBrush;
                case UserStatus.Inactive:
                    return this.InactiveBrush;
                case UserStatus.Offline:
                    return this.OfflineBrush;
                default:
                    return null;
            }
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
