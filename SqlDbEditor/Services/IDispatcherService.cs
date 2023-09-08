using System;
using System.Threading.Tasks;

namespace SqlDbEditor.Services
{
    /// <summary>
    /// An interface that defines methods for invoking actions on the UI thread.
    /// </summary>
    public interface IDispatcherService
    {
        /// <summary>
        /// Invokes an action asynchronously on the UI thread.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task InvokeAsync(Action action);
    }
}
