namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// Вспомогательный класс по работе с логами
    /// </summary>
    public static class WriteLogHelper
    {
        /// <summary>
        /// Запись лога информационного
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLogInfo(string text)
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            app.Logger.LogInformation(text);
        }

        /// <summary>
        /// Запись в лог об ошибке
        /// </summary>
        /// <param name="text">Текст ошибки в лог</param>
        public static void WriteLogError(string text)
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            app.Logger.LogError(text);
        }
    }
}
