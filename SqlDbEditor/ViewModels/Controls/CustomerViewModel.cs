using Caliburn.Micro;
using SqlDbEditor.Messages;
using SqlDbEditor.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Profile;
using static SqlDbEditor.DataAccessLayer.LinkTekTest;

namespace SqlDbEditor.ViewModels.Controls
{
    /// <summary>
    /// ViewModel for the CustomerView control.
    /// </summary>
    public class CustomerViewModel : PropertyChangedBase, ICustomerViewModel
    {
        #region Private Fields
        /// <summary>
        /// Represents a private field for event aggregator
        /// </summary>
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Represents a private field for the customer data service interface
        /// </summary>
        private ICustomerDataService _customerDataService;

        /// <summary> 
        /// A flag to indicate whether the object is disposed
        ///</summary> 
        private bool disposed = false;

        /// <summary> 
        /// Represents the dispatcher service.  
        ///</summary> 
        private IDispatcherService _dispatcherService = null;

        /// <summary> 
        /// Represents the customer data table. 
        /// </summary> 
        private CustomerDataTable customerTable = null;

        /// <summary> 
        /// Represents the flag that indicates if customers can be loaded. 
        /// </summary> 
        private bool canLoadCustomers = true;

        /// <summary> 
        /// Represents the window manager. 
        /// </summary> 
        private IWindowManager _windowManager = null;

        /// <summary> 
        /// Represents the customer edit view model. 
        /// </summary> 
        private ICustomerEditViewModel _customerEditViewModel = null;

        /// <summary>
        /// Represents the logger
        /// </summary>
        private ILog _logger;
        #endregion

        #region Constrcutor
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerViewModel"/> class.
        /// </summary>
        /// <param name="customerDataService">The customer data service.</param>
        /// <param name="dispatcherService">The dispatcher service.</param>
        /// <param name="windowManager">The window manager.</param>
        /// <param name="customerEditViewModel">The customer edit view model.</param>
        public CustomerViewModel(
            ICustomerDataService customerDataService,
            IDispatcherService dispatcherService,
            IWindowManager windowManager,
            ICustomerEditViewModel customerEditViewModel,
            IEventAggregator eventAggregator
        )
        {
            _eventAggregator = eventAggregator;
            _customerDataService = customerDataService;
            _dispatcherService = dispatcherService;
            _customerEditViewModel = customerEditViewModel;
            _windowManager = windowManager;
            _logger = _logger = LogManager.GetLog(typeof(CustomerViewModel)); ;
            _logger?.Info("CustomerViewModel contractor initialization is completed.");
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether customers can be loaded.
        /// </summary>
        public bool CanLoadCustomers
        {
            get => canLoadCustomers;
            set
            {
                canLoadCustomers = value;
                NotifyOfPropertyChange(() => CanLoadCustomers);
            }
        }

        /// <summary>
        /// Gets or sets the customer table.
        /// </summary>
        public CustomerDataTable CustomerTable
        {
            get => customerTable;
            set
            {
                customerTable = value;
                NotifyOfPropertyChange(nameof(CustomerTable));
                _logger?.Info("CustomerTable filed is assigned in the CustomerViewModel");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the customers asynchronously.
        /// </summary>
        public async Task LoadCustomers()
        {
            CustomerTable?.Clear();
            CanLoadCustomers = false;

            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    _logger?.Info("Start fetching the customer information from the database");
                    var customers = _customerDataService.CustomerTableAdapter.GetCustomers();
                    _dispatcherService.Invoke(() => CustomerTable = customers);
                    _logger?.Info("Completed fetching the customer information from the database");
                }
                catch (SqlException ex)
                {
                    await PublishOnUIThreadAsync(new ErrorMessage { Message = ex.Message, Type = "Sql Connection Error" });
                }
                catch (Exception ex)
                {
                    _logger?.Error(ex);
                }
            });

            CanLoadCustomers = true;
        }
        
        /// <summary>
        /// Publish the error message through the event aggregator
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public virtual Task  PublishOnUIThreadAsync(ErrorMessage errorMessage)
        {
            return _eventAggregator.PublishOnUIThreadAsync(errorMessage);
        }

        /// <summary>
        /// Edits an item in the customer table.
        /// </summary>
        /// <param name="rowView">The row view of the item to be edited.</param>
        public async Task EditItem(DataRowView rowView)
        {
            try
            {
                _logger?.Info("Started to show the edit dialog");
                _customerEditViewModel.CustomerRow = rowView;
                _logger?.Info("Start populating the fields in the edit form");
                bool filledCustomer = _customerEditViewModel.FillCustomer();

                if (filledCustomer)
                {
                    _logger?.Info("Complete populating the fields in the edit form");
                    var result = await _windowManager.ShowDialogAsync(_customerEditViewModel);
                    _logger?.Info("Edit dialog is closed and result the dialog returns is:" + result);
                }
                else
                {
                    _logger?.Info("Unable to populate the fields in the edit form");
                }
            }
            catch (Exception ex)
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
            _logger?.Info("Customer view model is disposed");
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
            if (!disposed)
            {
                // If disposing is true, release managed resources
                if (disposing)
                {
                    _customerDataService.Dispose();
                }

                // Set the flag to true
                disposed = true;
            }
        }
        #endregion
    }
}