using Microsoft.AspNetCore.Mvc;
using SQ.Common.Library.Models;
using SQ.Service.API.Interfaces;

namespace SQ.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageControl : ControllerBase
    {

        private static IPublishService _publishService;
        public MessageControl(IPublishService publishService) { _publishService = publishService; } 

        [HttpPost]
        [Route("publish")]
        public bool PublishMsg([FromBody] Message message)
        {
            return _publishService.Publish(message);
        }
    }
}
