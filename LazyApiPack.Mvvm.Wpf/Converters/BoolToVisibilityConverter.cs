using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LazyApiPack.Mvvm.Wpf.Converters
{
    /// <summary>
    /// Converts boolean values to Visibility with parameters that support hidden and invert result (invert, !)
    /// </summary>
    public class BoolToVisibilityConverter : BoolConverterBase, IValueConverter
    {
        /// <summary>
        /// Converts a boolean into a Visibility.
        /// </summary>
        /// <param name="value">Boolean input</param>
        /// <param name="parameter">Can contain "!", "invert", "hidden". Multiple parameters are separated with the characters ";", "," and "|".</param>
        /// <returns>Visibility due to value and parameter</returns>
        /// <example>Convert(myBooleanValue, typeof(bool), "!;hidden", CultureInfo.InvariantCulture); // Inverts "value" and if it is false, the result would be Visibility.Hidden.</example>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool input)
            {
                if (parameter is not string param)
                {
                    return input ? Visibility.Visible : Visibility.Collapsed;

                }
                else
                {
                    ParseVisibilityParameter(param, out var invertResult, out var falseVisibility);
                    var result = invertResult ? !input : input;
                    return result ? Visibility.Visible : falseVisibility;
                }

            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// Converts a visibility back to boolean. See Convert for details.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Visibility visibility)
            {
                return DependencyProperty.UnsetValue;
            }
            ParseVisibilityParameter((string)parameter, out var invertResult, out var _);

            switch (visibility)
            {
                case Visibility.Visible:
                    return !invertResult;
                case Visibility.Hidden:
                case Visibility.Collapsed:
                    return invertResult;
                default:
                    throw new NotSupportedException($"Visibility type {visibility} is not supported.");
            }

        }
    }
}


