using MQTTnet;                    // for MqttFactory
using MQTTnet.Client;             // for IMqttClient
using MQTTnet.Client.Options;     // for MqttClientOptionsBuilder
using MQTTnet.Protocol;

using Microsoft.Extensions.Hosting;       // for BackgroundService
using Microsoft.Extensions.DependencyInjection; // for CreateScope, GetRequiredService
using Microsoft.Extensions.Configuration;  // for IConfiguration
using Microsoft.AspNetCore.SignalR;       // for SignalR
using AlienCyborgSynergyNetwork;        // for IHubContext<NeuralHub>

namespace Hubs
{
    public class MqttSubscriberService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly IHubContext<NeuralHub> _hubContext;
        private readonly string _mqttHost;

        public MqttSubscriberService(IServiceProvider sp, IHubContext<NeuralHub> hub, IConfiguration cfg)
        {
            _sp = sp;
            _hubContext = hub;
            _mqttHost = cfg["MqttHost"] ?? "localhost";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var client = new MqttFactory()
                .CreateMqttClient();
            var opts = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.1.16", 1883)
                .Build();

            await client.ConnectAsync(opts, stoppingToken);

            await client.SubscribeAsync("esp8266/#", MqttQualityOfServiceLevel.AtMostOnce);
            client.UseApplicationMessageReceivedHandler(e => HandleMessage(e));

            await Task.Delay(Timeout.Infinite, stoppingToken);

        }

        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = e.ApplicationMessage.Payload == null
                                ? string.Empty
                : System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var session = new CyborgSession
            {
                Id = Guid.NewGuid(),
                UnitId = topic,
                Type = topic.Contains("neuralstream")
                    ? SessionType.NeuralStream
                    : SessionType.BioLog,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5), // Example end time, adjust as needed
                Metadata = payload
            };

            using var scope = _sp.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await uow.Sessions.AddAsync(session);
            await uow.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveSession", session);
        }
    }

}
