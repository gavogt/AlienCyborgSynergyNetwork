using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AlienCyborgSynergyNetwork.Shared;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Hubs;
using MQTTnet;            // for MqttFactory
using MQTTnet.Client;             // for IMqttClient
using MQTTnet.Client.Options;     // for MqttClientOptionsBuilder

namespace FirmwareDistributionService
{
    public class FirmwareJobProcessor : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private readonly IEnumerable<ICommand> _commands;
        private readonly IHubContext<NeuralHub> _hub;
        private readonly IMqttClient _mqttClient;

        public FirmwareJobProcessor(IConfiguration cfg, IEnumerable<ICommand> commands, IHubContext<NeuralHub> hub)
        {
            _commands = commands;
            _hub = hub;

            _factory = new ConnectionFactory
            {
                HostName = cfg["RABBIT_HOST"] ?? "localhost",
                UserName = "guest",
                Password = "guest"
            };

            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();
            var mqttOpts = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.0.204", 1883) 
                .Build();

            _mqttClient.ConnectAsync(mqttOpts).GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var conn = await _factory.CreateConnectionAsync();
            var channel = await conn.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: "firmware_jobs",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += HandleMessage;

            await channel.BasicConsumeAsync(queue: "firmware_jobs",
                                 autoAck: true,
                                 consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleMessage(object sender, BasicDeliverEventArgs ea)
        {

            var body = ea.Body.ToArray();
            var job = JsonSerializer.Deserialize<FirmwareJob>(body);
            if (job == null) return;

            var progress = new Progress<int>(async pct =>
            {
               await _hub.Clients.All.SendAsync("FirmwareProgress", job.Version, pct);
            });

            try
            {
                foreach (var cmd in _commands)
                {
                    await cmd.ExecuteAsync(job, progress);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing job {job.Version}: {ex.Message}");

            }

            var msg = new MqttApplicationMessageBuilder()
                .WithTopic("esp8266/update")
                .WithPayload(job.Version)
                .WithAtMostOnceQoS()
                .WithRetainFlag()
                .Build();

            await _mqttClient.PublishAsync(msg);
        }
    }
}
