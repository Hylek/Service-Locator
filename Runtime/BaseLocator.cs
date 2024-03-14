using System;
using System.Collections.Generic;
using System.Linq;

namespace DC.ServiceLocator
{
    /// <summary>
    /// Implements the Locator pattern so all service classes can be placed under one roof.
    /// </summary>
    public abstract class BaseLocator
    {
        private static readonly Dictionary<Type, object> Services;
        
        static BaseLocator() => Services = new Dictionary<Type, object>();

        /// <summary>
        /// Add a new service to the locator.
        /// </summary>
        /// <param name="service">The initialised service object to be added.</param>
        /// <param name="serviceUnique">Whether there should only ever be 1 instance of the given service.</param>
        /// <typeparam name="T">The type of service to add.</typeparam>
        public static void AddNewService<T>(object service, bool serviceUnique = true)
        {
            var type = typeof(T);
            
            // Check if this service already exists
            if (serviceUnique)
            {
                if (Services.Any(activeService => activeService.Key == type))
                {
                    // If the service is declared unique and it already exists; then no new service needs to be added.
                    return;
                }
            }
            
            Services.Add(type, service);
        }

        /// <summary>
        /// Removes a service from the locator.
        /// </summary>
        /// <typeparam name="T">The type of service to remove.</typeparam>
        public static void RemoveService<T>() => Services.Remove(typeof(T));

        /// <summary>
        /// Finds and returns a service for use.
        /// </summary>
        /// <typeparam name="T">The type of service to request.</typeparam>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Requested service was not found.</exception>
        public static T Find<T>()
        {
            try
            {
                return (T)Services[typeof(T)];
            }
            catch
            {
                throw new ApplicationException("The requested service could not be found!");
            }
        }

        /// <summary>
        /// Checks if the service exists, useful for checking if a service already exists before adding a new one.
        /// </summary>
        /// <param name="type">The type of service to check for.</param>
        /// <returns>True if the service does exist within the locator, and false if not.</returns>
        public static bool DoesServiceExist(Type type) => Services.Any(s => s.Key == type);

        /// <summary>
        /// Cleans up the locator by removing all services.
        /// </summary>
        public static void CleanUp() => Services.Clear();
    }
}