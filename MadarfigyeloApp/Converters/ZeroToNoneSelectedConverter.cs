using Terepnaplo.Resources;
using System.Globalization;

namespace Terepnaplo.Converters
{
    public class EmptyToNoneSelectedConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || value is string s && string.IsNullOrWhiteSpace(s))
            {
                return AppRes.NoneSelected;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
