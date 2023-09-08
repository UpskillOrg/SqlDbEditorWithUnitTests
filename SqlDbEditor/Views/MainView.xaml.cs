using Caliburn.Micro;
using SqlDbEditor.Messages;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using SqlDbEditor.Controls;

namespace SqlDbEditor.Views
{
    /// <summary>
    /// Represents the main window of the application.
    /// </summary>
    public partial class MainView : IHandle<ErrorMessage>
    {
        /// <summary>
        /// Initializes a new instance of the MainView class.
        /// </summary>
        public MainView()
        {            
            InitializeComponent();
            var eventAggregator = IoC.Get<IEventAggregator>();
            eventAggregator.SubscribeOnUIThread(this);
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseWindow));
        }

        public async Task HandleAsync(ErrorMessage message, CancellationToken cancellationToken)
        {
            CustomMessageBox.Show(message.Message, message.Type);
            await Task.CompletedTask;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            CloseWindow(sender, null);
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButtonClick(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        private void ToggleWindowState()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 1)
            {
                if (WindowState == WindowState.Maximized)
                {
                    WindowState = WindowState.Normal;
                    Height = SystemParameters.PrimaryScreenHeight;
                    Width = SystemParameters.PrimaryScreenWidth;
                }
                DragMove();
            } else if(e.ClickCount == 2)
            {
                ToggleWindowState();
            }            
        }
    }
}