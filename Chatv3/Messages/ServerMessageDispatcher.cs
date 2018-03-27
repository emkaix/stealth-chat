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
    class ServerMessageDispatcher : IMessageDispatcher
    {
        Server server;

        public ServerMessageDispatcher(Server _server)
        {
            server = _server;
        }

        /// <summary>
        /// Sendet eine IMessage an alle TCPClients in der TCPClient Liste des Servers
        /// </summary>
        /// <param name="message">Die Nachricht, die an alle Clients in der Liste gesendet werden soll</param>
        public async void Send(IMessage message)
        {
            foreach (var client in server.TcpClients)
            {
                var stream = client.GetStream();
                if (stream == null || !client.Connected) continue;

                var messageBytes = JsonHelper.SerializeMessage(message).ToArray();
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
            }
        }
    }
}
