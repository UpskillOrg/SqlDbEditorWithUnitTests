using Caliburn.Micro;
using SqlDbEditor.Services;
using SqlDbEditor.ViewModels;
using SqlDbEditor.ViewModels.Controls;
using System;

/// <summary>
/// Represents the application bootstrapper for the SQL database editor.
/// </summary>
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
                _container.Singleton<IDispatcherService, DispatcherService>();
                _container.Singleton<IStateProviderService, StateProviderService>();

                // Register the per-request service
                _container.PerRequest<ICustomerDataService, CustomerDataService>();

                // Register the per-request view models
                _container.PerRequest<ICustomerViewModel, CustomerViewModel>();
                _container.PerRequest<ICustomerEditViewModel, CustomerEditViewModel>();                
            }
            catch(Exception ex)
            {
                _logger?.Error(ex);
            }            
        }
    }
}