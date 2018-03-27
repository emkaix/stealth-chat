using Chatv3.ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatv3.Interfaces
{
    interface IMessageDispatcher
    {
        void Send(IMessage message);
    }
}
