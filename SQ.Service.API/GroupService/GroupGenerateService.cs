using SQ.Common.Library.Handlers;
using SQ.Common.Library.Helpers;
using SQ.Service.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Service.API.GroupService
{
    public class GroupGenerateService :IGenerateGroupService
    {
        public static Dictionary<string,string> OwnerGroupMapping = new Dictionary<string,string>();
        public string GenerateGroup(string ownerId)
        {
            _EnsureValidOwner(ownerId);
            string groupUniqueId = _GenerateGroup(ownerId);

            NetworkHelperV2.CreateServerSocket(groupUniqueId);
            OwnerGroupMapping.Add(ownerId, groupUniqueId);
            ThreadHandler.AddWork(() => NetworkHelperV2.GroupServerStart(groupUniqueId));
            
            return groupUniqueId;
        }

        void _EnsureValidOwner(string ownerId)
        {
            if (string.IsNullOrEmpty(ownerId))
                throw new Exception("ownerId Cant be null or empty");

            //todo: check owner id present
        }

        string _GenerateGroup(string ownerId)
        {
            return ownerId + DateTime.Now.Hour.ToString() + DateTime.Now.Second.ToString();
        }

        public int JoinGroupService(string groupId)
        {
            return NetworkHelperV2.TryGetGroupServerPort(groupId);
        }

        public async Task<bool> StopGroupServer(string ownerId) 
        {
            //todo: from db get ownerId to GroupMapping
            string groupUniqueId;
            if(!OwnerGroupMapping.TryGetValue(ownerId, out groupUniqueId)) { return false; }
            var ok = await NetworkHelperV2.GroupServerStop(groupUniqueId);
            if (ok) { OwnerGroupMapping.Remove(ownerId); }
            return ok;
        }
    }
}
