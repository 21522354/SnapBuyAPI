using Newtonsoft.Json;
using OrderService.Models.Dtos.ResponseModels;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        // Constructor: Thiết lập kết nối và channel khi khởi tạo
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            // Khởi tạo kết nối và channel trong hàm khởi tạo
            var factory = new ConnectionFactory { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };
            _connection = factory.CreateConnection();  // Tạo kết nối
            _channel = _connection.CreateModel();      // Tạo channel

            // Đảm bảo rằng queue đã được tạo sẵn
            _channel.QueueDeclare("noti", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        // Phương thức để publish notification mới
        public async Task PublishNewNotification(MRes_Notification notification)
        {
            var json = JsonConvert.SerializeObject(notification);
            var body = Encoding.UTF8.GetBytes(json);

            // Tái sử dụng channel đã tạo
            _channel.BasicPublish(
                exchange: "noti",
                routingKey: "",
                body: body);
            await Task.CompletedTask;
        }

        // Phương thức hủy kết nối (nên được gọi khi không còn sử dụng MessageBusClient)
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
