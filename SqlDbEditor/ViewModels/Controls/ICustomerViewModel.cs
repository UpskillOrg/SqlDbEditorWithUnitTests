using SqlDbEditor.DataAccessLayer;
using System.Data;
using System.Threading.Tasks;

namespace SqlDbEditor.ViewModels.Controls
{
    /// <summary>
    /// Represents a view model for managing customer-related operations.
    /// </summary>
    public interface ICustomerViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether customers can be loaded.
        /// </summary>
        bool CanLoadCustomers { get; set; }

        /// <summary>
        /// Gets or sets the customer data table associated with the view model.
        /// </summary>
        LinkTekTest.CustomerDataTable CustomerTable { get; set; }

        /// <summary>
        /// Disposes of any allocated resources and performs cleanup.
        /// </summary>
        void Dispose();

        /// <summary>
        /// Asynchronously initiates the editing of a data row view.
        /// </summary>
        /// <param name="rowView">The data row view to be edited.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task EditItem(DataRowView rowView);

        /// <summary>
        /// Asynchronously loads customer data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LoadCustomers();
    }

}