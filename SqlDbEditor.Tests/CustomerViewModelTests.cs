using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlDbEditor.Messages;
using SqlDbEditor.Services;
using SqlDbEditor.ViewModels.Controls;
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
            var dispatcherServiceMock = new Mock<DispatcherService>();
            var eventAggregatorMock = new Mock<IEventAggregatorService>();

            var viewModel = new CustomerViewModel(
                customerDataServiceMock.Object,
                dispatcherServiceMock.Object,
                null, // We don't need the window manager for this test
                null, // We don't need the customer edit view model for this test
                eventAggregatorMock.Object
            )
            {
                PageIndex = 1,
                PageSize = 10
            };

            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.GetCustomers(1, 10)).Returns(DataTableHelper.CustomerDataTable);

            // Act
            await viewModel.LoadCustomers();

            // Assert
            dispatcherServiceMock.Verify(dispatcher => dispatcher.InvokeAsync(It.IsAny<System.Action>()), Times.AtMost(2));            
            Assert.IsTrue(viewModel.CanLoadCustomers, "CanLoadCustomers should be true");
        }

        [TestMethod]
        public async Task TestLoadCustomersAsyncSqlException()
        {
            // Arrange
            var customerDataServiceMock = new Mock<ICustomerDataService>();
            var dispatcherServiceMock = new Mock<IDispatcherService>();
            var eventAggregatorServiceMock = new Mock<IEventAggregatorService>();

            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.GetCustomers(It.IsAny<int?>(), It.IsAny<int?>())).Returns(DataTableHelper.CustomerDataTable);

            var viewModel = new CustomerViewModel(
                customerDataServiceMock.Object,
                dispatcherServiceMock.Object,
                null, // We don't need the window manager for this test
                null, // We don't need the customer edit view model for this test
                eventAggregatorServiceMock.Object
            );

            customerDataServiceMock.Setup(service => service.CustomerTableAdapter.GetCustomers(It.IsAny<int?>(), It.IsAny<int?>()))
                .Throws(MakeSqlException());

            // Act
            await viewModel.LoadCustomers();

            // Assert
            eventAggregatorServiceMock.Verify(
                aggregator => aggregator.PublishOnUIThreadAsync(It.IsAny<ErrorMessage>()),
                Times.Once
            );
            Assert.IsTrue(viewModel.CanLoadCustomers, "CanLoadCustomers should be true");
        }

        private static SqlException MakeSqlException()
        {
            SqlException exception = null;
            try
            {
                var conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
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
