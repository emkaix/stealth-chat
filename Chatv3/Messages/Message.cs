using Chatv3.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatv3.Messages
{
    class Message : IMessage
    {
        private string content;

        public string Content
        {
            get { return content; }
            set { content = value.TrimEnd('\0'); }
        }



        public string Sender  { get; set; }
        public DateTime TimeStamp { get; set; }

        public Message(string content, string sender)
        {
            Content = content;
            Sender = sender;
            TimeStamp = DateTime.Now;
        }

        
    }
}
