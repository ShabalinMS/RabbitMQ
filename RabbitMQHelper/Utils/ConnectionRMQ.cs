using RabbitMQ.Client;

namespace RabbitMQHelper.Utils
{

    /// <summary>
    /// Сформированные данные для подключения
    /// </summary>
    public class ConnectionRMQ : IRMQConnectionFactory
    {
        /// <summary>
        /// Название очереди
        /// </summary>
        string IRMQConnectionFactory.QueueName
        {
            get
            {
                return "test";
            }
        }

        /// <summary>
        /// Получение данных для подключения
        /// </summary>
        /// <returns>Данные для подключения</returns>
        public ConnectionFactory GetRMQConnection()
        {
            GetSettingsMQ settingsMQ = new GetSettingsMQ();
            return settingsMQ.GetConnectionFactory();
        }
    }
}
