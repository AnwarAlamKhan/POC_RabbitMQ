using System;
using System.Configuration;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace TopicProducer
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

        private void btnSendMessage_Click(object sender, EventArgs e)
        {


            var userPaymentsMessage = "A european user paid for something";

            var userPaymentsBody = Encoding.UTF8.GetBytes(userPaymentsMessage);

            _emailChannel.BasicPublish(exchange: "topic", routingKey: "user.sweden.payments", null, userPaymentsBody);


            var businessOrderMessage = "A sweden business ordered goods";

            var businessOrderBody = Encoding.UTF8.GetBytes(businessOrderMessage);

            _emailChannel.BasicPublish(exchange: "topic", routingKey: "business.sweden.order", null, businessOrderBody);

            
        }
    }
}