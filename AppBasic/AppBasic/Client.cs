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
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AppBasic
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private string ip;
        private int port;

        public event EventHandler<OnAnmeldungEventArgs> OnAnmeldung;
        public event EventHandler<OnMessageReceivedEventArgs> OnMessageRecieved;
        public event EventHandler<OnDatenCompleteEventArgs> OnDatenComplete;
        public event EventHandler<OnSpielerErhaltenEventArgs> OnSpielerErhalten;
        public event EventHandler<OnClientErrorEventArgs> OnClientError;
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

            OnMessageRecieved.Invoke(this, new OnMessageReceivedEventArgs(message));
            CheckMessage(message);


            //Console.WriteLine(message);

        }

        private void CheckMessage(string m)
        {
            string[] s = m.Split(Protokoll.TRENN);

            switch (s[0])
            {
                case Protokoll.SPIELER:
                    HerstellenSpieler(s[1]);
                    break;
                case Protokoll.ERROR:
                    //Fehler aufgetreten
                    break;
                case Protokoll.ARTEN:
                    ErstelleArtenDatei(s[1]);
                    break;
                case Protokoll.ANGRIFFE:
                    ErstelleAngriffeDatei(s[1]);
                    break;
                default:
                    OnClientError.Invoke(this, new OnClientErrorEventArgs(m));
                    break;
            }
        }
        public void HerstellenSpieler(string m)
        {
            Spieler s = JsonConvert.DeserializeObject<Spieler>(m);
            OnSpielerErhalten.Invoke(this, new OnSpielerErhaltenEventArgs(s));

        }
        public void ErfrageDaten()
        {
            sendMessage(Protokoll.ARTEN);
            sendMessage(Protokoll.ANGRIFFE);

            OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());
        }
        private void ErstelleArtenDatei(string m)
        {
            List<Monsterart> arten = JsonConvert.DeserializeObject<List<Monsterart>>(m);
            FileStream fs = new FileStream(Protokoll.PFADART, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(typeof(List<Monsterart>));
            xml.Serialize(fs, arten);
        }

        private void ErstelleAngriffeDatei(string m)
        {
            List<Angriff> angriffe = JsonConvert.DeserializeObject<List<Angriff>>(m);
            FileStream fs = new FileStream(Protokoll.PFADANGR, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(typeof(List<Angriff>));
            xml.Serialize(fs, angriffe);
        }
    }

    public class OnSpielerErhaltenEventArgs : EventArgs
    {
        private Spieler spieler;

        public OnSpielerErhaltenEventArgs(Spieler s) : base()
        {
            Spieler = s;
        }

        public Spieler Spieler { get => spieler; set => spieler = value; }
    }
    public class OnDatenCompleteEventArgs : EventArgs
    {
        private bool complete;

        public OnDatenCompleteEventArgs() : base()
        {
            if (File.Exists(Protokoll.PFADART) && File.Exists(Protokoll.PFADANGR)) Complete = true;
            else Complete = false;
        }
        public bool Complete { get => complete; set => complete = value; }
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

    public class OnClientErrorEventArgs : EventArgs
    {
        private string message;

        public OnClientErrorEventArgs(string m) : base()
        {
            Message = m;
        }
        public string Message { get => message; set => message = value; }
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