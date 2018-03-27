using Chatv3.ClientServer;
using Chatv3.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Chatv3.Messages;
using static Chatv3.Enums;
using System.Net.Sockets;
using System.Net;
using Chatv3.Notification;
using System.Collections.Generic;

namespace Chatv3
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Client client;
        Server server;
        NotificationIcon notificationIcon;
        ServiceMode serviceMode;
        
        public MainWindow()
        {
            InitializeComponent();
            client = new Client();
            notificationIcon = new NotificationIcon();
        }
        
        private void Eingabe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            ValidateOutput();
            var message = new Message(Eingabe.Text, Settings.GetUserName());

            IMessageDispatcher messageDispatcher = null;

            if (serviceMode == ServiceMode.Client)
            {
                messageDispatcher = new ClientMessageDispatcher(client);
            }

            if (serviceMode == ServiceMode.Server)
            {
                messageDispatcher = new ServerMessageDispatcher(server);
                //eigene messages als server lokal im fenster anzeigen
                DisplayMessageToScreen(message);
            }

            messageDispatcher.Send(message);
            Eingabe.Text = string.Empty;
        }

        private void Dragbar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ipAddresses = Settings.GetIpAddresses();
            DisplayMessageToScreen("Suche Server im LAN...");
            serviceMode = await client.DetermineServiceMode(ipAddresses);
            IMessageReceiver messageReceiver;

            //wenn mode == client dann ist der netzwerkstream mit getstream() abrufbar
            if (serviceMode == ServiceMode.Client)
            {
                DisplayMessageToScreen("Server gefunden, du bist nun Client.");
                DisplayMessageToScreen("------------------------------------");
                messageReceiver = new ClientMessageReceiver(client);
                while (true)
                {
                    await Task.Delay(100);
                    var messageList = await messageReceiver.Receive();
                    if (messageList == null) continue;

                    foreach (var message in messageList)
                    {
                        ValidateInput(message);
                        DisplayMessageToScreen(message);
                        if (!this.IsActive)
                            notificationIcon.TurnOn();
                    }
                }
            }

            if (serviceMode == ServiceMode.Server)
            {
                DisplayMessageToScreen("Kein Server gefunden. Du bist jetzt Host.");
                DisplayMessageToScreen("-----------------------------------------");
                server = new Server();
                messageReceiver = new ServerMessageReceiver(server);
                IMessageDispatcher messageDispatcher = new ServerMessageDispatcher(server);
                server.StartListeningToClients();

                while (true)
                {
                    if (server.ConnectionsPending())
                    {
                        var newClient = await server.AcceptPendingConnection();
                        DisplayMessageToScreen($"{newClient.Client.RemoteEndPoint} ist dem Chat beigetreten");
                    }

                    await Task.Delay(100);
                    var messageList = (List<IMessage>)await messageReceiver.Receive();
                    if (messageList == null || messageList.Count == 0) continue;

                    foreach (var message in messageList)
                    {
                        //einkommende messages an alle clients schicken
                        ValidateInput(message);
                        messageDispatcher.Send(message);
                        DisplayMessageToScreen(message);
                        if (!this.IsActive)
                            notificationIcon.TurnOn();
                    }                    
                }
            }
        }

        private void DisplayMessageToScreen(IMessage message)
        {
            Ausgabe.AppendText($"{message.Sender} [{message.TimeStamp.ToShortTimeString()}]: {message.Content}\n");
            Ausgabe.ScrollToEnd();
        }

        private void DisplayMessageToScreen(string text)
        {
            Ausgabe.AppendText($"{text}\n");
            Ausgabe.ScrollToEnd();
        }
        private void ValidateOutput()
        {
            var input = Eingabe.Text;

            switch (input)
            {
                case "->exit":
                    if (serviceMode == ServiceMode.Client)
                        client.CloseConnection();
                    if (serviceMode == ServiceMode.Server)
                        server.CloseAllConnections();
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        private void ValidateInput(IMessage message)
        {
            switch (message.Content)
            {
                case "CONNECTION_CLOSE":
                    if (serviceMode == ServiceMode.Client)
                        client.CloseConnection();
                    if (serviceMode == ServiceMode.Server)
                        //bei CONNECTION_CLOSE message ist der sender der endpoint
                        server.CloseSingleConnection(message.Sender);
                    break;
                default:
                    break;
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            notificationIcon.TurnOff();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
        }
    }
}
