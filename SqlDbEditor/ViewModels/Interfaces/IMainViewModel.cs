using SqlDbEditor.ViewModels.Controls;
using System;

namespace SqlDbEditor.ViewModels
{
    /// <summary>
    /// Represents a main view model for the application.
    /// </summary>
    public interface IMainViewModel : IDisposable
    {
        /// <summary>
        /// Gets or sets the customer view model associated with the main view model.
        /// </summary>
        ICustomerViewModel CustomerViewModel { get; set; }
    }
}