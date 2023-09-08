using Caliburn.Micro;
using SqlDbEditor.Messages;
using System;

// This application is used for editing SQL databases.
namespace SqlDbEditor
{
    /// <summary>
    /// Represents the entry point of the application.
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Represents a logger configured with the bootstrap
        /// </summary>
        private readonly ILog _logger = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            DispatcherUnhandledException += OnApplicationDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            _logger = LogManager.GetLog(typeof(App));
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if(e.ExceptionObject is Exception exception)
            {
                _logger?.Error(exception);
            }            
        }

        private void OnApplicationDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger?.Error(e.Exception);
            IoC.Get<IEventAggregator>().PublishOnUIThreadAsync(new ErrorMessage() { Message = e.Exception.Message, Type="Global Error Handler" });
        }
    }
}
