using SqlDbEditor.DataAccessLayer.LinkTekTestTableAdapters;
using System;

namespace SqlDbEditor.Services
{
    /// <summary>
    /// Represents a service for accessing customer data.
    /// </summary>
    public interface ICustomerDataService : IDisposable
    {
        /// <summary>
        /// Gets or sets the customer table adapter.
        /// </summary>
        CustomerTableAdapter CustomerTableAdapter { get; set; }
    }
}