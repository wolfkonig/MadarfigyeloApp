using Terepnaplo.Resources;
using System.Globalization;

namespace Terepnaplo.Converters
{
    public class StringToResxConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string key || string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }
            var text = key;
            try
            {
                text = AppRes.ResourceManager.GetString(key, AppRes.Culture);
            }
            catch { }
            return text;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
