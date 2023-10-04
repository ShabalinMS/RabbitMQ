namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Запись в лог
    /// </summary>
    public class WriteLogHelper
    {
        /// <summary>
        /// Запись лога информационного
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        public static void WriteLogInfo(string text)
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            app.Logger.LogInformation(text);
        }
    }
}
