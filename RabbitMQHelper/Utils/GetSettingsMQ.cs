using RabbitMQ.Client;

namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Класс для подготовки настроек для действий с rabbitMQ
    /// </summary>
    public class GetSettingsMQ
    {
        #region Param Connection

        /// <summary>
        /// Имя пользователя
        /// </summary>
        private const string USERNAMECONF = "UserNameRabbitMQ";

        /// <summary>
        /// Пароль для подключения
        /// </summary>
        private const string PASSWORDCONF = "PasswordRabbitMQ";

        /// <summary>
        /// Порт
        /// </summary>
        private const string PORTCONF = "PortRabbitMQ";

        /// <summary>
        /// Наименование виртуального хоста
        /// </summary>
        private const string HOSTNAMECONF = "HostNameRabbitMQ";

        #endregion

        #region Param Channel

        /// <summary>
        /// Пункта сбора распределения
        /// </summary>
        private string _exchangeName;

        /// <summary>
        /// Наименования очереди
        /// </summary>
        private  string _queueName;

        /// <summary>
        /// Ключ для распределения
        /// </summary>
        private string _routingKey;

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="exchangeName">Пункта сбора распределения</param>
        /// <param name="queueName">Наименования очереди</param>
        /// <param name="routingKey">Ключ для распределения</param>
        public GetSettingsMQ()
        {
            _exchangeName = "test";
            _queueName = "test1";
            _routingKey = "test";
        }

        #region Method Public

        /// <summary>
        /// Получение параметров для подключения
        /// </summary>
        /// <exception cref="ArgumentNullException">Аргумента для установки соединения неудачное</exception>
        /// <exception cref="FormatException">Порт не соответствует формату</exception>
        /// <exception cref="RabbitMQ.Client.Exceptions.BrokerUnreachableException">Соединение не установлено</exception>
        public (IConnection, IModel, string, string, string) OpenConnection()
        {
            string? userName = System.Configuration.ConfigurationManager.AppSettings[USERNAMECONF];
            if (userName == null) throw new ArgumentNullException($"Fill in the system setup {USERNAMECONF}");

            string? password = System.Configuration.ConfigurationManager.AppSettings[PASSWORDCONF];
            if (password == null) throw new ArgumentNullException($"Fill in the system setup {PASSWORDCONF}");

            string? host = System.Configuration.ConfigurationManager.AppSettings[HOSTNAMECONF];
            if (host == null) throw new ArgumentNullException($"Fill in the system setup {HOSTNAMECONF}");

            string? port = System.Configuration.ConfigurationManager.AppSettings[PORTCONF];
            if (port == null) throw new ArgumentNullException($"Fill in the system setup {PORTCONF}");

            int portInt = 0;
            int.TryParse(port, out portInt);

            if (portInt == 0)
                throw new FormatException($"the setting {PORTCONF} equals 0");

            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = userName,
                Password = password,
                HostName = host,
                Port = portInt
            };

            IConnection conn = factory.CreateConnection();

            WriteLogHelper.WriteLogInfo("Сonnection is set up");

            return (conn, GetRabbitChannel(conn), _exchangeName, _queueName, _routingKey);
        }

        #endregion

        #region Method Private

        /// <summary>
        /// Получение настройки канала
        /// </summary>
        /// <returns>Параметры для канала <see href="IModel">IModel</see></returns>
        /// <exception cref="RabbitMQ.Client.Exceptions.OperationInterruptedException">Не удалось настроить канал</exception>
        private IModel GetRabbitChannel(IConnection conn)
        {
            IModel model = conn.CreateModel();

            if (string.IsNullOrWhiteSpace(_exchangeName))
            {
                throw new ArgumentNullException("Argument exchangeName is string.Empty");
            }
            try
            {
                model.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            }
            catch (RabbitMQ.Client.Exceptions.OperationInterruptedException operationEx)
            {
                throw new Exception($"Incorrect Exchange settings");
            }

            if (string.IsNullOrWhiteSpace(_queueName))
            {
                throw new ArgumentNullException("Argument queueName is string.Empty");
            }
            try
            {
                model.QueueDeclare(_queueName, false, false, false, null);
            }
            catch (RabbitMQ.Client.Exceptions.OperationInterruptedException operationEx)
            {
                throw new Exception("Incorrect queue settings");
            }

            if (string.IsNullOrWhiteSpace(_routingKey))
            {
                throw new ArgumentNullException("Argument routingKey is string.Empty");
            }

            model.QueueBind(_queueName, _exchangeName, _routingKey, null);

            WriteLogHelper.WriteLogInfo("The channel is configured");

            return model;
        }
        #endregion
    }
}
