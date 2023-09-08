using Caliburn.Micro;
using SqlDbEditor.Command;
using SqlDbEditor.Messages;
using SqlDbEditor.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
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
        /// Represents whether the data loaded first time with the manual invocation
        /// </summary>
        private bool _loadInitialized;

        /// <summary> 
        /// A flag to indicate whether the object is disposed
        ///</summary> 
        private bool _disposed;

        /// <summary> 
        /// Represents the customer data table. 
        /// </summary> 
        private CustomerDataTable _customerTable;

        /// <summary> 
        /// Represents the flag that indicates if customers can be loaded. 
        /// </summary> 
        private bool _canLoadCustomers = true;

        /// <summary>
        /// This field indicates the number of items you want to display on each page of a paginated list. 
        /// For example, if you're displaying a list of customer records and pageSize is 
        /// set to 10, each page will show 10 customer records.
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// This field represents the starting index (offset) of the current page in the paginated list. 
        /// It helps in determining which subset of data to display for the current page.
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// This field reflects the total number of customers in the entire data-set or list. 
        /// It provides the count of all available customers, irrespective of how they are divided across pages.
        /// </summary>
        private int? _totalCustomers;

        /// <summary>
        /// Represents a private field for event aggregator service
        /// </summary>
        private readonly IEventAggregatorService _eventAggregatorService;

        /// <summary>
        /// Represents a private field for the customer data service interface
        /// </summary>
        private readonly ICustomerDataService _customerDataService;

        /// <summary> 
        /// Represents the dispatcher service.  
        ///</summary> 
        private readonly IDispatcherService _dispatcherService;

        /// <summary> 
        /// Represents the window manager. 
        /// </summary> 
        private readonly IWindowManager _windowManager;

        /// <summary> 
        /// Represents the customer edit view model. 
        /// </summary> 
        private readonly ICustomerEditViewModel _customerEditViewModel;

        /// <summary>
        /// Represents the logger
        /// </summary>
        private readonly ILog _logger;

        private bool _isEditInProgress;

        #endregion

        #region Constrcutor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerViewModel"/> class.
        /// </summary>
        /// <param name="customerDataService">The customer data service.</param>
        /// <param name="dispatcherService">The dispatcher service.</param>
        /// <param name="windowManager">The window manager.</param>
        /// <param name="customerEditViewModel">The customer edit view model.</param>
        /// <param name="eventAggregatorService">The proxy for IEventAggregator</param>
        public CustomerViewModel(
            ICustomerDataService customerDataService,
            IDispatcherService dispatcherService,
            IWindowManager windowManager,
            ICustomerEditViewModel customerEditViewModel,
            IEventAggregatorService eventAggregatorService
        )
        {
            _eventAggregatorService = eventAggregatorService;
            _customerDataService = customerDataService;
            _dispatcherService = dispatcherService;
            _customerEditViewModel = customerEditViewModel;
            _windowManager = windowManager;
            _logger = _logger = LogManager.GetLog(typeof(CustomerViewModel));
            ChangeIndexCommand = new ChangeIndexCommand(ChangeIndexAction);
            _logger?.Info("CustomerViewModel contractor initialization is completed.");
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Change Index Command
        /// </summary>
        public ICommand ChangeIndexCommand { get; set; }

        /// <summary>
        /// This field reflects the total number of customers in the entire data-set or list. 
        /// It provides the count of all available customers, irrespective of how they are divided across pages.
        /// </summary>
        public int? TotalCustomers
        {
            get => _totalCustomers;
            set
            {
                _totalCustomers = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// If the row is edited then this will set to true otherwise false
        /// </summary>
        public bool IsEditInProgress
        {
            get => _isEditInProgress;
            set
            {
                _isEditInProgress = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// This field represents the starting index (offset) of the current page in the paginated list. 
        /// It helps in determining which subset of data to display for the current page.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// This field represents the starting index (offset) of the current page in the paginated list. 
        /// It helps in determining which subset of data to display for the current page.
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                _pageIndex = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers can be loaded.
        /// </summary>
        public bool CanLoadCustomers
        {
            get => _canLoadCustomers;
            set
            {
                _canLoadCustomers = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the customer table.
        /// </summary>
        public CustomerDataTable CustomerTable
        {
            get => _customerTable;
            set
            {
                _customerTable = value;
                NotifyOfPropertyChange();
                _logger?.Info("CustomerTable filed is assigned in the CustomerViewModel");
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method will be called once the corresponding view gets loaded
        /// </summary>
        public async Task Loaded()
        {
            if (!_loadInitialized)
            {
                PageIndex = 0;
                PageSize = 100;
                return;
            }

            CanLoadCustomers = false;

            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    _logger?.Info("Start fetching the customer count from the database");                    
                    var totalCustomers = _customerDataService.CustomerTableAdapter.GetCoustomersCount();
                    await _dispatcherService.InvokeAsync(() => TotalCustomers = totalCustomers);
                    _logger?.Info("Completed fetching the customer count from the database");
                }
                catch (SqlException ex)
                {
                    await _eventAggregatorService.PublishOnUIThreadAsync(new ErrorMessage { Message = ex.Message, Type = "Sql Connection Error" });
                }
                catch (Exception ex)
                {
                    _logger?.Error(ex);
                }
            });            

            CanLoadCustomers = true;
        }

        /// <summary>
        /// Loads the customers asynchronously.
        /// </summary>
        public async Task LoadCustomers()
        {
            _loadInitialized = true;
            CanLoadCustomers = false;
            
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    _logger?.Info("Start fetching the customer information from the database");
                    var customers = _customerDataService.CustomerTableAdapter.GetCustomers(PageIndex, PageSize);
                    await _dispatcherService.InvokeAsync(
                        () => {
                            CustomerTable?.Clear();
                            CustomerTable = customers;                            
                        }
                    );
                    _logger?.Info("Completed fetching the customer information from the database");
                }
                catch (SqlException ex)
                {
                    await _eventAggregatorService.PublishOnUIThreadAsync(new ErrorMessage { Message = ex.Message, Type = "Sql Connection Error" });
                }
                catch (Exception ex)
                {
                    _logger?.Error(ex);
                }
            });

            await Loaded();

            CanLoadCustomers = true;
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
                var filledCustomer = _customerEditViewModel.FillCustomer();

                if (filledCustomer)
                {
                    _logger?.Info("Complete populating the fields in the edit form");
                    IsEditInProgress = true;
                    var result = await _windowManager.ShowDialogAsync(_customerEditViewModel);
                    IsEditInProgress = false;
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

        #region Private Methods
        private async Task InternalLoadCustomers()
        {
            if (_loadInitialized)
            {
                await Loaded();
                await LoadCustomers();
            }
        }

        private async void ChangeIndexAction()
        {
            await InternalLoadCustomers();
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
            if (!_disposed)
            {
                // If disposing is true, release managed resources
                if (disposing)
                {
                    _customerDataService.Dispose();
                }

                // Set the flag to true
                _disposed = true;
            }
        }
        #endregion
    }
}