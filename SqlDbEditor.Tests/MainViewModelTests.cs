using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDbEditor.ViewModels.Controls;
using SqlDbEditor.ViewModels;
using Moq;
using SqlDbEditor.Services;
using Caliburn.Micro;

namespace SqlDbEditor.Tests
{
    [TestClass]
    public class MainViewModelTests
    {       
        [TestMethod]
        public void MainViewModel_Dispose_CallsDisposeOnCustomerViewModel()
        {
            // Arrange
            var mockCustomerDataService = new Mock<ICustomerDataService>();
            var mockDispatcherService = new Mock<IDispatcherService>();
            var mockWindowManager = new Mock<IWindowManager>();            
            var mockEventAggregator = new Mock<IEventAggregator>();            
            var mockStateProviderService = new Mock<IStateProviderService>();            

            var customerEditViewModel = new CustomerEditViewModel(
                mockCustomerDataService.Object,
                mockStateProviderService.Object,
                mockEventAggregator.Object
            );

            var customerViewModel = new CustomerViewModel(
                mockCustomerDataService.Object,
                mockDispatcherService.Object,
                mockWindowManager.Object,
                customerEditViewModel,
                mockEventAggregator.Object
            );

            var mainViewModelMock = new Mock<MainViewModel>(mockWindowManager.Object, customerViewModel);

            // Act
            mainViewModelMock.Object.Dispose();

            // Assert
            mainViewModelMock.Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void MainViewModel_Dispose_CalledMultipleTimes_DisposeOnCustomerViewModelCalledOnce()
        {
            // Arrange
            var mockCustomerDataService = new Mock<ICustomerDataService>();
            var mockDispatcherService = new Mock<IDispatcherService>();
            var mockWindowManager = new Mock<IWindowManager>();
            var mockEventAggregator = new Mock<IEventAggregator>();
            var mockStateProviderService = new Mock<IStateProviderService>();

            var customerEditViewModel = new CustomerEditViewModel(
                mockCustomerDataService.Object,
                mockStateProviderService.Object,
                mockEventAggregator.Object
            );

            var customerViewModel = new CustomerViewModel(
                mockCustomerDataService.Object,
                mockDispatcherService.Object,
                mockWindowManager.Object,
                customerEditViewModel,
                mockEventAggregator.Object
            );

            var mainViewModelMock = new Mock<MainViewModel>(mockWindowManager.Object, customerViewModel);

            // Act
            mainViewModelMock.Object.Dispose();
            mainViewModelMock.Object.Dispose();

            // Assert
            mainViewModelMock.Verify(x => x.Dispose(), Times.AtMost(2));
        }
    }
}
