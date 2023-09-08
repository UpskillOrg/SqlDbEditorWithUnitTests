using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.String;

namespace SqlDbEditor.Controls
{
    /// <summary>
    /// NumericalBox allows to type in integer or double numbers only
    /// Verification is made by regular expressions means
    /// </summary>
    public class NumericalBox : TextBox
    {
        #region Verification Patterns
        private static readonly Regex RgxChangedValid =
            new Regex(@"^[-+]?\d*(\.?)(\d*)([eE][-+]?\d*)?$", RegexOptions.ECMAScript);
        private static readonly Regex RgxLostFocusValid =
            new Regex(@"^[-+]?\d*(\.?)(\d+)([eE][-+]?\d+)?$", RegexOptions.ECMAScript);
        private static readonly Regex RgxSaveChanged =
            new Regex(@"[-+]?\d*(\.?)(\d*)([eE][-+]?\d*)?", RegexOptions.ECMAScript);
        private static readonly Regex RgxSaveLost = 
            new Regex(@"[-+]?\d*(\.?)(\d+)([eE][-+]?\d+)?", RegexOptions.ECMAScript);
        #endregion Verification Patterns

        #region Overrides
        /// <summary>
        /// filters numeric keys
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            switch (e.Key)
            {
                case Key.E:
                case Key.OemPlus:
                case Key.OemMinus:
                case Key.OemPeriod:
                case Key.Subtract:
                case Key.Add:
                case Key.Decimal:
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                case Key.Back:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// is called when text box is changed
        /// tries to correct text to longest validated substring
        /// </summary>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (IsTextValid(RgxChangedValid, RgxSaveChanged, Text, out var longestValidText)) return;

            Text = longestValidText;
            CaretIndex = Text.Length;
        }

        /// <summary>
        /// is called when text box looses the focus
        /// tries to correct text to longest validated substring
        /// </summary>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            if (!IsTextValid(RgxLostFocusValid, RgxSaveLost, Text, out var longestValidText))
            {
                Text = longestValidText;
            }
        }
        #endregion Overrides

        #region Private Methods
        /// <summary>
        /// checks text validity and tries to separate most left valid substring
        /// </summary>
        private static bool IsTextValid(Regex rgxValid, Regex rgxSave, string text, out string longestValidSubstr)
        {
            longestValidSubstr = rgxSave.Match(text).Value;
            return !IsNullOrEmpty(text.Trim()) && rgxValid.IsMatch(text);
        }
        #endregion Private Methods
    }

}
