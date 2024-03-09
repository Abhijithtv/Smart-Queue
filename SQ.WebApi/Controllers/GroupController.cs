using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQ.Service.API.Interfaces;

namespace SQ.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        static int max_Group = 100;
        private IGenerateGroupService _GenerateGroupService;
        public GroupController(IGenerateGroupService IGenerateGroupService)
        {
            _GenerateGroupService = IGenerateGroupService;
        }

        [HttpGet]
        [Route("create")]
        public string CreateGroup(string ownerId)
        {
            return _GenerateGroupService.GenerateGroup(ownerId);
        }

        [HttpGet]
        [Route("delete")]
        public async Task<bool>  DeleteGroup(string ownerId) 
        {
            var res = await  _GenerateGroupService.StopGroupServer(ownerId);
            return res;
        }

        [HttpGet]
        [Route("join")]
        public int JoinGroup(string groupId)
        {
            return _GenerateGroupService.JoinGroupService(groupId);
        }

    }
}
