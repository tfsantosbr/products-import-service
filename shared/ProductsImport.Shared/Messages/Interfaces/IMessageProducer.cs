using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsImport.Shared.Messages.Interfaces
{
    public interface IMessageProducer
    {
        Task<DeliveryResult<Null, string>> ProduceAsync<TMessage>(ProducerConfig config, string topic, TMessage value);
    }
}
