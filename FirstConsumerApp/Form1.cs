using System;
using System.Configuration;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FirstConsumerApp
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
        }

        private void btnSubscribeEmailQueue_Click(object sender, EventArgs e)
        {
            _emailChannel = _rabbitMqConnection.CreateModel();

            _emailChannel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

            var queueName = _emailChannel.QueueDeclare().QueueName;

            _emailChannel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: "");

            var consumer = new EventingBasicConsumer(_emailChannel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                lstEmail.Invoke((MethodInvoker)(() => lstEmail.Items.Add("First " + message)));
            };

            _emailChannel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }

}