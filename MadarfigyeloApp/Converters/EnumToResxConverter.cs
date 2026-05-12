using Terepnaplo.Resources;
using System.Globalization;

namespace Terepnaplo.Converters
{
    public class EnumToResxConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return AppRes.PleaseChoose;
            }

            if (value is Enum enumValue)
            {
                return enumValue.GetResxText();
            }

            throw new ArgumentException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
