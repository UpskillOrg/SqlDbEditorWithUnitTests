using Microsoft.Xaml.Behaviors;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace SqlDbEditor.Behaviors
{
    /// <summary>
    /// Represents a behavior for validating and controlling input in a TextBox control.
    /// </summary>
    public class TextBoxInputBehavior : Behavior<TextBox>
    {
        #region Constant
        const NumberStyles validNumberStyles =
            NumberStyles.AllowDecimalPoint |
            NumberStyles.AllowThousands |
            NumberStyles.AllowLeadingSign;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the TextBoxInputBehavior class.
        /// </summary>
        public TextBoxInputBehavior()
        {
            InputMode = TextBoxInputMode.None;
            JustPositiveDecimalInput = false;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the input mode for the behavior.
        /// </summary>
        public TextBoxInputMode InputMode { get; set; }
        #endregion

        #region Dependency Properties
        /// <summary>
        /// Gets or sets a value indicating whether only positive decimal input is allowed.
        /// </summary>
        public static readonly DependencyProperty JustPositiveDecimalInputProperty =
         DependencyProperty.Register("JustPositiveDecimalInput", typeof(bool),
         typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(false));

        public bool JustPositiveDecimalInput
        {
            get { return (bool)GetValue(JustPositiveDecimalInputProperty); }
            set { SetValue(JustPositiveDecimalInputProperty, value); }
        }
        #endregion

        #region Protected Methods
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;

            DataObject.AddPastingHandler(AssociatedObject, Pasting);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;

            DataObject.RemovePastingHandler(AssociatedObject, Pasting);
        }
        #endregion

        #region Private Methods
        private void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pastedText = (string)e.DataObject.GetData(typeof(string));

                if (!this.IsValidInput(this.GetText(pastedText)))
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.CancelCommand();
                }
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                e.CancelCommand();
            }
        }

        private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!this.IsValidInput(this.GetText(" ")))
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!this.IsValidInput(this.GetText(e.Text)))
            {
                System.Media.SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Gets the updated text after applying the input.
        /// </summary>
        /// <param name="input">The input to be applied to the text.</param>
        /// <returns>The updated text.</returns>
        private string GetText(string input)
        {
            var txt = this.AssociatedObject;

            int selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
                selectionStart = txt.Text.Length;

            int selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
                selectionLength = txt.Text.Length - selectionStart;

            var realtext = txt.Text.Remove(selectionStart, selectionLength);

            int caretIndex = txt.CaretIndex;
            if (realtext.Length < caretIndex)
                caretIndex = realtext.Length;

            var newtext = realtext.Insert(caretIndex, input);

            return newtext;
        }

        /// <summary>
        /// Checks if the input is valid based on the input mode.
        /// </summary>
        /// <param name="input">The input to be validated.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        private bool IsValidInput(string input)
        {
            switch (InputMode)
            {
                case TextBoxInputMode.None:
                    return true;
                case TextBoxInputMode.DigitInput:
                    return CheckIsDigit(input);

                case TextBoxInputMode.DecimalInput:
                    decimal d;
                    if (input.ToCharArray().Where(x => x == ',').Count() > 1)
                        return false;


                    if (input.Contains("-"))
                    {
                        if (JustPositiveDecimalInput)
                            return false;


                        if (input.IndexOf("-", StringComparison.Ordinal) > 0)
                            return false;

                        if (input.ToCharArray().Count(x => x == '-') > 1)
                            return false;

                        if (input.Length == 1)
                            return true;
                    }

                    var result = decimal.TryParse(input, validNumberStyles, CultureInfo.CurrentCulture, out d);
                    return result;



                default: throw new ArgumentException("Unknown TextBoxInputMode");

            }
        }

        private bool CheckIsDigit(string input)
        {
            return input.ToCharArray().All(Char.IsDigit);
        }
        #endregion
    }
}