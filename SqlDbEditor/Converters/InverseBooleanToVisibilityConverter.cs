using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SqlDbEditor.Converters
{
    /// <summary>
    /// A value converter that converts a bool value to a Visibility value, inverting the logic of the BooleanToVisibilityConverter.
    /// </summary>
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a bool value to a Visibility value, inverting the logic of the BooleanToVisibilityConverter.
        /// </summary>
        /// <param name="value">The bool value to convert.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">An optional parameter to pass to the converter. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>A Visibility value that is Visible if the bool value is false, and Collapsed if the bool value is true.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a bool
            if (value is bool boolValue)
            {
                // Return the opposite of the built-in BooleanToVisibilityConverter logic
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                // Return the default value for Visibility
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// Converts a Visibility value to a bool value, inverting the logic of the BooleanToVisibilityConverter.
        /// </summary>
        /// <param name="value">The Visibility value to convert.</param>
        /// <param name="targetType">The type of the binding target property. This parameter is not used.</param>
        /// <param name="parameter">An optional parameter to pass to the converter. This parameter is not used.</param>
        /// <param name="culture">The culture to use in the converter. This parameter is not used.</param>
        /// <returns>A bool value that is false if the Visibility value is Visible, and true if the Visibility value is Collapsed.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a Visibility
            if (value is Visibility visibilityValue)
            {
                // Return the opposite of the built-in BooleanToVisibilityConverter logic
                return visibilityValue != Visibility.Visible;
            }
            else
            {
                // Return the default value for bool
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
