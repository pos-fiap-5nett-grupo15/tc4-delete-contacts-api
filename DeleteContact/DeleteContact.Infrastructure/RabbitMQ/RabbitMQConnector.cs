﻿using DeleteContact.Infrastructure.Settings;
using RabbitMQ.Client;

namespace DeleteContact.Infrastructure.RabbitMQ
{
    public class RabbitMQConnector
    {
        public readonly IRabbitMQConsumerSettings RabbitMQSettings;

        public RabbitMQConnector(IRabbitMQConsumerSettings? rabbitMQSettings) =>
            RabbitMQSettings = rabbitMQSettings ?? throw new ArgumentNullException(nameof(rabbitMQSettings));

        public Task<IConnection> GetConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = RabbitMQSettings.Host,
                Port = RabbitMQSettings.Port,
                UserName = RabbitMQSettings.Username,
                Password = RabbitMQSettings.Password
            };
            return Task.Run(async () => await factory.CreateConnectionAsync());
        }
    }
}
