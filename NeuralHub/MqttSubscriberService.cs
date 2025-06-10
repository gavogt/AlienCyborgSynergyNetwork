using AlienCyborgSynergyNetwork;
using AlienCyborgSynergyNetwork.Shared;
using Microsoft.AspNetCore.SignalR;       // for SignalR
using MQTTnet;                    // for MqttFactory
using MQTTnet.Client;             // for IMqttClient
using MQTTnet.Client.Options;     // for MqttClientOptionsBuilder
using MQTTnet.Client.Subscribing;
using MQTTnet.Formatter;        // for IHubContext<NeuralHub>
using System.Text;
using System.Text.Json;

namespace Hubs
{
    public class MqttSubscriberService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly IHubContext<NeuralHub> _hubContext;
        private readonly IMqttClientOptions _mqttOptions;

        public MqttSubscriberService(IServiceProvider sp, IHubContext<NeuralHub> hub, IConfiguration cfg)
        {
            _sp = sp;
            _hubContext = hub;
            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.0.204", 1883)
                .WithProtocolVersion(MqttProtocolVersion.V311)
                .Build();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Create and connect to MQTT broker
            var client = new MqttFactory().CreateMqttClient();
            await client.ConnectAsync(_mqttOptions, cancellationToken: stoppingToken);

            var options = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(f => f
                    .WithTopic("esp8266/#")
                    .WithAtMostOnceQoS()
                )
                .Build();

            // Subscribe to topics
            await client.SubscribeAsync(options, stoppingToken);


            client.UseApplicationMessageReceivedHandler(async e =>
            {
                byte[] payloadBytes = e.ApplicationMessage.Payload;

                string json = Encoding.UTF8.GetString(payloadBytes);

                // Deserialize the JSON payload into a CyborgSession object
                var session = JsonSerializer.Deserialize<CyborgSession>(json);
                if (session is null) return;

            });

            client.UseApplicationMessageReceivedHandler(HandleMessage);
            await Task.Delay(Timeout.Infinite, stoppingToken);

        }

        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            // Read Topic and Payload
            var topic = e.ApplicationMessage.Topic;
            var payload = e.ApplicationMessage.Payload == null
                                ? string.Empty
                : System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            // Create a new SensorReading with GUID, topic, payload, and timestamp
            var reading = new SensorReading
            {
                ID = Guid.NewGuid(),
                Topic = e.ApplicationMessage.Topic,
                Payload = payload,
                Timestamp = DateTime.UtcNow,
            };

            // Retrieve ISensorUnitOfWork from the service provider
            using var scope = _sp.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<ISensorUnitOfWork>();

            // Save sensors reading to the database
            await uow.Sensors.AddAsync(reading);
            await uow.SaveChangesAsync();

            // Broadcast to all SignalR clients
            await _hubContext.Clients.All.SendAsync("ReceiveSession", reading);
        }
    }

}
