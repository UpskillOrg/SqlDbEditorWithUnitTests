using System.Threading;
using System.Windows;

namespace SqlDbEditor.Controls
{
    /// <summary>
    /// Represents data for displaying a customized message box in a WPF application.
    /// </summary>
    internal class MessageBoxData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the owner window of the message box.
        /// </summary>
        public Window Owner { get; set; }

        /// <summary>
        /// Gets or sets the message text to display in the message box.
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// Gets or sets the caption or title of the message box.
        /// </summary>
        public string Caption { get; set; } = "Message";

        /// <summary>
        /// Gets or sets the buttons to display in the message box.
        /// </summary>
        public MessageBoxButton Buttons { get; set; } = MessageBoxButton.OK;

        /// <summary>
        /// Gets or sets the image icon to display in the message box.
        /// </summary>
        public MessageBoxImage Image { get; set; } = MessageBoxImage.None;

        /// <summary>
        /// Gets or sets the custom caption for the "Yes" button in the message box.
        /// </summary>
        public string YesButtonCaption { get; set; }

        /// <summary>
        /// Gets or sets the custom caption for the "No" button in the message box.
        /// </summary>
        public string NoButtonCaption { get; set; }

        /// <summary>
        /// Gets or sets the custom caption for the "Cancel" button in the message box.
        /// </summary>
        public string CancelButtonCaption { get; set; }

        /// <summary>
        /// Gets or sets the custom caption for the "OK" button in the message box.
        /// </summary>
        public string OkButtonCaption { get; set; }

        /// <summary>
        /// Gets or sets the result of the message box dialog.
        /// </summary>
        public MessageBoxResult Result { get; set; } = MessageBoxResult.None;

        #endregion

        #region Methods

        /// <summary>
        /// Displays a message box that is defined by the properties of this class.
        /// In case the current thread is not a STA thread already,
        /// a new STA thread is being created and the MessageBox is being displayed from there.
        /// </summary>
        /// <returns>The result of the message box dialog.</returns>
        public MessageBoxResult ShowMessageBox()
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                ShowMessageBoxSta();
            }
            else
            {
                var thread = new Thread(ShowMessageBoxSta);
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            return Result;
        }

        /// <summary>
        /// Displays the message box in a single-threaded apartment (STA) thread.
        /// </summary>
        private void ShowMessageBoxSta()
        {
            var msg = new CustomMessageBoxWindow(Message, Caption, Buttons, Image);

            msg.YesButtonText = YesButtonCaption ?? msg.YesButtonText;
            msg.NoButtonText = NoButtonCaption ?? msg.NoButtonText;
            msg.CancelButtonText = CancelButtonCaption ?? msg.CancelButtonText;
            msg.OkButtonText = OkButtonCaption ?? msg.OkButtonText;
            msg.Owner = Owner ?? msg.Owner;

            msg.ShowDialog();

            Result = msg.Result;
        }

        #endregion
    }
}
