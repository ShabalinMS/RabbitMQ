using Microsoft.AspNetCore.Mvc;
using RabbitMQHelper.Utils;

namespace RabbitMQHelper.Controllers
{
    /// <summary>
    /// Констроллер для работы с сообщениями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// Разрыв соединения
        /// </summary>
        [HttpGet]
        public void Get()
        {
            UseServiceCostomer.UnSubscribe();
        }

        /// <summary>
        /// Подложить сообщение в очередь
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        [HttpPost]
        public string Post(string text) {
            string queueName = "test";

            ConnectionRMQ conn = new ConnectionRMQ();
            using (RabbitMQConnectionManagers rcManager = new RabbitMQConnectionManagers(conn))
            {
                rcManager.PublishToQueue(queueName, text);
            }

            return "true";
        }
    }
}
