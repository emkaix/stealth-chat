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
using static Chatv3.Enums;

namespace Chatv3.ClientServer
{
    public class Client
    {
        private TcpClient tcpClient;
        private const int port = 5000;
        

        public Client()
        {
            tcpClient = new TcpClient();
        }

        public async Task<ServiceMode> DetermineServiceMode(IEnumerable<IPAddress> ipAddresses)
        {

            foreach (var ipAddress in ipAddresses)
            {
                try
                {
                    await tcpClient.ConnectAsync(ipAddress, port);
                    Console.WriteLine("Server gefunden und verbunden");
                    return ServiceMode.Client;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Kein Server unter {0}\n{1}", ipAddress.ToString(), ex.Message);
                }
            }

            return ServiceMode.Server;
        }

        public NetworkStream GetStream()
        {
            if (!tcpClient.Connected) return null;
            return tcpClient.GetStream();
        }

        public string GetRemoteEndPoint()
        {
            return tcpClient.Client.RemoteEndPoint.ToString();
        }

        public string GetLocalEndPoint()
        {
            return tcpClient.Client.LocalEndPoint.ToString();
        }

        public int GetReceiveBufferSize()
        {
            return tcpClient.ReceiveBufferSize;
        }

        public void CloseConnection()
        {
            var messageDispatcher = new ClientMessageDispatcher(this);
            messageDispatcher.Send(new Message("CONNECTION_CLOSE", GetLocalEndPoint()));
            tcpClient.GetStream().Close();
            tcpClient.Close();
        }

        
        

        
    }
}
