namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Интерфейс хендлера клиента
    /// </summary>
    public interface IHandlerConsumer : IDisposable
    {
        /// <summary>
        /// Название очереди
        /// </summary>
        string QueueName { get; set; }

        /// <summary>
        /// Подписка
        /// </summary>
        void Subscribe();

    }
}
