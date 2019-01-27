using System;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Distask.TaskDispatchers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Distask.TaskDispatchers.Config;
using Distask.TaskDispatchers.Routing;
using Distask.TaskDispatchers.AvailabilityCheckers;
using Microsoft.Extensions.Logging;
using Distask.TaskDispatchers.Client;
using Distask.Tests.Integration.Master.Config;
using System.Reflection;
using Newtonsoft.Json;

namespace Distask.Tests.Integration.Master
{
    public sealed class IntegrationTestRunner : ServiceRunner<IntegrationTestHost>
    {
        #region Public Constructors

        public IntegrationTestRunner()
        {
        }

        #endregion Public Constructors

        #region Protected Properties

        protected override string EnvironmentVariablePrefix => "DISTASK_INTTEST_";

        #endregion Protected Properties

        #region Protected Methods

        protected override void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
        {
            base.ConfigureLogging(context, logging);
        }

        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            
            base.ConfigureServices(context, services);

            var config = context.Configuration;
            var taskDispatcherConfig = config.GetSection("TaskDispatcher")?.Get<TaskDispatcherConfiguration>() ?? TaskDispatcherConfiguration.Default;
            var integrationTestHostConfig = config.GetSection("IntegrationTestHost").Get<IntegrationTestHostConfig>();
            

            services.AddSingleton(taskDispatcherConfig)
                .AddSingleton(serviceProvider => 
                    InitializeObject<IRouter>(integrationTestHostConfig.Router, serviceProvider) ?? 
                    new RandomizedRouter(serviceProvider.GetService<ILogger<RandomizedRouter>>()))

                .AddSingleton(serviceProvider => 
                    InitializeObject<IAvailabilityChecker>(integrationTestHostConfig.AvailabilityChecker, serviceProvider) ??
                    new HealthLevelChecker(serviceProvider.GetService<ILogger<HealthLevelChecker>>(), BrokerClientHealthLevel.Excellent))

                .AddSingleton<ITaskDispatcher, TaskDispatcher>();
        }
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">The configuration.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Invalid instance type.
        /// or
        /// No suitable constructor found on type '{instanceType}
        /// </exception>
        private static T InitializeObject<T>(ObjectInitializerConfig config, IServiceProvider resolver)
            where T : class
        {
            bool ParameterEquals(ParameterInfo[] parameters, List<ObjectInitializerParameterConfig> parameterConfig, out List<object> activatedValues)
            {
                if (parameters.Length != parameterConfig.Count)
                {
                    activatedValues = null;
                    return false;
                }

                activatedValues = new List<object>();
                for (var idx = 0; idx < parameters.Length; idx++)
                {
                    var configuredParameterName = config.Parameters[idx].Name;
                    var configuredParameterValueString = config.Parameters[idx].Value;
                    if (parameters[idx].Name == configuredParameterName)
                    {
                        if (configuredParameterValueString == "$ref")
                        {
                            activatedValues.Add(resolver.GetService(parameters[idx].ParameterType));
                        }
                        else
                        {
                            activatedValues.Add(JsonConvert.DeserializeObject(configuredParameterValueString, parameters[idx].ParameterType));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            var instanceType = Type.GetType(config.Type);
            if (instanceType == null || !typeof(T).IsAssignableFrom(instanceType))
            {
                throw new InvalidOperationException("Invalid instance type.");
            }

            foreach(var ctor in instanceType.GetConstructors())
            {
                if (ParameterEquals(ctor.GetParameters(), config.Parameters, out var activatedValues))
                {
                    return (T)Activator.CreateInstance(instanceType, activatedValues.ToArray());
                }
            }

            return null;
        }

        #endregion Private Methods
    }
}
