using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using IntergalacticTransmissionService.HttpWebServer;
using Fleck;
using System.Globalization;
using System.Net.Sockets;

namespace IntergalacticTransmissionService.WebController
{
    class ClientConnection
    {
        public readonly IWebSocketConnection Connection;
        public readonly List<string> Messages = new List<string>();
        public ClientConnection(IWebSocketConnection Connection)
        {
            this.Connection = Connection;
        }

        public void AddMessage(string message)
        {
            lock(Messages) {
                Messages.Add(message);
            }
        }
    }

    class ServerManager
    {
        private bool Running = true;
        private readonly WebServer WebServer;
        private readonly List<ClientConnection> Connections = new List<ClientConnection>();
        private readonly WebSocketServer WebSocketServer;

        public ServerManager()
        {
            string host = getLanIp();
            WebSocketServer = new WebSocketServer("ws://" + host + ":8082");
            WebServer = new WebServer("http://"  + host + ":8080/");
            WebServer.RegisterFile("/", "Content/WebController/index.html");
            WebServer.RegisterFile("/style.css", "Content/WebController/style.css");
            WebServer.RegisterFile("/script.js", "Content/WebController/script.js");
            WebServer.Run();
            WebSocketServer.Start(WebSocketAction);
        }

        private static string getLanIp() {
            // would be better to choose, but this should work for the GGJ:
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string value = ip.ToString();
                    if(value.StartsWith("192."))
                        return value;
                }
            }
            return "127.0.0.1";
        }

        private void WebSocketAction(IWebSocketConnection Connection)
        {
            Connection.OnOpen = () =>
            {
                Console.WriteLine("Open!");
                lock(Connections) {
                    Connections.Add(new ClientConnection(Connection));
                }
            };
            Connection.OnClose = () =>
            {
                Console.WriteLine("Close!");
                lock(Connections) {
                    Connections.RemoveAll(c => c == Connection);
                }
            };
            Connection.OnMessage = message =>
            {
                string[] parts = message.Split('|');
                if(parts[0] == "^") {
                    Console.WriteLine("Direction: " + parseFloat(parts[1]) + "/" + parseFloat(parts[2]));
                } else if(parts[0] == "!") {
                    Console.WriteLine("Button: " + int.Parse(parts[1]));
                }
            };
        }

        public float parseFloat(string value) {
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        public void Stop()
        {
            WebServer.Stop();
        }
    }
}
