using Confluent.Kafka;
using ProductsImport.Shared.Messages.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductsImport.Shared.Messages.Service
{
    public class MessageProducer : IMessageProducer
    {
        public async Task<DeliveryResult<Null, string>> ProduceAsync<TMessage>(ProducerConfig config, string topic, TMessage value)
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var importCreatedJson = JsonSerializer.Serialize(value);

            var message = new Message<Null, string>
            {
                Value = importCreatedJson
            };

            return await producer.ProduceAsync(topic, message);
        }
    }
}
