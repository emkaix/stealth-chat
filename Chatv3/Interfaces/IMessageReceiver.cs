using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatv3.Interfaces
{
    interface IMessageReceiver
    {
        Task<IEnumerable<IMessage>> Receive();
    }
}
