using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienCyborgSynergyNetwork.Shared;
using RabbitMQ.Client;

namespace FirmwareDistributionService
{
    public class JobPublisher
    {
        // RabbitMQ connection and channel
        private IChannel? _channel;
        private IConnection? _connection;

        // RabbitMQ connection factory
        private readonly ConnectionFactory _factory;

        public JobPublisher()
        {
            // Connect to localhost
            _factory = new ConnectionFactory { HostName = "localhost" };

        }

        public async Task InitAsync()
        {
            // Open Connection
            _connection = await _factory.CreateConnectionAsync();

            // Open Channel
            _channel = await _connection.CreateChannelAsync();

            // Declare a durable queue for firmware jobs
            await _channel.QueueDeclareAsync(queue: "firmware_jobs",
                                 durable: true, // Survive server restart
                                 exclusive: false, // More than one consumer can use it
                                 autoDelete: false, // Don't delete it when last consumer leaves
                                 arguments: null); // No fancy options

        }
    }
}
