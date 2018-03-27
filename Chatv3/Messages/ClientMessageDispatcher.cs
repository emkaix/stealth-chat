using Chatv3.ClientServer;
using Chatv3.Interfaces;
using Chatv3.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatv3.Messages
{
    class ClientMessageDispatcher : IMessageDispatcher
    {
        Client client;

        public ClientMessageDispatcher(Client _client)
        {
            client = _client;
        }

        /// <summary>
        /// Sendet eine IMessage an den Server
        /// </summary>
        /// <param name="message">Die Nachricht, die an den Server gesendet werden soll</param>
        public async void Send(IMessage message)
        {
            var stream = client.GetStream();
            if (stream == null) return;

            var messageBytes = JsonHelper.SerializeMessage(message).ToArray();
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }


        
    }
}
