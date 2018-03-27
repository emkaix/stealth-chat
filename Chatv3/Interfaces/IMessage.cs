using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatv3.Interfaces
{
    public interface IMessage
    {
        string Content { get; set; }
        string Sender { get; set; }
        DateTime TimeStamp { get; set; }
        
        
    }
}
    