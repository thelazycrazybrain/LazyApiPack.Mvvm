using LazyApiPack.Mvvm.Wpf.Model;
using System.Reflection;
using System.Windows;
namespace LazyApiPack.Mvvm.Wpf.Localization
{
    /// <summary>
    /// Provides functionality for localized text.
    /// </summary>
    public static class CaptionHelper 
    {
        /// <summary>
        /// Tries to get a child for the MVVM Caption Pattern (FrameworkElement.DataContext, ISupportModel<>.Model)
        /// </summary>
        /// <param name="obj">The top most object.</param>
        /// <returns>The child or null.</returns>
        private static object? GetMvvmChild(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is FrameworkElement fxe && fxe.DataContext != null)
            {
                return fxe.DataContext;
            }
            object? model;
            if ((model = obj.GetType().GetProperty(nameof(ISupportModel<object>.Model), BindingFlags.Public | BindingFlags.Instance)
                     ?.GetValue(obj)) != null)
            {
                return model;
            }
            return null;
        }

        /// <summary>
        /// Tries to get the value of a public instance property that matches the propertyName and is a string and readable.
        /// </summary>
        /// <param name="type">Class type where to look for this property.</param>
        /// <param name="obj">The object that is of type "type".</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>The value of the property or null.</returns>
        private static string? TryGetCaptionPropertyValue(Type type, object obj, string propertyName)
        {
            var captionProperty = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (captionProperty?.PropertyType.IsAssignableFrom(typeof(string)) == true &&
                captionProperty.CanRead)
            {
                return (string?)captionProperty.GetValue(obj);
            }
            return null;
        }

        /// <summary>
        /// Tries to get the caption of obj with the Mvvm Caption Pattern (Caption, Title, ToString())
        /// </summary>
        /// <param name="obj">The object that is checked for the pattern properties.</param>
        /// <returns>The value of the caption or null.</returns>
        private static string? GetCaptionByPattern(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            var type = obj.GetType();

            // Get the Caption property
            var caption = TryGetCaptionPropertyValue(type, obj, "Caption");
            if (caption != null)
            {
                return caption;
            }

            // Get the Title property
            caption = TryGetCaptionPropertyValue(type, obj, "Title");
            if (caption != null)
            {
                return caption;
            }

            // Get the overridden ToString() method.
            var m = type.GetMethod(nameof(object.ToString), BindingFlags.Public | BindingFlags.Instance);
            if (m != null && m.DeclaringType == type)
            {

                return (string?)m.Invoke(obj, null);
            }

            // No pattern properties found.
            return null;
        }

        /// <summary>
        /// Tries to get the caption of obj or its children with the Mvvm Caption Pattern (Caption, Title, ToString())
        /// </summary>
        /// <param name="obj">The object or one of its children that contains the Mvvm Caption Pattern.</param>
        public static string? GetMvvmCaption(object obj)
        {
            var child = GetMvvmChild(obj);
            if (child == null)
            {
                return GetCaptionByPattern(obj);
            }
            else
            {
                return GetMvvmCaption(child);

            }
        }
    }
}
