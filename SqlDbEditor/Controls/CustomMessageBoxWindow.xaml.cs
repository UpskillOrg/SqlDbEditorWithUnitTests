using System.Drawing;
using System.Windows;

namespace SqlDbEditor.Controls
{
    /// <summary>
    /// Represents a custom message box window with customizable message, buttons, and icon.
    /// </summary>
    internal partial class CustomMessageBoxWindow
    {
        #region Properties

        /// <summary>
        /// Gets or sets the caption (title) of the message box window.
        /// </summary>
        internal string Caption
        {
            get => Title;
            set => Title = value;
        }

        /// <summary>
        /// Gets or sets the message text displayed in the message box.
        /// </summary>
        internal string Message
        {
            get => TextBlockMessage.Text;
            set => TextBlockMessage.Text = value;
        }

        /// <summary>
        /// Gets or sets the text for the "OK" button.
        /// </summary>
        internal string OkButtonText
        {
            get => LabelOk.Content.ToString();
            set => LabelOk.Content = value.TryAddKeyboardAccelerators();
        }

        /// <summary>
        /// Gets or sets the text for the "Cancel" button.
        /// </summary>
        internal string CancelButtonText
        {
            get => LabelCancel.Content.ToString();
            set => LabelCancel.Content = value.TryAddKeyboardAccelerators();
        }

        /// <summary>
        /// Gets or sets the text for the "Yes" button.
        /// </summary>
        internal string YesButtonText
        {
            get => LabelYes.Content.ToString();
            set => LabelYes.Content = value.TryAddKeyboardAccelerators();
        }

        /// <summary>
        /// Gets or sets the text for the "No" button.
        /// </summary>
        internal string NoButtonText
        {
            get => LabelNo.Content.ToString();
            set => LabelNo.Content = value.TryAddKeyboardAccelerators();
        }

        /// <summary>
        /// Gets or sets the result of the message box (e.g., OK, Cancel, Yes, No).
        /// </summary>
        public MessageBoxResult Result { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMessageBoxWindow"/> class with specified message, caption, buttons, and icon.
        /// </summary>
        /// <param name="message">The message text displayed in the message box.</param>
        /// <param name="caption">The caption (title) of the message box window.</param>
        /// <param name="button">The buttons to be displayed in the message box.</param>
        /// <param name="image">The icon to be displayed in the message box.</param>
        internal CustomMessageBoxWindow(string message, string caption, MessageBoxButton button, MessageBoxImage image)
        {
            InitializeComponent();

            Message = message;
            Caption = caption;
            ImageMessageBox.Visibility = Visibility.Collapsed;

            DisplayImage(image);
            DisplayButtons(button);
        }

        #endregion

        #region Methods

        private void DisplayButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OKCancel:
                    // Hide all but OK, Cancel
                    ButtonOk.Visibility = Visibility.Visible;
                    ButtonOk.Focus();
                    ButtonCancel.Visibility = Visibility.Visible;

                    ButtonYes.Visibility = Visibility.Collapsed;
                    ButtonNo.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.YesNo:
                    // Hide all but Yes, No
                    ButtonYes.Visibility = Visibility.Visible;
                    ButtonYes.Focus();
                    ButtonNo.Visibility = Visibility.Visible;

                    ButtonOk.Visibility = Visibility.Collapsed;
                    ButtonCancel.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.YesNoCancel:
                    // Hide only OK
                    ButtonYes.Visibility = Visibility.Visible;
                    ButtonYes.Focus();
                    ButtonNo.Visibility = Visibility.Visible;
                    ButtonCancel.Visibility = Visibility.Visible;

                    ButtonOk.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.OK:
                default:
                    // Hide all but OK
                    ButtonOk.Visibility = Visibility.Visible;
                    ButtonOk.Focus();

                    ButtonYes.Visibility = Visibility.Collapsed;
                    ButtonNo.Visibility = Visibility.Collapsed;
                    ButtonCancel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void DisplayImage(MessageBoxImage image)
        {
            Icon icon;

            switch (image)
            {
                case MessageBoxImage.None:
                    return;

                case MessageBoxImage.Exclamation:       // Enumeration value 48 - also covers "Warning"
                    icon = SystemIcons.Exclamation;
                    break;

                case MessageBoxImage.Error:             // Enumeration value 16, also covers "Hand" and "Stop"
                    icon = SystemIcons.Hand;
                    break;

                case MessageBoxImage.Information:       // Enumeration value 64 - also covers "Asterisk"
                    icon = SystemIcons.Information;
                    break;

                case MessageBoxImage.Question:
                    icon = SystemIcons.Question;
                    break;

                default:
                    icon = SystemIcons.Information;
                    break;
            }

            ImageMessageBox.Source = icon.ToImageSource();
            ImageMessageBox.Visibility = Visibility.Visible;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void Button_Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void Button_No_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        #endregion
    }
}
