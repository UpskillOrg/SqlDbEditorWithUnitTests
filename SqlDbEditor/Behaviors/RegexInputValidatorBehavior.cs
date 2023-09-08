using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace SqlDbEditor.Behaviors
{
    public class RegexInputValidatorBehavior : Behavior<TextBox>
    {
        public string MatchRegEx
        {
            get => (string)GetValue(MatchRegExProperty);
            set => SetValue(MatchRegExProperty, value);
        }

        public static readonly DependencyProperty MatchRegExProperty =
            DependencyProperty.Register(
                nameof(MatchRegEx), 
                typeof(string), 
                typeof(RegexInputValidatorBehavior), 
                new PropertyMetadata(string.Empty)
            );

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
            if (IsValidInput(GetText(e.Text))) return;

            System.Media.SystemSounds.Beep.Play();
            e.Handled = true;
        }

        /// <summary>
        /// Gets the updated text after applying the input.
        /// </summary>
        /// <param name="input">The input to be applied to the text.</param>
        /// <returns>The updated text.</returns>
        private string GetText(string input)
        {
            var txt = this.AssociatedObject;

            var selectionStart = txt.SelectionStart;
            if (txt.Text.Length < selectionStart)
                selectionStart = txt.Text.Length;

            var selectionLength = txt.SelectionLength;
            if (txt.Text.Length < selectionStart + selectionLength)
                selectionLength = txt.Text.Length - selectionStart;

            var realText = txt.Text.Remove(selectionStart, selectionLength);

            var caretIndex = txt.CaretIndex;
            if (realText.Length < caretIndex)
                caretIndex = realText.Length;

            var newText = realText.Insert(caretIndex, input);

            return newText;
        }

        /// <summary>
        /// Checks if the input is valid based on the input mode.
        /// </summary>
        /// <param name="input">The input to be validated.</param>
        /// <returns>True if the input is valid, otherwise false.</returns>
        private bool IsValidInput(string input)
        {
            return Regex.Match(input, MatchRegEx).Success;
        }
    }
}
