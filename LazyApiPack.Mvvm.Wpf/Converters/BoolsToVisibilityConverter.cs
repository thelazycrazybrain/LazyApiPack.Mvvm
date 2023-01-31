using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LazyApiPack.Mvvm.Wpf.Converters
{
    /// <summary>
    /// Converts boolean values to Visibility with parameters that support hidden and invert result (invert, !)
    /// </summary>
    public class BoolsToVisibilityConverter : BoolToVisibilityConverter, IMultiValueConverter
    { /// <summary>
      /// Converts a list of booleans into a Visibility.
      /// </summary>
      /// <param name="value">Boolean input</param>
      /// <param name="parameter">Can contain "!", "invert", "hidden". Multiple parameters are separated with the characters ";", "," and "|".</param>
      /// <returns>Visibility due to value and parameter</returns>
      /// <example>Convert(myBooleanValue, typeof(bool), "!;hidden", CultureInfo.InvariantCulture); // Inverts "value" and if it is false, the result would be Visibility.Hidden.</example>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var input = values.All(v => v is bool b && b);
            return Convert(input, targetType, parameter, culture);

        }

        /// <summary>
        /// Not Supported
        /// </summary>
        /// <returns>[] { DependencyProperty.UnsetValue }</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { DependencyProperty.UnsetValue };
        }
    }
}


