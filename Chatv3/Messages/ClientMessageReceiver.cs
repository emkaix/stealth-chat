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
    class ClientMessageReceiver : IMessageReceiver
    {
        Client client;

        public ClientMessageReceiver(Client _client)
        {
            client = _client;
        }

        public async Task<IEnumerable<IMessage>> Receive()
        {
            var stream = client.GetStream();
            if (stream == null || !stream.DataAvailable) return null;

            var receiveBufferSize = client.GetReceiveBufferSize();
            var buffer = new byte[receiveBufferSize];
            await stream.ReadAsync(buffer, 0, receiveBufferSize);
            var message = JsonHelper.DeserializeMessage(buffer);

            var list = new List<IMessage>();
            list.Add(message);

            return list;
        }
    }
}
