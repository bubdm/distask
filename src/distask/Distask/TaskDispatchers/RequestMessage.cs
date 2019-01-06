using System;
using System.Collections.Generic;
using System.Text;

namespace Distask.TaskDispatchers
{
    /// <summary>
    /// Represents the message that will be distributed to the brokers for executing.
    /// </summary>
    public class RequestMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessage"/> class.
        /// </summary>
        /// <param name="taskName">Name of the task to be executed.</param>
        public RequestMessage(string taskName)
        {
            this.TaskName = taskName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMessage"/> class.
        /// </summary>
        /// <param name="taskName">Name of the task to be executed.</param>
        /// <param name="parameters">The parameters.</param>
        public RequestMessage(string taskName, IEnumerable<string> parameters)
            : this(taskName)
        {
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets the name of the task to be executed.
        /// </summary>
        /// <value>
        /// The name of the task.
        /// </value>
        public string TaskName { get; }

        /// <summary>
        /// Gets the parameters of the execution.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<string> Parameters { get; } = new List<string>();
    }
}
