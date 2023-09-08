using Caliburn.Micro;
using SqlDbEditor.ViewModels.Controls;
using System;

namespace SqlDbEditor.ViewModels
{
    /// <summary>
    /// The main view model for the application.
    /// </summary>
    public class MainViewModel : PropertyChangedBase, IMainViewModel
    {
        #region Private Field
        /// <summary> 
        /// A flag to indicate whether the object is disposed
        ///</summary> 
        private bool _disposed;

        /// <summary>
        /// Represents a view model for a customer.
        /// </summary>
        private ICustomerViewModel _customerViewModel;

        /// <summary>
        /// Represents the logger
        /// </summary>
        private readonly ILog _logger;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="viewModel">The customer view model.</param>
        public MainViewModel(ICustomerViewModel viewModel)
        {
            _logger = LogManager.GetLog(typeof(MainViewModel));
            CustomerViewModel = viewModel;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the customer view model.
        /// </summary>
        public ICustomerViewModel CustomerViewModel
        {
            get => _customerViewModel;
            set
            {
                _customerViewModel = value;
                NotifyOfPropertyChange();
                _logger?.Info("CustomerViewModel is assigned in the Main View Model");
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// The Dispose() method that implements IDisposable
        /// </summary>
        public virtual void Dispose()
        {
            // Call the Dispose(bool disposing) method with true argument
            Dispose(true);
            // Suppress the finalization of this object
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the CustomerViewModel and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">A boolean value that indicates whether the method call comes from a Dispose method (if true) or from a finalizer (if false).</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check if the object is already disposed
            if (!_disposed)
            {
                // If disposing is true, release managed resources
                if (disposing)
                {
                    _customerViewModel.Dispose();
                }

                // Set the flag to true
                _disposed = true;
            }
        }
        #endregion
    }
}