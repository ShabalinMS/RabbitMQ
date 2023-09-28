using RabbitMQ.Client;

namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Вспомогательный класс по работе с MQ
    /// </summary>
    public static class RabbitMQHelper
    {
        /// <summary>
        /// Закрытие подключения
        /// </summary>
        public static void CloseConnection(IModel model, IConnection conn)
        {
            if (model != null)
            {
                model.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            WriteLogHelper.WriteLogInfo("Сonnection close");
        }
    }
}
