namespace RabbitMQHelper.Utils
{

    using RabbitMQ.Client;

    /// <summary>
    /// Интерфейс фабрики подключения
    /// </summary>
    public interface IRMQConnectionFactory
    {
        /// <summary>
        /// Получить фабрику подключения
        /// </summary>
        /// <returns><see cref="ConnectionFactory">ConnectionFactory</see></returns>
        ConnectionFactory GetRMQConnection();

        /// <summary>
        /// Название очереди
        /// </summary>
        string QueueName { get;}
    }
}
