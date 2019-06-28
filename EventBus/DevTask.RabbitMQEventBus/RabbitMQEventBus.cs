using DevTask.EvenBus;
using DevTask.EvenBus.DomainEvents;
using DevTask.EvenBus.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DevTask.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus
    {
        IConnection connection;
        public string BrokerName { get; set; }
        public string QueueName { get; set; }

        public string HostName { get; set; }

        private IModel _consumerChannel;

        private KYCEventHandlerFactory _subscriptionFactory;

        private void InitiateBasicConsumer(string queuename)
        {
            
                _consumerChannel = connection.CreateModel();
                _consumerChannel.QueueDeclare(queue: queuename,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(queue: queuename,
                                  autoAck: true,
                                  consumer: consumer);

            
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
           
                var eventName = eventArgs.RoutingKey;
                var message = Encoding.UTF8.GetString(eventArgs.Body);
                HandelEvent(eventName, message);
           
           

           
        }

        private void HandelEvent(string eventName, string message)
        {
            var eventHanlder = _subscriptionFactory.GetEventHandlerByEvent(eventName);
            eventHanlder.Handle(message);
        }

        public RabbitMQEventBus(string hostName, string brokerName)
        {
            HostName = hostName;           
            BrokerName = brokerName;
            _subscriptionFactory = new KYCEventHandlerFactory();

            var factory = new ConnectionFactory() { HostName = HostName };
            connection = factory.CreateConnection();
            

            }

        public void Publish(KYCEvent kYCEvent)
        {
            string eventName = kYCEvent.GetType().Name;
            var factory = new ConnectionFactory() { HostName = HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: eventName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                string message = JsonConvert.SerializeObject(kYCEvent);
                var body = Encoding.UTF8.GetBytes(message);


                

                channel.BasicPublish(exchange: "",
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: null,
                        body: body);

            }
        }

        public void Subscribe(string eventName, Type handler)
        {
            InitiateBasicConsumer(eventName);
            _subscriptionFactory.AddSubscription(eventName, handler);
        }
    }
}
