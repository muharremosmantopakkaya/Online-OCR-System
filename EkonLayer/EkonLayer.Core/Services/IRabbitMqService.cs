using EkonLayer.Helpers.Models.CustomModels;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkonLayer.Core.Services
{
    public interface IRabbitMqService
    {
        ApplicationStartResult Initialize(string url, string username, string password);
        bool ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete);
        bool QueueBind(string exchange, string routingKey, Action<string, byte[]> onReceive);
        bool QueueDelete(string exchange, string routingKey);
        bool Publish(string exchange, string routingKey, object data, IBasicProperties props = null);
        bool Publish(string exchange, string routingKey, byte[] data, IBasicProperties props = null);
    }
}
