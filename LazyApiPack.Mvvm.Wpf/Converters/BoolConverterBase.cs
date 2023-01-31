using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LazyApiPack.Mvvm.Wpf.Converters
{
    public abstract class BoolConverterBase
    {
        /// <summary>
        /// Parses the parameter and returns the configuration for the BooleanConverter.
        /// </summary>
        /// <param name="parameter">Can contain "!", "invert", "hidden". Multiple parameters are separated with the characters ; , |.</param>
        /// <param name="invertResult">If the result is inverted (Parameter "invert" or "!").</param>
        /// <param name="falseVisibility">Visibility Hidden or collapsed (Parameter hidden or collapsed).</param>
        /// <returns>True, if the parameter could be parsed, otherwise false.</returns>
        /// <example>ParseVisibilityParameter("!;hidden"), out var invertResult, out var falseVisibility); // Inverts the result and if visibility is false, the result would be Visibility.Hidden.</example>
        protected virtual void ParseVisibilityParameter(string parameter, out bool invertResult, out Visibility falseVisibility)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                invertResult = false;
                falseVisibility = Visibility.Collapsed;
            }
            else
            {
                var parameters = parameter.ToUpper()
                                          .Split(new char[] { ';', ',', '|' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(p => p.Trim());
                invertResult = parameters.Any(p => p == "!" ||  p == "INVERT");
                falseVisibility = parameters.Contains("HIDDEN") ? Visibility.Hidden : Visibility.Collapsed;
            }

        }
    }
}


