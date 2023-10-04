namespace RabbitMQHelper.Utils
{
    using RabbitMQ.Client.Events;
    using RabbitMQ.Client;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Менеджер работы с RabbitMQ
    /// </summary>
    public class RabbitMQConnectionManagers : IRabbitMQConnection
    {
        /// <summary>
        /// Интервал переподключения для сохранения активности
        /// </summary>
        private readonly int _NetworkRecoveryInterval = 10;

        /// <summary>
        /// Фабрика подключения
        /// </summary>
        private ConnectionFactory RabbitMQConnectionFactory
        {
            get; set;
        }

        /// <summary>
        /// Привязки
        /// </summary>
        private IBasicProperties _rabbitProperties
        {
            get; set;
        }

        /// <summary>
        /// Название очереди
        /// </summary>
        private string _queueName;

        //private readonly ILog _logger;

        /// <summary>
        /// Канал
        /// </summary>
        public IModel RabbitChannel
        {
            get; set;
        }

        /// <summary>
        /// Соединение с rabbitMQ
        /// </summary>
        public IConnection Connection
        {
            get; set;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionFactory">Фабрика подключения</param>
        public RabbitMQConnectionManagers(IRMQConnectionFactory connectionFactory)//, ILog logger)
        {
            RabbitMQConnectionFactory = connectionFactory.GetRMQConnection();
            _queueName = connectionFactory.QueueName;
            //_logger = logger;
            SetAutomaticRecovery();
            OpenRabbitMQConnection();
        }

        /// <summary>
        /// Открытие подключения
        /// </summary>
        private void OpenRabbitMQConnection()
        {
            Connection = RabbitMQConnectionFactory.CreateConnection();
            RabbitChannel = Connection.CreateModel();
            RabbitChannel.QueueDeclare(_queueName, true, false, false, null);
            RabbitChannel.ConfirmSelect();
            _rabbitProperties = RabbitChannel.CreateBasicProperties();
            _rabbitProperties.Persistent = true;
        }

        /// <summary>
        /// Поддержания открытого канала
        /// </summary>
        private void SetAutomaticRecovery()
        {
            RabbitMQConnectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(_NetworkRecoveryInterval);
            RabbitMQConnectionFactory.AutomaticRecoveryEnabled = true;
        }

        /// <summary>
        /// Создания событий клиента
        /// </summary>
        /// <param name="queueName">Название очереди</param>
        /// <param name="prefetchSize">Размер предварительной выборки</param>
        /// <param name="prefetchCount">Предварительное колличество</param>
        /// <returns>Собатие клиента</returns>
        public EventingBasicConsumer CreateEventingBasicConsumer(string queueName,
                    ushort prefetchSize, ushort prefetchCount)
        {
            RabbitChannel.BasicQos(prefetchSize, prefetchCount, false);
            var rabbitConsumer = new EventingBasicConsumer(RabbitChannel);
            RabbitChannel.BasicConsume(queueName, false, rabbitConsumer);
            return rabbitConsumer;
        }

        /// <summary>
        /// Публикация сообщения в очередь
        /// </summary>
        /// <param name="data">Объект для сериализации и отправки</param>
        public void PublishToQueue(object data)
        {
            string message = JsonConvert.SerializeObject(data);
            PublishToQueue(_queueName, message);
        }

        /// <summary>
        /// Публикация сообщения в очередь
        /// </summary>
        /// <param name="queueName">Название очереди</param>
        /// <param name="message">Текст сообзения</param>
        public void PublishToQueue(string queueName, string message)
        {
            //if (_logger != null)
            {
            //    _logger.Info(string.Format("Request: {0}; QueueName: {1}; Data: {2}", RabbitChannel.ChannelNumber, _queueName, message));
            }
            byte[] body = Encoding.UTF8.GetBytes(message);
            RabbitChannel.BasicPublish("", queueName, _rabbitProperties, body);
        }

        /// <summary>
        /// Опебликовать в очередь
        /// </summary>
        /// <param name="queueName">Наименование очереди</param>
        /// <param name="body">Текст сообщения</param>
        public void PublishToQueue(string queueName, byte[] body) =>
            RabbitChannel.BasicPublish("", queueName, _rabbitProperties, body);

        /// <summary>
        ///Разъединение
        /// </summary>
        public void Dispose()
        {
            RabbitChannel.Dispose();
            Connection.Dispose();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryTag">Тег доставки</param>
        /// <param name="multiple">Множественный</param>
        public void BasicAck(ulong deliveryTag, bool multiple) => RabbitChannel.BasicAck(deliveryTag, multiple);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryTag">Тег доставки</param>
        /// <param name="multiple">Множественный</param>
        /// <param name="requeue">Запрос</param>
        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue) => RabbitChannel.BasicNack(deliveryTag, multiple, requeue);

        /// <summary>
        /// Удаление из очереди
        /// </summary>
        /// <param name="busEndpoint">Конечная точка</param>
        /// <param name="ifUnused">Неиспользуемый</param>
        /// <param name="ifEmpty">Пустой</param>
        public void QueueDelete(string busEndpoint, bool ifUnused, bool ifEmpty) => RabbitChannel.QueueDelete(busEndpoint, ifUnused, ifEmpty);

    }
}
