using System;
using System.Data;
using static SqlDbEditor.DataAccessLayer.LinkTekTest;

namespace SqlDbEditor.Tests.TestUtils
{
    public class DataTableHelper
    {
        private readonly CustomerDataTable _customerDataTable;

        private readonly DataRowView _dataRowView;      
        
        public DataTableHelper()
        {
            _customerDataTable = new CustomerDataTable();

            _dataRowView = _customerDataTable.DefaultView.AddNew();
            _dataRowView[CustomerColumnNames.FirstName] = "John";
            _dataRowView[CustomerColumnNames.LastName] = "Doe";
            _dataRowView[CustomerColumnNames.Address1] = "123 Main St";
            _dataRowView[CustomerColumnNames.Address2] = "Apt 4B";
            _dataRowView[CustomerColumnNames.City] = "New York";
            _dataRowView[CustomerColumnNames.State] = "NY";
            _dataRowView[CustomerColumnNames.Zip] = "10001";
            _dataRowView[CustomerColumnNames.Phone] = "5551234567";
            _dataRowView[CustomerColumnNames.Age] = 30;
            _dataRowView[CustomerColumnNames.Sales] = 1000.50m;
            _dataRowView[CustomerColumnNames.CreatedTime] = DateTime.Now;
            _dataRowView[CustomerColumnNames.UpdatedTime] = DateTime.Now;

            _dataRowView.EndEdit();
        }

        public CustomerDataTable CustomerDataTable => _customerDataTable;

        public DataRowView DataRowView => _dataRowView;
    }
}
