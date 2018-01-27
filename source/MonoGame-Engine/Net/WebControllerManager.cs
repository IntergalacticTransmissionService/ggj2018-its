using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Fleck;
using System.Globalization;
using System.Net.Sockets;

namespace IntergalacticTransmissionService.Net
{
    class ClientConnection
    {
        public readonly IWebSocketConnection Connection;
        public float[] Axes = new float[2];
        public bool[] Buttons = new bool[4];
        public ClientConnection(IWebSocketConnection Connection)
        {
            this.Connection = Connection;
        }

        public void SetButtonsState(string state)
        {
            for (var i = 0; i < Buttons.Length; i++)
                Buttons[i] = state[i] == '1';
            // Console.WriteLine("Buttons: " + state);
        }

        public void SetAxes(float x, float y)
        {
            Axes[0] = x;
            Axes[1] = -y;//quickfix
            // Console.WriteLine("Direction: " + x + "/" + y);
        }
    }

    class WebControllerManager
    {
        private static readonly int HTTP_PORT = 8080;
        private static readonly int WS_PORT = 8082;
        private readonly WebServer WebServer;
        private readonly ClientConnection[] Clients = new ClientConnection[16];
        private readonly WebSocketServer WebSocketServer;

        public WebControllerManager()
        {
            string host = getLanIp();
            WebServer = new WebServer("http://" + host + ":" + HTTP_PORT + "/");
            WebServer.RegisterFile("/", "Content/WebController/index.html");
            string[] files = {
                "style.css",
                "script.js",
                "controller-body.png",
                "controller-button-a.png",
                "controller-button-b.png",
                "controller-button-x.png",
                "controller-button-y.png",
                "controller-pad.png",
                "controller-padstick.png",
                "fullscreen.png"
            };
            foreach(string file in files) {
                WebServer.RegisterFile("/" + file, "Content/WebController/" + file);
            }
            WebServer.Run();

            WebSocketServer = new WebSocketServer("ws://" + host + ":" + WS_PORT);
            WebSocketServer.Start(Connection =>
            {
                Connection.OnOpen = () => OnOpen(Connection);
                Connection.OnClose = () => OnClose(Connection);
                Connection.OnMessage = message => OnMessage(Connection, message);
            });
        }

        public static string getLanIp()
        {
            // would be better to choose, but this should work for the GGJ:
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    string value = ip.ToString();
                    if (value.StartsWith("192."))
                        return value;
                }
            }
            return "127.0.0.1";
        }

        public static string getLanIpWithPort()
        {
            return getLanIp() + ":" + HTTP_PORT;
        }

        public void SetRumble(int index, int ms)
        {
            lock (Clients)
            {
                if (Clients[index] != null)
                    Clients[index].Connection.Send("%|" + ms);
            }
        }

        public float getAxis(int index, int axis)
        {
            lock (Clients)
            {
                if (Clients[index] != null)
                    return Clients[index].Axes[axis];
            }
            return 0;
        }

        public bool getButton(int index, int button)
        {
            lock (Clients)
            {
                if (Clients[index] != null)
                    return Clients[index].Buttons[button];
            }
            return false;
        }

        private void OnOpen(IWebSocketConnection Connection)
        {
            lock (Clients)
            {
                for (var i = 0; i < Clients.Length; i++)
                {
                    if (Clients[i] == null)
                    {
                        Console.WriteLine("Open!");
                        Clients[i] = new ClientConnection(Connection);
                        return;
                    }
                }
                Console.WriteLine("Full!");
                Connection.Close();
            }
        }

        private void OnClose(IWebSocketConnection Connection)
        {
            Console.WriteLine("Close!");
            lock (Clients)
            {
                for (var i = 0; i < Clients.Length; i++)
                {
                    if (Clients[i].Connection == Connection)
                    {
                        Clients[i] = null;
                        break;
                    }
                }
            }
        }

        private void OnMessage(IWebSocketConnection Connection, string message)
        {
            lock (Clients)
            {
                for (var i = 0; i < Clients.Length; i++)
                {
                    if (Clients[i].Connection == Connection)
                    {
                        var Client = Clients[i];
                        if (Client == null)
                            continue;
                        string[] parts = message.Split('|');
                        if (parts[0] == "^")
                        {
                            Client.SetAxes(parseFloat(parts[1]), parseFloat(parts[2]));
                        }
                        else if (parts[0] == "!")
                        {
                            Client.SetButtonsState(parts[1]);
                        }
                        break;
                    }
                }
            }
        }

        public float parseFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        }

        public void Dispose()
        {
            WebServer.Dispose();
            WebSocketServer.Dispose();
        }
    }
}
