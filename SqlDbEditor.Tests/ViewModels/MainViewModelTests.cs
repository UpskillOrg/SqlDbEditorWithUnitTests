using Caliburn.Micro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlDbEditor.Services;
using SqlDbEditor.ViewModels;
using SqlDbEditor.ViewModels.Controls;

namespace SqlDbEditor.Tests.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {
        /// <summary>
        /// Tests whether the Dispose method of MainViewModel calls the Dispose method on the CustomerViewModel.
        /// </summary>
        [TestMethod]
        public void MainViewModel_Dispose_CallsDisposeOnCustomerViewModel()
        {
            // Arrange
            var mockCustomerDataService = new Mock<ICustomerDataService>();
            var mockDispatcherService = new Mock<IDispatcherService>();
            var mockWindowManager = new Mock<IWindowManager>();
            var mockEventAggregatorService = new Mock<IEventAggregatorService>();
            var mockStateProviderService = new Mock<IStateProviderService>();

            // Create instances of ViewModel objects and set up dependencies
            var customerEditViewModel = new CustomerEditViewModel(
                mockCustomerDataService.Object,
                mockStateProviderService.Object,
                mockEventAggregatorService.Object
            );

            var customerViewModel = new CustomerViewModel(
                mockCustomerDataService.Object,
                mockDispatcherService.Object,
                mockWindowManager.Object,
                customerEditViewModel,
                mockEventAggregatorService.Object
            );

            // Create an instance of the MainViewModel with the previously created ViewModel objects
            var mainViewModelMock = new Mock<MainViewModel>(customerViewModel);

            // Act
            mainViewModelMock.Object.Dispose();

            // Assert            
            // Verify that the Dispose method on the MainViewModel was called once
            mainViewModelMock.Verify(x => x.Dispose(), Times.Once);
        }
    }
}
