using System;
using System.Configuration;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace TopicConsumer2
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

            channel.ExchangeDeclare(exchange: "topic", type: ExchangeType.Topic,true);

            var queueName =channel.QueueDeclare().QueueName;

            channel.QueueBind(queue: queueName, exchange: "topic", routingKey: "*.sweden.*");
            channel.QueueBind(queue: "testQueue", exchange: "topic", routingKey: "user.#");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                lstEmail.Invoke((MethodInvoker)(() => lstEmail.Items.Add($"Topic2 - Recieved new message: {message}")));
                //Console.WriteLine($"Payments - Recieved new message: {message}");
            };

            channel.BasicConsume(queue: "testQueue", autoAck: true, consumer: consumer);
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