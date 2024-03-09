using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Service.API.Interfaces
{
    public interface IGenerateGroupService
    {
        public string GenerateGroup(string ownerId);
        public int JoinGroupService(string groupId);
        public Task<bool> StopGroupServer(string ownerId);

    }
    
}
