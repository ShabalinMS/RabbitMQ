using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQHelper.Utils;
using static System.Net.Mime.MediaTypeNames;

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
        /// Получить все сообщения из очереди
        /// </summary>
        [HttpGet]
        public string Get()
        {
            (IEnumerable<string> list, bool isSuccess) = new RabbitMQHandler().GetMessage();
            return list.Count().ToString();
        }

        /// <summary>
        /// Подложить сообщение в очередь
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        [HttpPost]
        public string Post(string text) {
            return new RabbitMQHandler().SendMessage(text).ToString();
        }
    }
}
