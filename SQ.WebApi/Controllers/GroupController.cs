using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQ.Service.API.Interfaces;

namespace SQ.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGenerateGroupService _GenerateGroupService;
        public GroupController(IGenerateGroupService IGenerateGroupService)
        {
            _GenerateGroupService = IGenerateGroupService;
        }

        [HttpGet]
        public string CreateGroup(string ownerId)
        {
            return _GenerateGroupService.GenerateGroup(ownerId);
        }

    }
}
