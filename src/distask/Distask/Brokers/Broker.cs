using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Distask.Contracts;
using Grpc.Core;
using static Distask.Contracts.DistaskService;

namespace Distask.Brokers
{
    public class Broker : DistaskServiceBase, IBroker
    {
        private readonly List<BrokerTask> tasks = new List<BrokerTask>();

        public Broker(string name, string group, IEnumerable<BrokerTask> tasks)
        {
            this.Name = name;
            this.Group = group;
            if (tasks != null)
            {
                this.tasks.AddRange(tasks);
            }
        }

        public Broker(string name, string group)
            : this(name, group, null)
        { }

        public Broker(string name)
            : this(name, "default")
        { }

        public string Name { get; }

        public string Group { get; }

        public override async Task<DistaskResponse> Execute(DistaskRequest request, ServerCallContext context)
        {
            var task = (from t in this.tasks
                        where string.Equals(t.Name, request.TaskName)
                        select t).FirstOrDefault();
            if (task == null)
            {
                return DistaskResponse.Error($"Task '{request.TaskName}' has not been registered to the broker.");
            }

            try
            {
                return await task.ExecuteAsync(request.Parameters.ToList(), context.CancellationToken);
            }
            catch(Exception ex)
            {
                return DistaskResponse.Exception(ex);
            }
        }

        public override Task<PingResponse> Ping(PingRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PingResponse
            {
                Status = Contracts.StatusCode.Success
            });
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
