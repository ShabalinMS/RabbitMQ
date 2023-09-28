using RabbitMQ.Client;
using System.Text;

namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Интерфейс реализации работы с очередью RabbitMQ
    /// </summary>
    public abstract class RabbitMQHandlerBase
    {
        #region Param Channel

        /// <summary>
        /// Пункта сбора распределения
        /// </summary>
        protected string _exchangeName;

        /// <summary>
        /// Наименования очереди
        /// </summary>
        protected string _queueName;

        /// <summary>
        /// Ключ для распределения
        /// </summary>
        protected string _routingKey;

        #endregion

        #region Method Public

        /// <summary>
        /// Получение найстройки очереди
        /// </summary>
        /// <returns>(Подключение)(Канал)(Exchange)(Channel)(Queue)</returns>
        public abstract (IConnection, IModel, string, string, string) GetSettingMQ();

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        /// <param name="text">Текст Сообщения</param>
        /// <returns>Успешно?</returns>
        public bool SendMessage(string text)
        {
            IModel model = null;
            IConnection conn = null;
            bool IsSuccess = true;
            try
            {
                (conn, model, _exchangeName, _queueName, _routingKey) = GetSettingMQ();
                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(text);
                model.BasicPublish(_exchangeName, _routingKey, null, messageBodyBytes);
                WriteLogHelper.WriteLogInfo($"The message has been sent, text {text}");
            } catch (Exception ex)
            {
                WriteLogHelper.WriteLogError(ex.ToString());
                IsSuccess = false;
            }
            finally 
            {
                RabbitMQHelper.CloseConnection(model, conn); 
            }
            return IsSuccess;
        }

        /// <summary>
        /// Получить сообщения
        /// </summary>
        /// <returns>{Коллекцию сообщений} {Успешно?}</returns>
        public (IEnumerable<string>, bool) GetMessage()
        {
            var listMessages = new List<string>();
            bool IsSuccess = true;
            IModel model = null;
            IConnection conn = null;

            try
            {
                (conn, model, _exchangeName, _queueName, _routingKey) = GetSettingMQ();
                while (true)
                {
                    BasicGetResult result = model.BasicGet(_queueName, true);
                    if (result == null)
                    {
                        break;
                    }
                    else
                    {
                        byte[] body = result.Body.ToArray();
                        listMessages.Add(Encoding.UTF8.GetString(body));
                    }
                }
            } catch 
            {
                IsSuccess = false;
            }
            finally 
            {
                RabbitMQHelper.CloseConnection(model, conn); 
            }
            return (listMessages, IsSuccess);
        }

        #endregion
    }
}
