using RabbitMQ.Client.Events;

namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Базовый класс для клиента
    /// </summary>
    public class BaseHandlerCosumer : IHandlerConsumer
    {
        /// <summary>
        /// Базовый хендлер
        /// </summary>
        private readonly IHandlerItem _handlerItem;

        //private readonly ILog _logger;

        /// <summary>
        /// Таймер ожидания
        /// </summary>
        public int timeOut = 10000;

        /// <summary>
        /// События отправителя
        /// </summary>
        public EventingBasicConsumer EventingConsumer;

        /// <summary>
        /// Параметры для соединения
        /// </summary>
        public IRabbitMQConnection RabbitConnection;

        /// <summary>
        /// Строка подключения
        /// </summary>
        public string RabbitConnectionString;

        /// <summary>
        /// Название очереди
        /// </summary>
        public string QueueName
        {
            get;
            set;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionFactory">Фабрика подключения</param>
        /// <param name="handlerItem">Базовый хендлер</param>
        public BaseHandlerCosumer(IRMQConnectionFactory connectionFactory, IHandlerItem handlerItem) //, ILog logger)
        {
            RabbitConnection = new RabbitMQConnectionManagers(connectionFactory);//, logger);
            _handlerItem = handlerItem;
            QueueName = connectionFactory.QueueName;
            //_logger = logger;
        }

        /// <summary>
        /// Подписка
        /// </summary>
        public void Subscribe()
        {
            Execute();
        }

        /// <summary>
        /// Событие получения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        public virtual void EventingHandler_Received(object sender, BasicDeliverEventArgs ea)
        {
            try
            {
                byte[] body = ea.Body.ToArray();
                _handlerItem.Execute(ea.Body.ToArray());
                RabbitConnection.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                try
                {
                    //_logger.Error("ReturnOnError " + Encoding.UTF8.GetString(ea.Body.ToArray()));
                    Thread.Sleep(timeOut);
                    RabbitConnection.BasicNack(ea.DeliveryTag, false, true);
                }
                catch (Exception exception)
                {
                    //_logger.Error(string.Format("ReturnOnError in catch\n\rdata:\n\r{0},\n\rException: {1}", Encoding.UTF8.GetString(ea.Body.ToArray()), exception.ToString()));
                }
            }
        }

        /// <summary>
        /// Выполнить
        /// </summary>
        protected virtual void Execute()
        {
            EventingConsumer = RabbitConnection.CreateEventingBasicConsumer(QueueName, 0, 1);
            EventingConsumer.Received += EventingHandler_Received;
        }

        /// <summary>
        /// Завершение
        /// </summary>
        public virtual void Dispose()
        {
            EventingConsumer.Received -= EventingHandler_Received;
            RabbitConnection.Dispose();
        }

    }
}
