using SQ.Common.Library.Handlers;
using SQ.Common.Library.Helpers;
using SQ.Common.Library.Models;
using SQ.Service.API.GroupService;
using SQ.Service.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Service.API.MessageService
{
    public class PublishService : IPublishService
    {
        

        public PublishService() { }
        public bool Publish(Message message)
        {
            //check if group exist
            SocketWrapper? socket;
            NetworkHelperV2.SocketServers.TryGetValue(message.groupId, out socket);
           
            //check if its his group
            string? groupID;
            GroupGenerateService.OwnerGroupMapping.TryGetValue(message.OwnerId, out groupID);

            if(string.IsNullOrEmpty(groupID) || (socket==null))
            {
                return false;
            }

            //put the data in msg queue for later publish
            MessageHandler.Messages.Enqueue(message);
            ThreadHandler.Publish();
            return true;
        }

    }
}
