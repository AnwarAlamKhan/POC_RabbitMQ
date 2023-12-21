using System;
using System.Configuration;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Request_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private IConnection _rabbitMqConnection;
        private IModel _emailChannel;
        private EventingBasicConsumer consumer ;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMqConnection"].ConnectionString;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(connectionString);
            // connectionFactory.AutomaticRecoveryEnabled = true;
            //connectionFactory.DispatchConsumersAsync = false;
            _rabbitMqConnection = connectionFactory.CreateConnection();
            _emailChannel = _rabbitMqConnection.CreateModel();

            //_emailChannel.ExchangeDeclare(exchange: "topic", type: ExchangeType.Topic, true);
        }

        private void btnSubscribeEmailQueue_Click(object sender, EventArgs e)
        {
            var replyQueue = _emailChannel.QueueDeclare("reply-queue", exclusive: false,durable:true, autoDelete: false);

            _emailChannel.QueueDeclare("request-queue", exclusive: false, durable: true, autoDelete: false);

             consumer = new EventingBasicConsumer(_emailChannel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                lstEmail.Invoke((MethodInvoker)(() => lstEmail.Items.Add($"Reply Recieved: {message}")));
                
            };

            _emailChannel.BasicConsume(queue: replyQueue.QueueName, autoAck: true, consumer: consumer);

        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            var properties = _emailChannel.CreateBasicProperties();
            properties.ReplyTo = "reply-queue";
            properties.CorrelationId = Guid.NewGuid().ToString();

            var message = txtPublishEmail.Text+ DateTime.Now.TimeOfDay.ToString() ;
            var body = Encoding.UTF8.GetBytes(message);
            _emailChannel.BasicPublish("", "request-queue", properties, body);
        }
    }
}