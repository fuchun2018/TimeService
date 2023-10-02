using Confluent.Kafka;
using MessageServiceProvider.@out;
using Microsoft.Extensions.Configuration;

namespace MessageServiceProvider.Adapter
{
    public class MessageServiceProvider : IMessageServiceProvider
    {
        private readonly IConfiguration _configuration;

        public MessageServiceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendAsync(string message)
        {
            var config = new ProducerConfig { BootstrapServers = _configuration["kafka-server"] };
            var producer = new ProducerBuilder<Null, string>(config).Build();
            await producer.ProduceAsync(_configuration["kafka-topic"], new Message<Null, string> { Value = message });
        }
    }
}