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
    class ServerMessageReceiver : IMessageReceiver
    {
        Server server;

        public ServerMessageReceiver(Server _server)
        {
            server = _server;
        }

        public async Task<IEnumerable<IMessage>> Receive()
        {
            var list = new List<IMessage>();

            foreach (var client in server.TcpClients)
            {
                var stream = client.GetStream();
                if (stream == null || !stream.DataAvailable) continue;

                var receiveBufferSize = client.ReceiveBufferSize;
                var buffer = new byte[receiveBufferSize];
                await stream.ReadAsync(buffer, 0, receiveBufferSize);
                var message = JsonHelper.DeserializeMessage(buffer);

                list.Add(message);
            }
            return list;
        }
    }
}
