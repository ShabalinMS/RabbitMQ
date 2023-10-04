namespace RabbitMQHelper.Utils
{
    /// <summary>
    /// IHandlerItem
    /// </summary>
    public interface IHandlerItem
    {
        /// <summary>
        /// Выполнить
        /// </summary>
        /// <param name="data">Передаваемые данные</param>
        void Execute(byte[] data);
    }
}
