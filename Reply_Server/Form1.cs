using System;
using System.Configuration;
using System.Text;
using System.Threading.Channels;
using System.Windows.Forms;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Reply_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private IConnection _rabbitMqConnection;
        private IModel _emailChannel;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["RabbitMqConnection"].ConnectionString;
            var connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri(connectionString);
            // connectionFactory.AutomaticRecoveryEnabled = true;
            //connectionFactory.DispatchConsumersAsync = false;
            _rabbitMqConnection = connectionFactory.CreateConnection();
            _emailChannel = _rabbitMqConnection.CreateModel();
        }

        private void btnSubscribeEmailQueue_Click(object sender, EventArgs e)
        {
            _emailChannel.QueueDeclare("request-queue", exclusive: false, durable: true,autoDelete:false);

            var consumer = new EventingBasicConsumer(_emailChannel);

            consumer.Received += (model, ea) =>
            {
                
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                lstEmail.Invoke((MethodInvoker)(() => lstEmail.Items.Add($"Reply Recieved: {message + ea.BasicProperties.CorrelationId}")));

                var replyMessage = $"This is your reply Data is processed: {ea.BasicProperties.CorrelationId}";

                body = Encoding.UTF8.GetBytes(replyMessage);

                Thread.Sleep(5000);
                _emailChannel.BasicPublish("", ea.BasicProperties.ReplyTo, null, body);
            };

            _emailChannel.BasicConsume(queue: "request-queue", autoAck: true, consumer: consumer);

        }
    }
}