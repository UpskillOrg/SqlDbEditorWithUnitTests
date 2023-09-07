using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlDbEditor.DataAccessLayer;
using SqlDbEditor.Messages;
using SqlDbEditor.Services;
using SqlDbEditor.ViewModels.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlDbEditor.Tests
{

    [TestClass]
    public class CustomerViewModelTests
    {
        private DataTableHelper DataTableHelper { get; set; }

        /// <summary>
        /// Initializes the test environment by creating a new CustomerDataTable instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            DataTableHelper = new DataTableHelper();
        }

        [TestMethod]
        public async Task TestLoadCustomersAsyncSuccess()
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var dispatcherServiceMock = new Mock<IDispatcherService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();

            var viewModel = new CustomerViewModel(
                customerDataServiceMock.Object,
                dispatcherServiceMock.Object,
                null, // We don't need the window manager for this test
                null, // We don't need the customer edit view model for this test
                eventAggregatorMock.Object
            );

            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.GetCustomers()).Returns(DataTableHelper.CustomerDataTable);

            // Act
            await viewModel.LoadCustomers();

            // Assert
            dispatcherServiceMock.Verify(dispatcher => dispatcher.Invoke(It.IsAny<System.Action>()), Times.Once);
            Assert.AreEqual(DataTableHelper.CustomerDataTable, viewModel.CustomerTable, "Expected data table is not equal with actual data table");
            Assert.IsTrue(viewModel.CanLoadCustomers);
        }

        [TestMethod]
        public async Task TestLoadCustomersAsyncSqlException()
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var dispatcherServiceMock = new Mock<IDispatcherService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();

            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.GetCustomers()).Returns(DataTableHelper.CustomerDataTable);

            var viewModel = new CustomerViewModel(
                customerDataServiceMock.Object,
                dispatcherServiceMock.Object,
                null, // We don't need the window manager for this test
                null, // We don't need the customer edit view model for this test
                eventAggregatorMock.Object
            );

            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.GetCustomers())
                .Throws(MakeSqlException());

            // Act
            await viewModel.LoadCustomers();

            // Assert
            eventAggregatorMock.Verify(
                aggregator => aggregator.PublishOnUIThreadAsync(It.IsAny<ErrorMessage>()),
                Times.Once
            );
            Assert.IsTrue(viewModel.CanLoadCustomers);
        }

        private SqlException MakeSqlException()
        {
            SqlException exception = null;
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
                conn.Open();
            }
            catch (SqlException ex)
            {
                exception = ex;
            }
            return (exception);
        }
    }
}
