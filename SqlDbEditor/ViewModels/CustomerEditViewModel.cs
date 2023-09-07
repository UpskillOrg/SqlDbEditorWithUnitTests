using Caliburn.Micro;
using SqlDbEditor.Messages;
using SqlDbEditor.RegEx;
using SqlDbEditor.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlDbEditor.ViewModels
{
    /// <summary> 
    /// Represents the view model for editing customer details. 
    /// </summary> 
    public class CustomerEditViewModel : Screen, ICustomerEditViewModel
    {
        #region Private Fields
        /// <summary>
        /// Represents that whether the required validations are satisfied and the data can be updated to the customer table
        /// </summary>
        private bool _canOk = false;

        /// <summary>
        /// Represents whether the customer update is completed or not
        /// </summary>
        private bool _isUpdateProgress;

        /// <summary>
        /// Represents a State provider service
        /// </summary>
        private IStateProviderService _stateProviderService;

        /// <summary>
        /// Represents a event aggregator
        /// </summary>
        private IEventAggregator _eventAggregator;

        /// <summary> 
        /// A flag to indicate whether the object is disposed
        ///</summary> 
        private bool disposed = false;

        /// <summary>
        /// Represents the customer data service.
        /// </summary>
        private ICustomerDataService _customerDataService = null;

        /// <summary>
        /// Represents the current customer row.
        /// </summary>
        private DataRowView _customerRow;

        /// <summary>
        /// A field that holds the selected state of the customer.
        /// </summary>
        private string _selectedState;

        /// <summary>
        /// A field that holds the first name of the customer.
        /// </summary>
        private string _firstName;

        /// <summary>
        /// A field that holds the last name of the customer.
        /// </summary>
        private string _lastName;

        /// <summary>
        /// A field that holds the first line of the address of the customer.
        /// </summary>
        private string _address1;

        /// <summary>
        /// A field that holds the second line of the address of the customer, if any.
        /// </summary>
        private string _address2;

        /// <summary>
        /// A field that holds the city of the customer.
        /// </summary>
        private string _city;

        /// <summary>
        /// A field that holds the zip code of the customer.
        /// </summary>
        private string _zip;

        /// <summary>
        /// A field that holds the phone number of the customer.
        /// </summary>
        private string _phone;

        /// <summary>
        /// A field that holds the age of the customer in years.
        /// </summary>
        private string _age;

        /// <summary>
        /// A field that holds the total sales amount of the customer in dollars.
        /// </summary>
        private string _sales;

        /// <summary>
        /// Represents a logger configured with the bootstrap
        /// </summary>
        private ILog _logger = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the CustomerEditViewModel class with the specified data row view and customer data service.
        /// </summary>       
        /// <param name="customerDataService">The customer data service that provides access to the database.</param>
        /// <param name="stateProviderService">The state provider service that provides all available states in US.</param>
        /// <param name="eventAggregator">Represents the instance of event aggregator.</param>
        public CustomerEditViewModel(ICustomerDataService customerDataService, IStateProviderService stateProviderService, IEventAggregator eventAggregator)
        {
            _stateProviderService = stateProviderService;
            _eventAggregator = eventAggregator;
            _customerDataService = customerDataService;
            _logger?.Info("States retrieved from data base are :", State);
            _logger = LogManager.GetLog(typeof(CustomerDataService));
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the time when the customer data was updated.
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the data row view that represents the customer.
        /// </summary>
        public virtual DataRowView CustomerRow
        {
            get => _customerRow;
            set
            {
                _customerRow = value;
                NotifyOfPropertyChange(nameof(CustomerRow));
            }
        }

        /// <summary>
        /// Gets or sets the collection of states that the customer can choose from.
        /// </summary>
        public BindableCollection<string> State
        {
            get
            {
                return new BindableCollection<string>(_stateProviderService.States);
            }
        }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public string FirstName
        {
            get => _firstName;
            set
            {
                ValidateName("first name", value, 30);
                _firstName = value;
                NotifyOfPropertyChange(nameof(FirstName));
            }
        }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public string LastName
        {
            get => _lastName;
            set
            {
                ValidateName("last name", value, 30);
                _lastName = value;
                NotifyOfPropertyChange(nameof(LastName));
            }
        }

        /// <summary>
        /// Gets or sets the first line of the address of the customer.
        /// </summary>
        public string Address1
        {
            get => _address1;
            set
            {
                ValidateAddress("address1", value, 40);
                _address1 = value;
                NotifyOfPropertyChange(nameof(Address1));
            }
        }

        /// <summary>
        /// Gets or sets the second line of the address of the customer, if any.
        /// </summary>
        public string Address2
        {
            get => _address2;
            set
            {
                ValidateAddress("address2", value, 40);
                _address2 = value;
                NotifyOfPropertyChange(nameof(Address2));
            }
        }

        /// <summary>
        /// Gets or sets the city of the customer.
        /// </summary>
        public string City
        {
            get => _city;
            set
            {
                ValidateCity("city", value, 50);
                _city = value;
                NotifyOfPropertyChange(nameof(City));
            }
        }

        /// <summary>
        /// Gets or sets the zip code of the customer.
        /// </summary>
        public string Zip
        {
            get => _zip;
            set
            {
                ValidateZip("zip", value, 10);
                _zip = value;
                NotifyOfPropertyChange(nameof(Zip));
            }
        }

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        public string Phone
        {
            get => _phone;
            set
            {
                ValidatePhone("phone", value, 20);
                _phone = value;
                NotifyOfPropertyChange(nameof(Phone));
            }
        }

        /// <summary>
        /// Gets or sets the age of the customer in years.
        /// </summary>
        public string Age
        {
            get => _age;
            set
            {
                ValidateAge(value);
                _age = value;
                NotifyOfPropertyChange(nameof(Age));
            }
        }

        /// <summary>
        /// Gets or sets the total sales amount of the customer in dollars.
        /// </summary>
        public string Sales
        {
            get => _sales;
            set
            {
                ValidateSales(value);
                _sales = value;
                NotifyOfPropertyChange(nameof(Sales));
            }
        }

        /// <summary>
        /// Gets or sets the selected state of the customer.
        /// </summary>
        public string SelectedState
        {
            get => _selectedState;
            set
            {
                ValidateState("state", value, 2);
                _selectedState = value;
                NotifyOfPropertyChange(nameof(SelectedState));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the update in progress.
        /// </summary>
        public bool IsUpdateProgress
        {
            get => _isUpdateProgress;
            set
            {
                _isUpdateProgress = value;
                NotifyOfPropertyChange(nameof(IsUpdateProgress));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can click the OK button.
        /// </summary>
        public bool CanOk
        {
            get
            {
                return _canOk;
            }
            set
            {
                _canOk = value;
                NotifyOfPropertyChange(nameof(CanOk));
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the customer data in the database and closes the edit form.
        /// </summary>
        public async Task<OkResult> Ok()
        {
            OkResult okResult = OkResult.Passed;
            try
            {
                if (ValidateAllFields())
                {
                    _logger?.Info("Start updating the customer to the database");
                    UpdatedTime = DateTime.Now;
                    IsUpdateProgress = true;

                    var result = await Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var sales = Decimal.Parse(Sales);
                            return _customerDataService.CustomerTableAdapter.UpdateCustomer(
                                FirstName,
                                LastName,
                                Address1,
                                Address2,
                                City,
                                SelectedState,
                                Zip,
                                Phone,
                                Convert.ToInt32(Age),
                                sales,
                                UpdatedTime,
                                CustomerId
                            );
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                            _eventAggregator.PublishOnUIThreadAsync(new ErrorMessage { Type = "Database Error", Message = ex.Message });
                        }

                        return -1;
                    });

                    if (result == 1)
                    {
                        _logger?.Info("Completed updating the customer details to the database");
                        _logger?.Info("Start updating the customer on the UI");
                        UpdateCustomer();
                        _logger?.Info("Complete updating the customer on the UI");

                        _logger?.Info("Start closing the CustomerEditView");
                        await TryCloseAsync();
                        _logger?.Info("Stop closing the CustomerEditView");
                        okResult = OkResult.Passed;
                    }
                    else
                    {
                        okResult = OkResult.Failed;
                        _logger?.Warn("Failed to updating the customer details to the database");
                    }

                    IsUpdateProgress = false;
                } 
                else
                {
                    okResult = OkResult.ValidationFailed;
                }
            }
            catch (Exception ex)
            {
                okResult = OkResult.Failed;
                _logger?.Error(ex);
            }

            return okResult;
        }

        /// <summary>
        /// Fills the customer edit form with the data from the customer row.
        /// </summary>
        public bool FillCustomer()
        {
            bool result = true;
            try
            {
                if (_customerRow != null)
                {
                    string state = (string)_customerRow[nameof(State)];
                    bool isAgeNull = _customerRow[nameof(Age)] == DBNull.Value;
                    bool isSaleNull = _customerRow[nameof(Sales)] == DBNull.Value;
                    CustomerId = (int)_customerRow[nameof(CustomerId)];
                    _firstName = (string)_customerRow[nameof(FirstName)];
                    _lastName = (string)_customerRow[nameof(LastName)];
                    _address1 = (string)_customerRow[nameof(Address1)];
                    _address2 = Convert.ToString(_customerRow[nameof(Address2)]);
                    _city = (string)_customerRow[nameof(City)];
                    _selectedState = _stateProviderService.States.Contains(state) ? state : "";
                    _zip = (string)_customerRow[nameof(Zip)];
                    _phone = Convert.ToString(_customerRow[nameof(Phone)]);
                    _age = isAgeNull ? null : ((int?)_customerRow[nameof(Age)]).ToString();
                    _sales = isSaleNull ? null : _customerRow[nameof(Sales)].ToString();
                    CanOk = false;
                }
            }
            catch (Exception ex)
            {
                _logger?.Error(ex);
                result = false;
            }

            return result;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the customer row with the edited data.
        /// </summary>
        private void UpdateCustomer()
        {
            try
            {
                _customerRow[nameof(FirstName)] = FirstName;
                _customerRow[nameof(LastName)] = LastName;
                _customerRow[nameof(Address1)] = Address1;
                _customerRow[nameof(Address2)] = Address2;
                _customerRow[nameof(City)] = City;
                _customerRow[nameof(State)] = SelectedState;
                _customerRow[nameof(Zip)] = Zip;
                _customerRow[nameof(Phone)] = Phone;
                _customerRow[nameof(Age)] = Age;
                _customerRow[nameof(Sales)] = Convert.ToDecimal(Sales);
                _customerRow[nameof(UpdatedTime)] = UpdatedTime;
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
            _logger?.Info("Disposed the CustomerEditViewModel");
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
                    _customerRow = null;
                }

                // Set the flag to true
                disposed = true;
            }
        }
        #endregion

        #region Validation Methods
        /// <summary>
        /// Validates all of the fields in the form.
        /// </summary>
        /// <returns>True if all of the fields are valid, false otherwise.</returns>
        private bool ValidateAllFields()
        {
            bool validationResult = true;
            try
            {
                // Log that field validation has started.
                _logger?.Info("Field validation started");

                // Validate each field.
                ValidateName("first name", _firstName, 30);
                ValidateName("last name", _lastName, 30);
                ValidateAddress("address1", _address1, 40);
                ValidateAddress("address2", _address2, 40);
                ValidateCity("city", _city, 50);
                ValidateState("state", _selectedState, 2);
                ValidateZip("zip", _zip, 10);
                ValidatePhone("phone", _phone, 20);
                ValidateAge(_age);
                ValidateSales(_sales);

                // Log that field validation has completed.
                _logger?.Info("Field validation completed");
            }
            catch (ValidationException ex)
            {
                // Publish an error message to the UI thread.
                _eventAggregator.PublishOnUIThreadAsync(new ErrorMessage { Message = ex.Message, Type = "Validation Error" });
                validationResult = false;
            }
            catch (Exception ex)
            {
                // Log the error.
                _logger?.Error(ex);
                validationResult = false;
            }

            // Return the validation result.
            return validationResult;
        }


        /// <summary>
        /// Validates the given text based on the specified criteria.
        /// </summary>
        /// <param name="fieldName">The name of the field being validated.</param>
        /// <param name="textToBeValidated">The text to be validated.</param>
        /// <param name="textLength">The maximum allowed length for the text.</param>        
        private void ValidateText(string fieldName, string textToBeValidated, int textLength)
        {
            if (textToBeValidated == null || string.IsNullOrWhiteSpace(textToBeValidated))
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter your {0}.", fieldName));
            }
            else if (textToBeValidated.Length > textLength)
            {
                CanOk = false;
                throw new ValidationException(string.Format("{0} cannot be longer than {1} characters.", fieldName, textLength));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="fieldName">The name of the name field.</param>
        /// <param name="nameToBeValidated">The name to be validated.</param>
        /// <param name="textLength">The maximum length of the name.</param>
        private void ValidateName(string fieldName, string nameToBeValidated, int textLength)
        {
            // Validate the text field.
            ValidateText(fieldName, nameToBeValidated, textLength);

            // Validate that the name starts with an uppercase letter and only contains
            // alphanumeric characters.
            if (!Regex.Match(nameToBeValidated, RegExConstants.NameRegEx).Success)
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter a valid {0}.", fieldName));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the address.
        /// </summary>
        /// <param name="fieldName">The name of the address field.</param>
        /// <param name="addressToBeValidated">The address to be validated.</param>
        /// <param name="textLength">The maximum length of the address.</param>
        private void ValidateAddress(string fieldName, string addressToBeValidated, int textLength)
        {
            // Validate the text field.
            ValidateText(fieldName, addressToBeValidated, textLength);

            // Validate that the address only contains alphanumeric characters and spaces.
            if (!Regex.Match(addressToBeValidated, RegExConstants.AddressRegEx).Success)
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter a valid {0}.", fieldName));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the city.
        /// </summary>
        /// <param name="fieldName">The name of the city field.</param>
        /// <param name="cityToBeValidated">The city to be validated.</param>
        /// <param name="textLength">The maximum length of the city.</param>
        private void ValidateCity(string fieldName, string cityToBeValidated, int textLength)
        {
            // Validate the text field.
            ValidateText(fieldName, cityToBeValidated, textLength);

            // Validate that the city only contains alphanumeric characters and spaces,
            // and can optionally contain hyphens.
            if (!Regex.Match(cityToBeValidated, RegExConstants.CityRegEx).Success)
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter a valid {0}.", fieldName));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the state.
        /// </summary>
        /// <param name="fieldName">The name of the state field.</param>
        /// <param name="stateToBeValidated">The state to be validated.</param>
        /// <param name="textLength">The maximum length of the state.</param>
        private void ValidateState(string fieldName, string stateToBeValidated, int textLength)
        {
            // Validate that the state is not null or empty.
            if (stateToBeValidated == null || string.IsNullOrWhiteSpace(stateToBeValidated))
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please select your {0}.", fieldName));
            }

            // Validate that the state is not longer than the specified length.
            if (stateToBeValidated.Length > textLength)
            {
                CanOk = false;
                throw new ValidationException(string.Format("{0} cannot be longer than {1} characters.", fieldName, textLength));
            }

            // Validate that the state is a valid state name.
            if (!_stateProviderService.States.Contains(stateToBeValidated))
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter a valid {0}.", fieldName));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the zip code.
        /// </summary>
        /// <param name="fieldName">The name of the zip code field.</param>
        /// <param name="zipToBeValidated">The zip code to be validated.</param>
        /// <param name="textLength">The maximum length of the zip code.</param>
        private void ValidateZip(string fieldName, string zipToBeValidated, int textLength)
        {
            // Validate the text field.
            ValidateText(fieldName, zipToBeValidated, textLength);

            // Validate that the zip code is a valid US zip code.
            // The zip code can be either 5 digits or 5 digits followed by a hyphen and 4 digits.
            if (!Regex.Match(zipToBeValidated, RegExConstants.ZipRegEx).Success)
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter a valid {0}.", fieldName));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the phone number.
        /// </summary>
        /// <param name="fieldName">The name of the phone number field.</param>
        /// <param name="phoneToBeValidated">The phone number to be validated.</param>
        /// <param name="textLength">The maximum length of the phone number.</param>
        private void ValidatePhone(string fieldName, string phoneToBeValidated, int textLength)
        {
            // Validate the text field.
            ValidateText(fieldName, phoneToBeValidated, textLength);

            // Validate that the phone number is a valid US phone number.
            // The phone number must be 9 digits long.
            if (!Regex.Match(phoneToBeValidated, RegExConstants.PhoneRexEx).Success)
            {
                CanOk = false;
                throw new ValidationException(string.Format("Please enter a valid {0}.", fieldName));
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the given age based on the specified criteria.
        /// </summary>
        /// <param name="age">The age to be validated.</param>
        private void ValidateAge(string age)
        {
            if (age == null || string.IsNullOrEmpty(age))
            {
                CanOk = false;
                throw new ValidationException("Please enter an age from 1 to 99 years.");
            }
            int ageValue = Convert.ToInt32(age);
            if (ageValue < 1 || ageValue > 100)
            {
                CanOk = false;
                throw new ValidationException("Please enter an age from 1 to 99 years.");
            }

            CanOk = true;
        }

        /// <summary>
        /// Validates the given sales value based on the specified criteria.
        /// </summary>
        /// <param name="pSales">The sales value to be validated.</param>
        private void ValidateSales(string pSales)
        {
            ValidateText("sales", pSales, 38);

            decimal? sales;
            try
            {
                sales = Decimal.Parse(pSales, System.Globalization.NumberStyles.Float);
            }
            catch (Exception)
            {
                CanOk = false;
                throw new ValidationException("Invalid decimal value provided for sales.");
            }

            if (sales == null || sales > 9999999999999999.99m)
            {
                CanOk = false;
                throw new ValidationException("Invalid sales value.");
            }
            CanOk = true;
        }
        #endregion
    }
}
