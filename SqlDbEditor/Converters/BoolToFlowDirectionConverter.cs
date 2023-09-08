using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SqlDbEditor.Converters
{
    /// <summary>
    /// A converter that converts a boolean value to the FlowDirection property of a ProgressBar.
    /// </summary>
    public class BoolToFlowDirectionConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to the FlowDirection property.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type of the binding target property (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>The FlowDirection value based on the input boolean.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
            {
                return FlowDirection.RightToLeft; // RightToLeft for true
            }

            return FlowDirection.LeftToRight; // LeftToRight for false or other values
        }

        /// <summary>
        /// This method is not supported and will always throw a NotSupportedException.
        /// </summary>
        /// <param name="value">The value to convert back (not used).</param>
        /// <param name="targetType">The target type to convert to (not used).</param>
        /// <param name="parameter">An optional parameter (not used).</param>
        /// <param name="culture">The culture to use in the converter (not used).</param>
        /// <returns>Throws a NotSupportedException.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
