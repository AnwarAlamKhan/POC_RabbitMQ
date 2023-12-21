using System;
using System.Configuration;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PaymentsConsumer
{
    public partial class Form1 : Form
    {
        private IConnection connection;
        private IModel channel;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubscribeEmailQueue_Click(object sender, EventArgs e)
        {
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "routing", type: ExchangeType.Direct);

            var queueName = channel.QueueDeclare().QueueName;

            //channel.QueueBind(queue: queueName, exchange: "routing", routingKey: "paymentsonly");
            channel.QueueBind(queue: queueName, exchange: "routing", routingKey: "both");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                lstEmail.Invoke((MethodInvoker)(() => lstEmail.Items.Add($"Payments - Recieved new message: {message}")));
                //Console.WriteLine($"Payments - Recieved new message: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMqConnection"].ConnectionString;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(connectionString);
            // connectionFactory.AutomaticRecoveryEnabled = true;
            //connectionFactory.DispatchConsumersAsync = false;
            connection = connectionFactory.CreateConnection();
        }
    }
}