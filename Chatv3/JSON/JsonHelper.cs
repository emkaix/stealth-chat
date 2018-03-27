using Chatv3.Interfaces;
using Chatv3.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatv3.JSON
{
    public static class JsonHelper
    {
        public class RootObject
        {
            public IEnumerable<string> IPAddresses { get; set; }
            public string UserName { get; set; }
        }

        /// <summary>
        /// Die Nachricht als Byte Array
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<byte> SerializeMessage(IMessage message)
        {
            var jsonString = JsonConvert.SerializeObject(message);
            return Encoding.ASCII.GetBytes(jsonString);
        }

        public static IMessage DeserializeMessage(IEnumerable<byte> data)
        {
            var jsonString = Encoding.ASCII.GetString(data.ToArray());
            return JsonConvert.DeserializeObject<Message>(jsonString);
        }

    }
}
