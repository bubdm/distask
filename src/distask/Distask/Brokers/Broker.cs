using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distask.Contracts;
using Grpc.Core;
using static Distask.Contracts.DistaskService;
using Microsoft.Extensions.Logging;

namespace Distask.Brokers
{
    public sealed class Broker : DistaskServiceBase
    {
        private readonly ILogger logger;
        private readonly List<BrokerTask> tasks = new List<BrokerTask>();

        public Broker(IEnumerable<BrokerTask> tasks, ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<Broker>();

            if (tasks != null)
            {
                this.tasks.AddRange(tasks);
            }
        }

        public override async Task<DistaskResponse> Execute(DistaskRequest request, ServerCallContext context)
        {
            var task = (from t in this.tasks
                        where string.Equals(t.Name, request.TaskName)
                        select t).FirstOrDefault();
            if (task == null)
            {
                var errorMessage = $"Task '{request.TaskName}' has not been registered to the broker.";
                this.logger.LogWarning(errorMessage);
                return DistaskResponse.Error(errorMessage);
            }

            try
            {
                return await task.ExecuteAsync(request.Parameters.ToList(), context.CancellationToken);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, $"Error occurred while executing the task '{task.Name}'.");
                return DistaskResponse.Exception(ex);
            }
        }

        public void AddTask(BrokerTask task)
        {
            if (this.tasks.Contains(task))
            {
                throw new InvalidOperationException("The task with the specified name has already been added.");
            }

            this.tasks.Add(task);
        }

        public void AddTask<TBrokerTask>()
            where TBrokerTask : BrokerTask, new()
        {
            this.AddTask(new TBrokerTask());
        }
    }
}
