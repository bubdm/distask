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

using Distask.Contracts;
using Distask.TaskDispatchers.Config;
using Grpc.Core;
using Polly;
using System.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskService;

namespace Distask.TaskDispatchers.Client
{
    public sealed class BrokerClient : IBrokerClient
    {

        #region Private Fields

        private readonly Channel channel;
        private readonly TaskDispatcherConfiguration config;
        private readonly Policy policy;
        private readonly DistaskServiceClient wrappedClient;
        private bool disposed = false;

        #endregion Private Fields

        #region Public Constructors

        public BrokerClient(string name, string host, int port, ChannelCredentials channelCredentials, TaskDispatcherConfiguration config)
        {
            this.Name = name;
            this.Host = host;
            this.Port = port;
            this.channel = new Channel(host, port, channelCredentials);
            this.wrappedClient = new DistaskServiceClient(channel);
            this.config = config;
            if (config.BrokerClientConfiguration.Resiliency.Enabled)
            {
                this.policy = Policy.Handle<Exception>()
                    .CircuitBreakerAsync(
                        config.BrokerClientConfiguration.Resiliency.CircuitBreakOnExceptions,
                        TimeSpan.FromMilliseconds(config.BrokerClientConfiguration.Resiliency.CircuitBreakMilliseconds),
                        this.OnCircuitBroken, this.OnCircuitReset, this.OnCircuitHalfOpen);
            }
            else
            {
                this.policy = Policy.NoOpAsync();
            }
        }

        #endregion Public Constructors

        #region Private Destructors

        ~BrokerClient()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        #endregion Private Destructors

        #region Public Events

        public event EventHandler<BrokerClientCircuitBrokenEventArgs> CircuitBroken;
        public event EventHandler<BrokerClientCircuitHalfOpenEventArgs> CircuitHalfOpen;
        public event EventHandler<BrokerClientCircuitResetEventArgs> CircuitReset;
        public event EventHandler<BrokerClientDisposedEventArgs> Disposed;

        #endregion Public Events

        #region Public Properties

        public string Host { get; }
        public BrokerClientState State { get; } = new BrokerClientState();
        public string Name { get; }
        public int Port { get; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is BrokerClient client &&
                   Host == client.Host &&
                   Port == client.Port &&
                   Name == client.Name;
        }

        public async Task<DistaskResponse> ExecuteAsync(DistaskRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            this.State.IncreaseTotalRequests();
            this.State.LastRequestTime = DateTime.UtcNow;
            try
            {
                return await policy.ExecuteAsync(async ct =>
                {
                    this.State.IncreaseForwardedRequests();
                    var result = await wrappedClient.ExecuteAsync(request, cancellationToken: ct);
                    this.State.LastSuccessRequestTime = DateTime.UtcNow;
                    return result;
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                this.State.LastException = ex;
                this.State.AddException(ex);
                throw;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -1176501893;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Host);
            hashCode = hashCode * -1521134295 + Port.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public override string ToString()
        {
            return $"ClientName: {Name}, ClientHost: {Host}, ClientPort: {Port}, HealthLevel: {this.State.HealthLevel}, TotalRequests: {this.State.TotalRequests}, TotalExceptions: {this.State.TotalExceptions}";
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    channel.ShutdownAsync().Wait();
                    this.State.ClearExceptionLogEntries();
                    this.State.LifetimeState = BrokerClientLifetimeState.Disposed;
                }

                OnDisposed(new BrokerClientDisposedEventArgs(Name, Host, Port));

                disposed = true;
            }
        }

        private void OnCircuitBroken(Exception exception, TimeSpan timeSpan) => CircuitBroken?.Invoke(this, new BrokerClientCircuitBrokenEventArgs(exception, timeSpan));

        private void OnCircuitHalfOpen() => CircuitHalfOpen?.Invoke(this, new BrokerClientCircuitHalfOpenEventArgs());

        private void OnCircuitReset() => CircuitReset?.Invoke(this, new BrokerClientCircuitResetEventArgs());

        private void OnDisposed(BrokerClientDisposedEventArgs e) => Disposed?.Invoke(this, e);

        #endregion Private Methods

    }
}