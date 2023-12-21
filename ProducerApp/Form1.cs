using System;
using System.Configuration;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ProducerApp
{
    public partial class Form1 : Form
    {
        private IConnection _rabbitMqConnection;
        private IModel _emailChannel;
        public Form1()
        {
            InitializeComponent();
        }

       

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            using var channel = _rabbitMqConnection.CreateModel();

           

            channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);


            var message = "Hello I want to broadcast this message";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "pubsub", "", null, body);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMqConnection"].ConnectionString;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(connectionString);
            //connectionFactory.AutomaticRecoveryEnabled = true;
            //connectionFactory.DispatchConsumersAsync = true;
            _rabbitMqConnection = connectionFactory.CreateConnection();

        }

    }
}