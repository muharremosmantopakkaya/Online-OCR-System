using EkonLayer.Core.Services;
using EkonLayer.Helpers.CommonMethods;
using EkonLayer.Helpers.Models.CustomModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace EkonLayer.Service.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        public string _url;
        public string _username;
        public string _password;

        public IModel channel;
        public IConnection connection;
        public EventingBasicConsumer consumer;
        public List<KeyValuePair<string, string>> bindings = new List<KeyValuePair<string, string>>();

        public bool rabbitmqcontrolprogress = false;
        public System.Timers.Timer rabbitmqcontrol = new System.Timers.Timer();

        public void RabbitMqConnectionCheck(object sources, ElapsedEventArgs e)
        {
            if (!rabbitmqcontrolprogress)
            {
                rabbitmqcontrolprogress = true;

                try
                {
                    try
                    {
                        channel.Dispose();
                        connection.Dispose();
                        channel.Close();
                        connection.Close();
                        consumer = null;
                        channel = null;
                        connection = null;
                    }
                    catch (Exception ex)
                    {
                    }

                    var factory = new ConnectionFactory() { HostName = _url, UserName = _username, Password = _password, RequestedHeartbeat = TimeSpan.FromSeconds(30) };
                    connection = factory.CreateConnection();
                    channel = connection.CreateModel();

                }
                catch (Exception ex)
                {
                }
                rabbitmqcontrolprogress = false;
            }
        }

        public ApplicationStartResult Initialize(string url, string username, string password)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = url, UserName = username, Password = password, RequestedHeartbeat = TimeSpan.FromSeconds(30) };
                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                _url = url;
                _username = username;
                _password = password;

                rabbitmqcontrol.Elapsed += new System.Timers.ElapsedEventHandler(RabbitMqConnectionCheck);
                rabbitmqcontrol.Interval = 60000;
                rabbitmqcontrol.Enabled = true;

                return new ApplicationStartResult() { Success = true };
            }
            catch (Exception ex)
            {
                return new ApplicationStartResult() { Success = false, Exception = ex };
            }
        }

        public bool ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete)
        {
            try
            {
                channel.ExchangeDeclare(exchange: exchange, type: type, durable: durable, autoDelete: autoDelete);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Publish(string exchange, string routingKey, object data, IBasicProperties props = null)
        {
            try
            {
                channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: Serializers.Serialize(data));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Publish(string exchange, string routingKey, byte[] data, IBasicProperties props = null)
        {
            try
            {
                channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: props, body: data);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool QueueBind(string exchange, string routingKey, Action<string, byte[]> onReceive)
        {
            try
            {
                if (bindings.Count(x => x.Value == exchange + "|" + routingKey) == 0)
                {
                    string name = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: name, exchange: exchange, routingKey: routingKey);

                    EventingBasicConsumer cons = new EventingBasicConsumer(channel);

                    cons.Received += (model, ea) =>
                    {
                        onReceive(ea.RoutingKey, ea.Body.ToArray());
                    };

                    consumer = cons;

                    channel.BasicConsume(queue: name, autoAck: true, consumer: cons);

                    bindings.Add(new KeyValuePair<string, string>(name, exchange + "|" + routingKey));
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool QueueDelete(string exchange, string routingKey)
        {
            try
            {
                string key = exchange + "|" + routingKey;

                foreach (var item in bindings.Where(x => x.Value == key).ToList())
                {
                    if (item.Value == key)
                    {
                        channel.QueueDelete(item.Key);
                        bindings.Remove(item);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
