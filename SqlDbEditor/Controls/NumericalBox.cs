using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SqlDbEditor.Controls
{
    /// <summary>
    /// NumericalBox allows to type in integer or double numbers only
    /// Verification is made by regular expressions means
    /// </summary>
    public class NumericalBox : TextBox
    {
        #region Verification Patterns
        private static readonly Regex _rgxChangedValid = new Regex(@"^[-+]?\d*(\.?)(\d*)([eE][-+]?\d*)?$", RegexOptions.ECMAScript);
        private static readonly Regex _rgxLostFocusValid = new Regex(@"^[-+]?\d*(\.?)(\d+)([eE][-+]?\d+)?$", RegexOptions.ECMAScript);
        private static readonly Regex _rgxSaveChanged = new Regex(@"[-+]?\d*(\.?)(\d*)([eE][-+]?\d*)?", RegexOptions.ECMAScript);
        private static readonly Regex _rgxSaveLost = new Regex(@"[-+]?\d*(\.?)(\d+)([eE][-+]?\d+)?", RegexOptions.ECMAScript);
        #endregion Verification Patterns

        #region Overrides
        /// <summary>
        /// filters numeric keys
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
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
        /// is called when textbox is changed
        /// tries to correct text to longest validated substring
        /// </summary>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            string longestValidText;

            if (!IsTextValid(_rgxChangedValid, _rgxSaveChanged, Text, out longestValidText))
            {
                this.Text = longestValidText;
                CaretIndex = Text.Length;
            }
        }

        /// <summary>
        /// is called when textbox looses the focus
        /// tries to correct text to longest validated substring
        /// </summary>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            string longestValidText;

            if (!IsTextValid(_rgxLostFocusValid, _rgxSaveLost, Text, out longestValidText))
            {
                this.Text = longestValidText;
            }
        }
        #endregion Overrides

        #region Private Methods
        /// <summary>
        /// checks text validity and tries to separate most left valid substring
        /// </summary>
        private bool IsTextValid(Regex rgxValid, Regex rgxSave, string text, out string longestValidSubstr)
        {
            longestValidSubstr = rgxSave.Match(text).Value;
            return !string.IsNullOrEmpty(text == null ? null : text.Trim()) ? rgxValid.IsMatch(text) : false;
        }
        #endregion Private Methods
    }

}
