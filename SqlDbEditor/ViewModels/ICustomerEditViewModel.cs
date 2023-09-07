using Caliburn.Micro;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SqlDbEditor.ViewModels
{
    /// <summary>
    /// Represents a view model for editing customer information.
    /// </summary>
    public interface ICustomerEditViewModel : IDisposable
    {
        /// <summary>
        /// Gets or sets the first line of the customer's address.
        /// </summary>
        string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the customer's address.
        /// </summary>
        string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the age of the customer.
        /// </summary>
        string Age { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "Ok" action can be performed.
        /// </summary>
        bool CanOk { get; set; }

        /// <summary>
        /// Gets or sets the city where the customer is located.
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the data row view associated with the customer.
        /// </summary>
        DataRowView CustomerRow { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an update operation is in progress.
        /// </summary>
        bool IsUpdateProgress { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// Gets or sets the sales information related to the customer.
        /// </summary>
        string Sales { get; set; }

        /// <summary>
        /// Gets or sets the selected state for the customer.
        /// </summary>
        string SelectedState { get; set; }

        /// <summary>
        /// Gets the list of states available for selection.
        /// </summary>
        BindableCollection<string> State { get; }

        /// <summary>
        /// Gets or sets the date and time when the customer information was last updated.
        /// </summary>
        DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets the ZIP code associated with the customer's location.
        /// </summary>
        string Zip { get; set; }

        /// <summary>
        /// Asynchronously performs the "Ok" action.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Ok();

        /// <summary>
        /// Fill the customer info in the UI
        /// </summary>
        /// <returns>Returns bool whether the operation is successfully completed or not</returns>
        bool FillCustomer();
    }
}