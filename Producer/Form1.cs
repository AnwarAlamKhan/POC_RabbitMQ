using System;
using System.Configuration;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Producer
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

            _emailChannel.ExchangeDeclare(exchange: "routing", type: ExchangeType.Direct);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
           

            var message = "This message needs to be routed";

            var body = Encoding.UTF8.GetBytes(message);

            _emailChannel.BasicPublish(exchange: "routing", routingKey: "both", null, body);
            //_emailChannel.BasicPublish(exchange: "routing", routingKey: "both", null, body);
        }
    }
}