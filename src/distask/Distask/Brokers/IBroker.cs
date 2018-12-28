using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.Brokers
{
    /// <summary>
    /// Represents that the implemented classes are brokers that executes the delegated task (command)
    /// and returns the execution results.
    /// </summary>
    public interface IBroker
    {
        string Name { get; }

        string Group { get; }

        void AddTask(BrokerTask task);

        void AddTask<TBrokerTask>()
            where TBrokerTask : BrokerTask, new();
    }
}
