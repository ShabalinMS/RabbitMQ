using System.Text;

namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Сервис подписки
    /// </summary>
    public static class UseServiceCostomer
    {
        /// <summary>
        /// Клиент подписки
        /// </summary>
        private static IHandlerConsumer IncomingConsumer { get; set; }

        /// <summary>
        /// Подписка
        /// </summary>
        public static void Subscribe()
        {
            ConnectionRMQ conn = new ConnectionRMQ();

            var incomingMessageHandler = new ProcessingIncomingHandler();
            IncomingConsumer = new BaseHandlerCosumer(conn, incomingMessageHandler);
            IncomingConsumer.Subscribe();
        }

        /// <summary>
        /// Отписка
        /// </summary>
        public static void UnSubscribe()
        {
            if (IncomingConsumer == null)
            {
                return;
            }
            IncomingConsumer.Dispose();
            WriteLogHelper.WriteLogInfo("Unsubscribe OK");
        }
    }

    /// <summary>
    /// Процесс обработки входящих сообщений
    /// </summary>
    public class ProcessingIncomingHandler :IHandlerItem
    {
        /// <summary>
        /// Инициализирует обработчик входящих сообщений
        /// </summary>
        public ProcessingIncomingHandler() { }

        /// <summary>
        /// Выполняет обработку входящего сообщения.
        /// </summary>
        /// <param name="data">Данные входящего сообщения.</param>
        public virtual void Execute(byte[] data)
        {
            string incomingMessage;
            try
            {
                incomingMessage = Encoding.UTF8.GetString(data);
            }
            catch (Exception exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Couldn't get string representation of incoming message with information.");
                sb.Append(" Incoming message ignored. Inner exception: ");
                sb.Append(exception.Message);
                WriteLogHelper.WriteLogInfo(sb.ToString());
                return;
            }

            WriteLogHelper.WriteLogInfo(incomingMessage);

        }
    }
}
