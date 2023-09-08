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
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the DispatcherService class.
        /// </summary>
        public DispatcherService()
        {
            var logger = LogManager.GetLog(typeof(DispatcherService));
            // Check if there is an active application before accessing dispatcher
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
                logger?.Info("DispatcherService constructor initialization is completed");
            } else
            {
                logger?.Warn("Application.Current dispatcher is null");
            }
        }

        /// <summary>
        /// Invokes an action asynchronously on the UI thread, if the dispatcher is not null.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>A task that represents the asynchronous operation, or null if the dispatcher is null.</returns>
        public virtual async Task InvokeAsync(System.Action action)
        {
            if (_dispatcher != null)
            {
                // Use the null-conditional operator to invoke the action asynchronously if the dispatcher is not null
                await _dispatcher.InvokeAsync(action);
            }
            else
            {
                action();
            }            
        }
    }
}
