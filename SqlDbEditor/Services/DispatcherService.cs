using Caliburn.Micro;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SqlDbEditor.Services
{
    /// <summary>
    /// A class that implements the IDispatcherService interface using the WPF Dispatcher.
    /// </summary>
    public class DispatcherService : IDispatcherService
    {
        /// <summary>
        /// A field that holds a reference to the dispatcher, or null if there is no active application
        /// </summary>
        private Dispatcher _dispatcher;

        /// <summary>
        /// Represents a logger configured with the bootstrap
        /// </summary>
        private ILog _logger = null;

        /// <summary>
        /// Initializes a new instance of the DispatcherService class.
        /// </summary>
        public DispatcherService()
        {
            _logger = LogManager.GetLog(typeof(CustomerDataService));
            // Check if there is an active application before accessing dispatcher
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
                _logger?.Info("DispatcherService constructor initialization is completed");
            } else
            {
                _logger?.Warn("Application.Current dispatcher is null");
            }
        }

        /// <summary>
        /// Invokes an action synchronously on the UI thread, if the dispatcher is not null.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        public void Invoke(System.Action action)
        {
            // Use the null-conditional operator to invoke the action if the dispatcher is not null
            _dispatcher?.Invoke(action);
        }

        /// <summary>
        /// Invokes an action asynchronously on the UI thread, if the dispatcher is not null.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>A task that represents the asynchronous operation, or null if the dispatcher is null.</returns>
        public async Task InvokeAsync(System.Action action)
        {
            // Use the null-conditional operator to invoke the action asynchronously if the dispatcher is not null
            await _dispatcher?.InvokeAsync(action);
        }
    }
}
