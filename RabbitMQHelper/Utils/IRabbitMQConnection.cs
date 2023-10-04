namespace RabbitMQHelper.Utils
{
    using RabbitMQ.Client.Events;

    /// <summary>
    /// Интерфейс работы с RabbitMQ
    /// </summary>
    public interface IRabbitMQConnection : IDisposable
    {
        #region Methods: Public
        /// <summary>
        ///  Подтвердите одно или несколько полученных сообщений
        /// </summary>
        /// <param name="deliveryTag">Тег доставки</param>
        /// <param name="multiply">множественный: следует ли пакетировать. true: все сообщения, меньшие чем deliveryTag, будут отклонены сразу.</param>
        void BasicAck(ulong deliveryTag, bool multiply);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryTag">индекс сообщения</param>
        /// <param name="multiple">множественный: следует ли пакетировать. true: все сообщения, меньшие чем deliveryTag, будут отклонены сразу.</param>
        /// <param name="requeue">требование: Отказать ли повторно войти в очередь.</param>
        void BasicNack(ulong deliveryTag, bool multiple, bool requeue);

        /// <summary>
        /// Публикация сообщения
        /// </summary>
        /// <param name="data">Данные для передачи</param>
        void PublishToQueue(object data);

        /// <summary>
        /// Публикация сообщения
        /// </summary>
        /// <param name="queueName">Название очереди</param>
        /// <param name="message">Сообщение</param>
        void PublishToQueue(string queueName, string message);

        /// <summary>
        /// Публикация сообщения
        /// </summary>
        /// <param name="queueName">Название очереди</param>
        /// <param name="body">Тело письма</param>
        void PublishToQueue(string queueName, byte[] body);

        /// <summary>
        /// Удаление сообщения из очереди
        /// </summary>
        /// <param name="busEndpoint">Конечная точка</param>
        /// <param name="ifUnused">Неиспользуемый</param>
        /// <param name="ifEmpty">Пустой</param>
        void QueueDelete(string busEndpoint, bool ifUnused, bool ifEmpty);

        /// <summary>
        /// Собатия клиента
        /// </summary>
        /// <param name="queueName">Название очереди</param>
        /// <param name="prefetchSize">Размер предварительной выборки</param>
        /// <param name="prefetchCount">Колличество предварительной выборки</param>
        /// <returns>Событие клиента</returns>
        EventingBasicConsumer CreateEventingBasicConsumer(string queueName,
                    ushort prefetchSize, ushort prefetchCount);
        #endregion
    }
}
