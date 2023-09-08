using Caliburn.Micro;
using SqlDbEditor.Services;
using SqlDbEditor.ViewModels;
using SqlDbEditor.ViewModels.Controls;
using System;

namespace SqlDbEditor.Bootstrapper
{
    /// <summary>
    /// Represents the main application bootstrapper.
    /// </summary>
    public class AppBootstrapper : Bootstrapper<MainViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {            
        }

        protected override void Configure()
        {
            try
            {
                base.Configure();
                Container.Singleton<IDispatcherService, DispatcherService>();
                Container.Singleton<IStateProviderService, StateProviderService>();
                Container.Singleton<IEventAggregatorService, EventAggregatorService>();

                // Register the per-request service
                Container.PerRequest<ICustomerDataService, CustomerDataService>();

                // Register the per-request view models
                Container.PerRequest<ICustomerViewModel, CustomerViewModel>();
                Container.PerRequest<ICustomerEditViewModel, CustomerEditViewModel>();                
            }
            catch(Exception ex)
            {
                Logger?.Error(ex);
            }            
        }
    }
}