using System;
using System.Configuration;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TopicConsumer1
{
    public partial class Form1 : Form
    {
        private IConnection _rabbitMqConnection;
        private IModel _emailChannel;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMqConnection"].ConnectionString;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(connectionString);
            // connectionFactory.AutomaticRecoveryEnabled = true;
            //connectionFactory.DispatchConsumersAsync = false;
            _rabbitMqConnection = connectionFactory.CreateConnection();
            _emailChannel = _rabbitMqConnection.CreateModel();

            _emailChannel.ExchangeDeclare(exchange: "topic", type: ExchangeType.Topic,true);

           
        }

        private void btnSubscribeEmailQueue_Click(object sender, EventArgs e)
        {
            var queueName = _emailChannel.QueueDeclare().QueueName;

            _emailChannel.QueueBind(queue: queueName, exchange: "topic", routingKey: "*.sweden.*");
            

            var consumer = new EventingBasicConsumer(_emailChannel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                lstEmail.Invoke((MethodInvoker)(() => lstEmail.Items.Add($"Topic1 - Recieved new message: {message}")));
            };

            _emailChannel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

     }
}