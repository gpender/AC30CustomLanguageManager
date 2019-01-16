using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AC30CustomLanguageManager.Resources.Converters
{
    /// <summary>   
    /// A type converter for visibility and boolean values.   
    /// </summary>   
    public class VisToBool : IValueConverter
    {

        private bool _inverted = false;
        private bool _not = false;

        public bool Inverted
        {
            get { return _inverted; }
            set { _inverted = value; }
        }

        public bool Not
        {
            get { return _not; }
            set { _not = value; }
        }

        private object VisibilityToBool(object value)
        {
            if (!(value is Visibility))
                return DependencyProperty.UnsetValue;
            return (((Visibility)value) == Visibility.Visible) ^ Not;
        }

        private object BoolToVisibility(object value)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            return ((bool)value ^ Not) ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverted ? BoolToVisibility(value)
                : VisibilityToBool(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverted ? VisibilityToBool(value)
                : BoolToVisibility(value);
        }
    }
}
