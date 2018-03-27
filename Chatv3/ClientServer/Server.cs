using Chatv3.Interfaces;
using Chatv3.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chatv3.ClientServer
{ 
    public class Server
    {
        private TcpListener tcpListener;
        public List<TcpClient> TcpClients { get; set; }

        public Server()
        {
            tcpListener = new TcpListener(IPAddress.Any, 5000);
            TcpClients = new List<TcpClient>();
        }

        public void StartListeningToClients()
        {
            tcpListener.Start();
        }

        public bool ConnectionsPending()
        {
            return tcpListener.Pending();
        }

        public async Task<TcpClient> AcceptPendingConnection()
        {
            var client = await tcpListener.AcceptTcpClientAsync();
            TcpClients.Add(client);
            return client;
        }

        public void CloseAllConnections()
        {
            foreach (var client in TcpClients)
            {
                var messageDispatcher = new ServerMessageDispatcher(this);
                messageDispatcher.Send(new Message("CONNECTION_CLOSE", client.Client.LocalEndPoint.ToString()));
                client.GetStream().Close();
                client.Close();
            }
        }

        public void CloseSingleConnection(string endpoint)
        {
            var client = TcpClients.Where(x => x.Client.RemoteEndPoint.ToString() == endpoint).FirstOrDefault();
            client.GetStream().Close();
            client.Close();
            TcpClients.Remove(client);
        }

        
    }
}
