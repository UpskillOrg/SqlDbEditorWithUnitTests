using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlDbEditor.Services;
using SqlDbEditor.ViewModels;
using System;

namespace SqlDbEditor.Tests
{
    /// <summary>
    /// This class contains unit tests for the CustomerEditViewModel class.
    /// </summary>
    [TestClass]
    public class CustomerEditViewModelTests
    {
        /// <summary>
        /// Gets or sets a Data Table Helper
        /// </summary>
        private DataTableHelper DataTableHelper { get; set; }

        /// <summary>
        /// Initializes the test environment by creating a new DataTableHelper instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {            
            DataTableHelper = new DataTableHelper();
        }

        /// <summary>
        /// Tests the FillCustomer method with proper data.
        /// </summary>
        [TestMethod]
        public void TestFillCustomerWithProperData()
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var stateProviderServiceMock = new Mock<IStateProviderService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            stateProviderServiceMock.Setup(provider => provider.States).Returns(new string[] { "NY" });
            var viewModel = new CustomerEditViewModel(customerDataServiceMock.Object, stateProviderServiceMock.Object, eventAggregatorMock.Object)
            {
                CustomerRow = DataTableHelper.DataRowView
            };

            //Act
            bool result = viewModel.FillCustomer();

            // Asserts
            Assert.IsTrue(result, "FillCustomer method expected to be return true");
            Assert.AreEqual(viewModel.FirstName, (string) DataTableHelper.DataRowView[CustomerColumnNames.FirstName], "Expected FirstName is not equal to the actual FirstName");
            Assert.AreEqual(viewModel.LastName, (string)DataTableHelper.DataRowView[CustomerColumnNames.LastName], "Expected Last Name is not equal to the actual Last Name");
            Assert.AreEqual(viewModel.Address1, (string)DataTableHelper.DataRowView[CustomerColumnNames.Address1], "Expected Address1 is not equal to the actual Address1");
            Assert.AreEqual(viewModel.Address2, (string)DataTableHelper.DataRowView[CustomerColumnNames.Address2], "Expected Address2 is not equal to the actual Address2");
            Assert.AreEqual(viewModel.City, (string)DataTableHelper.DataRowView[CustomerColumnNames.City], "Expected City is not equal to the actual City");
            Assert.AreEqual(viewModel.SelectedState, (string)DataTableHelper.DataRowView[CustomerColumnNames.State], "Expected Selected State is not equal to the actual State");
            Assert.AreEqual(viewModel.Zip, (string)DataTableHelper.DataRowView[CustomerColumnNames.Zip], "Expected Zip is not equal to the actual Zip");
            Assert.AreEqual(viewModel.Phone, (string)DataTableHelper.DataRowView[CustomerColumnNames.Phone], "Expected Phone is not equal to the actual Phone");
            Assert.AreEqual(viewModel.Age, DataTableHelper.DataRowView[CustomerColumnNames.Age].ToString(), "Expected Age is not equal to the actual Age");
            Assert.AreEqual(viewModel.Sales, DataTableHelper.DataRowView[CustomerColumnNames.Sales].ToString(), "Expected Sales is not equal to the actual Sales");
        }

        /// <summary>
        /// Tests the FillCustomer method with no proper data.
        /// </summary>
        [TestMethod]
        public void TestFillCustomerWithNoProperData()
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var stateProviderServiceMock = new Mock<IStateProviderService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            stateProviderServiceMock.Setup(provider => provider.States).Returns(new string[] { "NY" });
            var viewModel = new CustomerEditViewModel(customerDataServiceMock.Object, stateProviderServiceMock.Object, eventAggregatorMock.Object)
            {
                CustomerRow = DataTableHelper.CustomerDataTable.DefaultView.AddNew()
            };

            //Act
            bool result = viewModel.FillCustomer();

            // Asserts
            Assert.IsFalse(result, "FillCustomer method expected to be return false");
        }

        /// <summary>
        /// Tests the Ok method with valid data, which should update the customer and close the view.
        /// </summary>
        [TestMethod]
        public void TestOkWithValidDataShouldUpdateCustomerAndCloseView()
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var stateProviderServiceMock = new Mock<IStateProviderService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            stateProviderServiceMock.Setup(provider => provider.States).Returns(new string[] { "NY" });
            var viewModel = new CustomerEditViewModel(customerDataServiceMock.Object, stateProviderServiceMock.Object, eventAggregatorMock.Object);

            // Set up customer row and other properties
            viewModel.CustomerRow = DataTableHelper.CustomerDataTable.DefaultView.AddNew();
            viewModel.FirstName = "John";
            viewModel.LastName = "Doe";
            viewModel.Address1 = "123 Main St";
            viewModel.Address2 = "Redmond";
            viewModel.City = "New York";
            viewModel.SelectedState = "NY";
            viewModel.Zip = "10001";
            viewModel.Phone = "776077099";
            viewModel.Age = "30";
            viewModel.Sales = "1000.50";

            // Mock the UpdateCustomer method of the data service
            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.UpdateCustomer(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.Address1,
                viewModel.Address2,
                viewModel.City,
                viewModel.SelectedState,
                viewModel.Zip,
                viewModel.Phone,
                It.IsAny<int>(),
                It.IsAny<decimal>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>()
            )).Returns(1); // Assuming update was successful

            // Act
            viewModel.Ok().Wait(); // Use Wait to handle async method

            // Assert
            customerDataServiceMock.Verify(service => service.CustomerTableAdapter.UpdateCustomer(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.Address1,
                viewModel.Address2,
                viewModel.City,
                viewModel.SelectedState,
                viewModel.Zip,
                viewModel.Phone,
                It.IsAny<int>(),
                It.IsAny<decimal>(),
                It.IsAny<DateTime>(),
                It.IsAny<int>()
            ), Times.Once);

            Assert.IsTrue(viewModel.CanOk, "CanOk expected to be true");
            Assert.IsFalse(viewModel.IsUpdateProgress, "IsUpdateProgress expected to be false");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.FirstName], viewModel.FirstName, "Expected FirstName is not equal to the actual FirstName");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.LastName], viewModel.LastName, "Expected Last Name is not equal to the actual Last Name");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.Address1], viewModel.Address1, "Expected Address1 is not equal to the actual Address1");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.Address2], viewModel.Address2, "Expected Address2 is not equal to the actual Address2");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.City], viewModel.City, "Expected City is not equal to the actual City");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.State], viewModel.SelectedState, "Expected Selected State is not equal to the actual State");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.Zip], viewModel.Zip, "Expected Zip is not equal to the actual Zip");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.Phone], viewModel.Phone, "Expected Phone is not equal to the actual Phone");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.Age].ToString(), viewModel.Age, "Expected Age is not equal to the actual Age");
            Assert.AreEqual(viewModel.CustomerRow[CustomerColumnNames.Sales].ToString(), viewModel.Sales, "Expected Sales is not equal to the actual Sales");
        }

        /// <summary>
        /// Tests various validations for the input data.
        /// </summary>
        /// <param name="firstName">The first name input.</param>
        /// <param name="lastName">The last name input.</param>
        /// <param name="address1">The address1 input.</param>
        /// <param name="address2">The address2 input.</param>
        /// <param name="city">The city input.</param>
        /// <param name="state">The state input.</param>
        /// <param name="zip">The zip input.</param>
        /// <param name="phone">The phone input.</param>
        /// <param name="age">The age input.</param>
        /// <param name="sales">The sales input.</param>
        [DataTestMethod]
        [DataRow("AbCdEfGhIjKlMnOpQrStUvWxYzAbCdEfG", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "AbCdEfGhIjKlMnOpQrStUvWxYzAbCdEfG", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow(null, "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("123", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("$%^###", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", null, "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "123", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "$%^###", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main && St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt $$ 4B", "New York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New 12 York", "NY", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "XX", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "XXX", "10001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "1000110001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "100011000110001100011000110001", "5551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567555123456755512345675551234567", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "AbCdEfGhIjKlMnOpQrStUvWxYzAbCdEfG", "30", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "100", "1000.50")]
        [DataRow("John", "Doe", "123 Main St", "Apt 4B", "New York", "NY", "10001", "5551234567", "30", "999999999999999999.99")]
        public void TestAllValidations(string firstName, string lastName, string address1, string address2, string city, string state, string zip, string phone, string age, string sales)
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var stateProviderServiceMock = new Mock<IStateProviderService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            stateProviderServiceMock.Setup(provider => provider.States).Returns(new string[] { "NY" });
            var viewModel = new CustomerEditViewModel(customerDataServiceMock.Object, stateProviderServiceMock.Object, eventAggregatorMock.Object);

            // Set up customer row and other properties
            viewModel.CustomerRow = DataTableHelper.CustomerDataTable.DefaultView.AddNew();
            viewModel.CustomerRow[CustomerColumnNames.FirstName] = firstName;
            viewModel.CustomerRow[CustomerColumnNames.LastName] = lastName;
            viewModel.CustomerRow[CustomerColumnNames.Address1] = address1;
            viewModel.CustomerRow[CustomerColumnNames.Address2] = address2;
            viewModel.CustomerRow[CustomerColumnNames.City] = city;
            viewModel.CustomerRow[CustomerColumnNames.State] = state;
            viewModel.CustomerRow[CustomerColumnNames.Zip] = zip;
            viewModel.CustomerRow[CustomerColumnNames.Phone] = phone;
            viewModel.CustomerRow[CustomerColumnNames.Age] = Int32.Parse(age);
            viewModel.CustomerRow[CustomerColumnNames.Sales] = Decimal.Parse(sales);
            viewModel.CustomerRow[CustomerColumnNames.CreatedTime] = DateTime.Now;
            viewModel.CustomerRow[CustomerColumnNames.UpdatedTime] = DateTime.Now;
            bool result = viewModel.FillCustomer();

            // Act
            var okResult = viewModel.Ok().Result; // Use Wait to handle async methods            

            // Assert
            Assert.IsTrue(result, "FillCustomer method expected to be return true");
            Assert.IsFalse(viewModel.CanOk, "CanOk expected to be false");
            Assert.IsFalse(viewModel.IsUpdateProgress, "IsUpdateProgress expected to be false");
            Assert.AreEqual(OkResult.ValidationFailed, okResult, "Validation should be failed");
        }
    }
}
