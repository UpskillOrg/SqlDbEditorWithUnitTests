using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using Caliburn.Micro;
using SqlDbEditor.Services;

namespace SqlDbEditor.Bootstrapper
{
    /// <summary>
    /// A generic class that inherits from the BootstrapperBase class and provides dependency injection and view model-first navigation for the application.
    /// </summary>
    /// <typeparam name="T">The type of the root view model.</typeparam>
    public class Bootstrapper<T> : BootstrapperBase
    {        
        /// <summary>
        /// A field that holds a reference to the SimpleContainer instance 
        /// </summary>
        protected SimpleContainer _container;

        /// <summary>
        /// Represents a logger configured with the bootstrap
        /// </summary>
        protected ILog _logger = null;

        /// <summary>
        /// Initializes a new instance of the Bootstrapper class and calls the base class Initialize method.
        /// </summary>
        public Bootstrapper()
        {
            Initialize();
            LogManager.GetLog = type => new DebugLog(type);
            _logger = LogManager.GetLog(GetType());
        }

        /// <summary>
        /// Configures the SimpleContainer instance and registers the services and view models.
        /// </summary>
        protected override void Configure()
        {
            try
            {
                // Create a new SimpleContainer instance
                _container = new SimpleContainer();

                // Register the singleton services
                _container.Singleton<IWindowManager, WindowManager>();
                _container.Singleton<IEventAggregator, EventAggregator>();
                _container.PerRequest<T>();
            }
            catch (Exception ex) {
                _logger?.Error(ex);
            }            
        }

        /// <summary>
        /// Gets an instance of the specified service type with the specified key from the container.
        /// </summary>
        /// <param name="service">The type of the service to get.</param>
        /// <param name="key">The key of the service to get.</param>
        /// <returns>An object that represents the service instance.</returns>
        protected override object GetInstance(Type service, string key)
        {
            // Use the container to get the instance
            return _container.GetInstance(service, key);
        }

        /// <summary>
        /// Gets all instances of the specified service type from the container.
        /// </summary>
        /// <param name="service">The type of the service to get.</param>
        /// <returns>An enumerable of objects that represent the service instances.</returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            // Use the container to get all instances
            return _container.GetAllInstances(service);
        }

        /// <summary>
        /// Performs property injection on the specified instance using the container.
        /// </summary>
        /// <param name="instance">The object to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            // Use the container to build up the instance
            _container.BuildUp(instance);
        }

        /// <summary>
        /// Displays the root view for the specified view model type using the window manager.
        /// </summary>
        /// <param name="sender">The source of the event. This parameter is not used.</param>
        /// <param name="e">The data for the event. This parameter is not used.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // Use the base class method to display the root view
            DisplayRootViewFor<T>();
        }

        /// <summary>
        /// Selects the assemblies to inspect for views and view models.
        /// </summary>
        /// <returns>An enumerable of assemblies to inspect.</returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            // Return an array containing only the current assembly
            return new[] { Assembly.GetExecutingAssembly() };
        }
    }
}
