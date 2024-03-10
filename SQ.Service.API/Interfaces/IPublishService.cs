using SQ.Common.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Service.API.Interfaces
{
    public interface IPublishService
    {
        public bool Publish(Message message);

    }
}
