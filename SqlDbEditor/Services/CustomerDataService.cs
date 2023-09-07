using Caliburn.Micro;
using SqlDbEditor.DataAccessLayer.LinkTekTestTableAdapters;
using System;

namespace SqlDbEditor.Services
{
    /// <summary>
    /// Represents a service for accessing customer data.
    /// </summary>
    public class CustomerDataService : ICustomerDataService, IDisposable
    {
        #region Private Fields
        /// <summary>
        /// A flag to indicate whether the object is disposed
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Represents a logger configured with the bootstrap
        /// </summary>
        private ILog _logger = null;
        #endregion
        
        #region Public Fields
        /// <summary>
        /// Gets or sets the customer table adapter.
        /// </summary>
        public CustomerTableAdapter CustomerTableAdapter { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerDataService"/> class.
        /// </summary>
        public CustomerDataService()
        {
            try
            {
                _logger = LogManager.GetLog(typeof(CustomerDataService));
                // Create an instance of CustomerTableAdapter to access customer data
                CustomerTableAdapter = new CustomerTableAdapter();
                _logger?.Info("CustomerDataService contructor is initialized");
            }
            catch(Exception ex)
            {
                _logger?.Error(ex);
            }
            
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// The Dispose() method that implements IDisposable
        /// </summary>
        public void Dispose()
        {
            // Call the Dispose(bool disposing) method with true argument
            Dispose(true);
            _logger?.Info("CustomerDataService is disposed");
            // Suppress the finalization of this object
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            // Check if the object is already disposed
            if (!disposed)
            {
                // If disposing is true, release managed resources
                if (disposing)
                {
                    CustomerTableAdapter.Dispose();
                    _logger?.Info("CustomerTableAdapter is disposed");
                }

                // Set the flag to true
                disposed = true;
            }
        }
        #endregion
    }
}