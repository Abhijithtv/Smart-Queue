using SQ.Service.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Service.API.GroupService
{
    public class ProducerService:IProducerService
    {
        public void Generate(string groupID, string currentToken)
        {
            // maybe do validation of groupID.
            //store the value in redis
            //take a batch of data from redis and write to file
            //remove the data from the batch of file.
        }
    }
}
