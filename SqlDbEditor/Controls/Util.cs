using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SqlDbEditor.Controls
{
    /// <summary>
    /// Utility class containing helper methods for various tasks.
    /// </summary>
    internal static class Util
    {
        /// <summary>
        /// Converts a Windows Forms <see cref="Icon"/> into a WPF <see cref="ImageSource"/>.
        /// </summary>
        /// <param name="icon">The Windows Forms icon to convert.</param>
        /// <returns>An <see cref="ImageSource"/> representing the converted icon.</returns>
        internal static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        /// <summary>
        /// Checks a string for the presence of a keyboard accelerator character.
        /// If the string doesn't contain an accelerator character, it adds one at the beginning.
        /// The keyboard accelerator character for WPF is an underscore (_).
        /// </summary>
        /// <param name="input">The input string to check for the accelerator character.</param>
        /// <returns>
        /// The input string with a keyboard accelerator character added at the beginning, if necessary.
        /// </returns>
        internal static string TryAddKeyboardAccelerators(this string input)
        {
            const string accelerator = "_";
            // If it already contains an accelerator, do nothing
            if (input.Contains(accelerator)) return input;

            return accelerator + input;
        }
    }
}