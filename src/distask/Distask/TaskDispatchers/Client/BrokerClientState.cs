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
    public sealed class BrokerClientState
    {

        #region Private Fields

        private readonly ConcurrentBag<ExceptionLogEntry> exceptionLogEntries = new ConcurrentBag<ExceptionLogEntry>();
        private long forwardedRequests = 0L;
        private long lastRequestTimeData;
        private long lastRoutedTimeData;
        private long lastSuccessRequestTimeData;
        private int lifetimeStateData;
        private long totalRequests = 0L;

        #endregion Private Fields

        #region Public Constructors

        public BrokerClientState()
        {
            this.LifetimeState = BrokerClientLifetimeState.Alive;
        }

        #endregion Public Constructors

        #region Public Properties

        public long ForwardedRequests => this.forwardedRequests;

        public BrokerClientHealthLevel HealthLevel
        {
            get
            {
                var score = this.TotalRequests == 0 ?
                    100 :
                    (this.TotalRequests - this.exceptionLogEntries.Count) * 100L / this.TotalRequests;
                var numbersPerBucket = 100 / (Enum.GetNames(typeof(BrokerClientHealthLevel)).Length - 1);
                var bucketNum = score / numbersPerBucket + 1;
                return (BrokerClientHealthLevel)bucketNum;
            }
        }

        public Exception LastException { get; internal set; }

        public DateTime LastRequestTime
        {
            get
            {
                var data = Interlocked.CompareExchange(ref this.lastRequestTimeData, 0, 0);
                return DateTime.FromBinary(data);
            }
            internal set
            {
                Interlocked.Exchange(ref this.lastRequestTimeData, value.ToBinary());
            }
        }

        public DateTime LastRoutedTime
        {
            get
            {
                var data = Interlocked.CompareExchange(ref this.lastRoutedTimeData, 0, 0);
                return DateTime.FromBinary(data);
            }
            internal set
            {
                Interlocked.Exchange(ref this.lastRoutedTimeData, value.ToBinary());
            }
        }

        public DateTime LastSuccessRequestTime
        {
            get
            {
                var data = Interlocked.CompareExchange(ref this.lastSuccessRequestTimeData, 0, 0);
                return DateTime.FromBinary(data);
            }
            internal set
            {
                Interlocked.Exchange(ref this.lastSuccessRequestTimeData, value.ToBinary());
            }
        }

        public BrokerClientLifetimeState LifetimeState
        {
            get
            {
                return (BrokerClientLifetimeState)this.lifetimeStateData;
            }
            set
            {
                var val = Convert.ToInt32(value);
                Interlocked.Exchange(ref this.lifetimeStateData, val);
            }
        }

        public long TotalExceptions => this.exceptionLogEntries.Count;

        public long TotalRequests => this.totalRequests;

        #endregion Public Properties

        #region Public Methods

        public void AddException(Exception exception)
        {
            exceptionLogEntries.Add(new ExceptionLogEntry(exception));
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
            else
            {
                return from entry in this.exceptionLogEntries
                       let extType = entry.Exception.GetType()
                       where (extType == exceptionType || extType.IsSubclassOf(exceptionType)) &&
                       (DateTime.UtcNow - entry.OccurredOn <= period)
                       select entry.Exception;
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void IncreaseForwardedRequests() => Interlocked.Increment(ref this.forwardedRequests);

        internal void IncreaseTotalRequests() => Interlocked.Increment(ref this.totalRequests);

        #endregion Internal Methods

    }
}