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
        
        public string GenerateGroup(string ownerId)
        {
            _EnsureValidOwner(ownerId);
            string groupUniqueId = _GenerateGroup(ownerId);
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
    }
}
