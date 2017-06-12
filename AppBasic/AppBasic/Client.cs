
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Sockets;
using System.Net;

namespace AppBasic
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private string ip;
        private int port;

        public event EventHandler<OnAnmeldungEventArgs> OnAnmeldung;
        public event EventHandler<OnMessageReceivedEventArgs> OnMessage;
        public Client()
        {
            connect();
        }

        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void connect()
        {
            //Console.WriteLine("[Try to connect to server...]");
            client = new TcpClient();
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), 10000);
            try
            {
                client.Connect(ipEnd);
                if (client.Connected)
                {
                    //Console.WriteLine("[Connected]");
                    OnAnmeldung.Invoke(this, new OnAnmeldungEventArgs(true));
                    stream = client.GetStream();

                }
                else
                {
                    //Console.WriteLine("[Failed connection]");
                    OnAnmeldung.Invoke(this, new OnAnmeldungEventArgs(false));

                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("[connectioin error: " + ex.Message.ToString() + "]");

            }

        }
        public void sendMessage(string m)
        {
            byte[] message = Encoding.Unicode.GetBytes(m);
            stream.Write(message, 0, message.Length);
            receivedMessage();
        }

        private void receivedMessage()
        {
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string message = Encoding.Unicode.GetString(buffer, 0, data);

            OnMessage.Invoke(this, new OnMessageReceivedEventArgs(message));
            //Console.WriteLine(message);

        }
    }

    public class OnAnmeldungEventArgs : EventArgs
    {
        private bool erfolg;

        public OnAnmeldungEventArgs(bool b) : base()
        {
            Erfolg = b;
        }

        public bool Erfolg { get => erfolg; set => erfolg = value; }
    }

    public class OnMessageReceivedEventArgs : EventArgs
    {
        private string message;

        public OnMessageReceivedEventArgs(string m) : base()
        {
            Message = m;
        }

        public string Message { get => message; set => message = value; }
    }
}