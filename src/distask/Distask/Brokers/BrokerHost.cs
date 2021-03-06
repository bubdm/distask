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

using Distask.Brokers.Config;
using Distask.Contracts;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Distask.Contracts.DistaskRegistrationService;

namespace Distask.Brokers
{
    public sealed class BrokerHost : ConfiguredServiceHost<BrokerHostConfig>
    {
        #region Private Fields

        private readonly Broker broker;
        private readonly ILogger logger;
        private Server server;

        #endregion Private Fields

        #region Public Constructors

        public BrokerHost(Broker broker, ILogger<BrokerHost> logger, IApplicationLifetime applicationLifetime, IConfiguration configuration)
            : base(applicationLifetime, configuration)
        {
            this.broker = broker;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Protected Properties

        protected override string ConfigurationSectionName => Utils.Constants.BrokerHostConfigurationSectionName;

        #endregion Protected Properties

        #region Public Methods

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug("Configuration Options:{0}{1}", Environment.NewLine, this.options);

            await Task.Factory.StartNew(async () =>
            {
                logger.LogInformation("Starting Broker Host {0} ...", this.options.Name);
                
                logger.LogInformation($"Registering BrokerHost to master {this.options.Master.Host}:{this.options.Master.Port}...");
                var registrationChannel = new Channel(this.options.Master.Host,
                    this.options.Master.Port, ChannelCredentials.Insecure); // TODO: Make the channel credential configurable.
                var registrationClient = new DistaskRegistrationServiceClient(registrationChannel);
                var registered = false;
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        var registrationResponse = await registrationClient.RegisterAsync(new RegistrationRequest
                        {
                            Name = this.options.Name,
                            Host = this.options.Host,
                            Port = this.options.Port,
                            Group = this.options.Group ?? Utils.Constants.DefaultGroupName
                        }, deadline: DateTime.UtcNow.AddSeconds(10).ToUniversalTime(), cancellationToken: cancellationToken);

                        if (registrationResponse.Status == Contracts.StatusCode.Success)
                        {
                            registered = true;
                            break;
                        }
                        else
                        {
                            if (registrationResponse.Reason == RegistrationResponse.Types.RejectReason.AlreadyExists)
                            {
                                logger.LogWarning($"Registration to master {this.options.Master.Host}:{this.options.Master.Port} failed. The name '{this.options.Name}' or the host/port '{this.options.Host}:{this.options.Port}' had already been registered.");
                                break;
                            }
                            else
                            {
                                logger.LogWarning($"Registration to master {this.options.Master.Host}:{this.options.Master.Port} failed. Reject Message: {registrationResponse.RejectMessage}. Retrying...");
                                await Task.Delay(5000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning($"Registration to master {this.options.Master.Host}:{this.options.Master.Port} failed. Error Message: {ex.Message}. Retrying...");
                        await Task.Delay(5000);
                    }
                }

                await registrationChannel.ShutdownAsync();

                if (registered)
                {
                    logger.LogInformation($"BrokerHost has been successfully registered to master {this.options.Master.Host}:{this.options.Master.Port}.");

                    this.server = new Server
                    {
                        Services = { DistaskService.BindService(this.broker) },
                        Ports = { new ServerPort(this.options.Host, this.options.Port, ServerCredentials.Insecure) }
                    };

                    this.server.Start();

                    logger.LogInformation("BrokerHost is ready for accepting distributed tasks.");
                }
            });
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.server != null)
            {
                await this.server.ShutdownAsync();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion Protected Methods
    }
}