/****************************************************************************
 *           ___      __             __
 *      ____/ (_)____/ /_____ ______/ /__
 *     / __  / / ___/ __/ __ `/ ___/ //_/
 *    / /_/ / (__  ) /_/ /_/ (__  ) ,<
 *    \__,_/_/____/\__/\__,_/____/_/|_|
 *
 * Copyright (C) 2018-2019 by daxnet, https://github.com/daxnet/distask
 * All rights reserved.
 * Licensed under MIT License.
 * https://github.com/daxnet/distask/blob/master/LICENSE
 ****************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Distask.TaskDispatchers.Client
{
    /// <summary>
    /// Represents the index of the broker client that could be used as a reference
    /// when measuring the availability of the clients.
    /// </summary>
    public sealed class Index
    {
        private long totalRequests = 0L;
        private long forwardedRequests = 0L;

        private readonly ConcurrentBag<ExceptionLogEntry> exceptionLogEntries = new ConcurrentBag<ExceptionLogEntry>();

        public void AddException(Exception exception)
        {
            exceptionLogEntries.Add(new ExceptionLogEntry(exception));
        }

        public long TotalRequests => this.totalRequests;

        public long TotalExceptions => this.exceptionLogEntries.Count;

        public long ForwardedRequests => this.forwardedRequests;

        public Exception LastException { get; internal set; }

        internal void IncreaseTotalRequests()
        {
            Interlocked.Increment(ref this.totalRequests);
        }

        internal void IncreaseForwardedRequests()
        {
            Interlocked.Increment(ref this.forwardedRequests);
        }

        public HealthLevel HealthLevel
        {
            get
            {
                var score = this.TotalRequests == 0 ?
                    100 :
                    (this.TotalRequests - this.exceptionLogEntries.Count) * 100L / this.TotalRequests;
                var numbersPerBucket = 100 / (Enum.GetNames(typeof(HealthLevel)).Length - 1);
                var bucketNum = score / numbersPerBucket + 1;
                return (HealthLevel)bucketNum;
            }
        }

        public IEnumerable<TException> GetExceptions<TException>(TimeSpan? period = null)
            where TException : Exception => GetExceptions(typeof(TException), period).Select(item => item as TException);

        public IEnumerable<Exception> GetExceptions(Type exceptionType, TimeSpan? period = null)
        {
            if (period == null)
            {
                return from entry in this.exceptionLogEntries
                       let extType = entry.Exception.GetType()
                       where extType == exceptionType || extType.IsSubclassOf(exceptionType)
                       select entry.Exception;
            }

            return from entry in this.exceptionLogEntries
                   let extType = entry.Exception.GetType()
                   where extType == exceptionType || exceptionType.IsSubclassOf(exceptionType) &&
                   (DateTime.UtcNow - entry.OccurredOn <= period)
                   select entry.Exception;
        }
    }
}