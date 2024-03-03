using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Models
{
    public class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string OwnerId { get; set; }

        public int TotalMembers { get; set; }

        /// <summary>
        /// change its value as active member move out of it.
        /// </summary>
        public string ActiveMembers { get; set; }


    }
}
