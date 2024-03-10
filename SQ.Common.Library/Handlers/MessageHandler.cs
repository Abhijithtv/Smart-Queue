using SQ.Common.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQ.Common.Library.Handlers
{
    public class MessageHandler
    {
        public static Queue<Message> Messages = new Queue<Message>();

        public static void ProcessMessage()
        {
            while (true)
            {
                if (MessageHandler.Messages.Count > 0)
                {
                    var message = Messages.Dequeue();
                    FileHandler.Write(FileHandler.GroupIdToFileName(message.groupId), FileDateFormatter(message.Data));
                }
            }
        }

        private static string FileDateFormatter(string data)
        {
            return data + Environment.NewLine;
        }
    }
}
