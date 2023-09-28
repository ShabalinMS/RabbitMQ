using RabbitMQ.Client;

namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Класс для работы с rabbitMQ
    /// </summary>
    public class RabbitMQHandler : RabbitMQHandlerBase
    {
        /// <summary>
        /// Установка настроек
        /// </summary>
        /// <returns>(Подключение)(Канал)(Exchange)(Channel)(Queue)</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override (IConnection, IModel, string, string, string) GetSettingMQ()
        {
            return new GetSettingsMQ().OpenConnection();
        }
    }
}
