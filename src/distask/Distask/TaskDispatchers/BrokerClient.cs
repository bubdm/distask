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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskService;

namespace Distask.TaskDispatchers
{
    public sealed class BrokerClient : IBrokerClient
    {

        #region Private Fields

        private readonly Channel channel;
        private readonly TaskDispatcherConfig config;
        private readonly Policy policy;
        private readonly DistaskServiceClient wrappedClient;
        private bool disposedValue = false;

        #endregion Private Fields

        #region Public Constructors

        public BrokerClient(string name, string host, int port, ChannelCredentials channelCredentials, TaskDispatcherConfig config)
        {
            this.Name = name;
            this.Host = host;
            this.Port = port;
            this.channel = new Channel(host, port, channelCredentials);
            this.wrappedClient = new DistaskServiceClient(channel);
            this.config = config;
            this.policy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(
                    config.BrokerClient.Resiliency.CircuitBreakOnExceptions,
                    TimeSpan.FromMilliseconds(config.BrokerClient.Resiliency.CircuitBreakMilliseconds),
                    this.OnCircuitBroken, this.OnCircuitReset, this.OnCircuitHalfOpen);
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

        #endregion Public Events

        #region Public Properties

        public string Name { get; }

        public string Host { get; }

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
            => await policy.ExecuteAsync(async ct =>
            {
                return await wrappedClient.ExecuteAsync(request, cancellationToken: ct);
            }, cancellationToken);

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
            return $"ClientName: {Name}, ClientHost: {Host}, ClientPort: {Port}";
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    channel.ShutdownAsync().Wait();
                }

                disposedValue = true;
            }
        }

        private void OnCircuitBroken(Exception exception, TimeSpan timeSpan) => CircuitBroken?.Invoke(this, new BrokerClientCircuitBrokenEventArgs(exception, timeSpan));

        private void OnCircuitHalfOpen() => CircuitHalfOpen?.Invoke(this, new BrokerClientCircuitHalfOpenEventArgs());

        private void OnCircuitReset() => CircuitReset?.Invoke(this, new BrokerClientCircuitResetEventArgs());

        #endregion Private Methods
    }
}