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
using System.Threading;

namespace AppBasic
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private string ip;
        private int port;
        private string pathArten;
        private string pathAngriffe;

        private Thread listenThread;
        private Thread sendThread;

        public event EventHandler<OnAnmeldungEventArgs> OnAnmeldung;
        public event EventHandler<OnMessageReceivedEventArgs> OnMessageRecieved;
        public event EventHandler<OnDatenCompleteEventArgs> OnDatenComplete;
        public event EventHandler<OnSpielerErhaltenEventArgs> OnSpielerErhalten;
        public event EventHandler<OnClientErrorEventArgs> OnClientError;
        public event EventHandler<OnRegistVersuchtEventArgs> OnRegistVersucht;
        public Client()
        {
            connect();
        }

        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            pathArten = Protokoll.GetPathArten();
            pathAngriffe = Protokoll.GetPathAngriffe();
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
                    listenThread = new Thread(new ThreadStart(receivedMessage));
                    listenThread.Start();

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
            sendThread = new Thread(SendMessageInThread);
            sendThread.Start(m);
            
        }

        private void SendMessageInThread(object input)
        {
            String m = (string)input;
            Console.WriteLine("Zu versendende Nachricht: " + m);
            byte[] message = Encoding.Unicode.GetBytes(m);
            stream.Write(message, 0, message.Length);
            Console.WriteLine("Nachricht verschickt: " + m);
            //receivedMessage();
        }
        private void receivedMessage()
        {
           
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string message = Encoding.Unicode.GetString(buffer, 0, data);

            //OnMessageRecieved.Invoke(this, new OnMessageReceivedEventArgs(message));
            CheckMessage(message);


            Console.WriteLine(message);

            
        }

        private void CheckMessage(string m)
        {
            string[] s = m.Split(new Char[] { Convert.ToChar(Protokoll.TRENN) });

            switch (s[0])
            {
                case Protokoll.SPIELER:
                    Console.WriteLine("Spieler erhalten");
                    HerstellenSpieler(s[1]);
                    break;
                case Protokoll.ERROR:
                    Console.WriteLine("Fehler aufgetreten" + s[1]);
                    //Fehler aufgetreten
                    break;
                case Protokoll.ARTEN:
                    ErstelleArtenDatei(s[1]);
                    break;
                case Protokoll.ANGRIFFE:
                    ErstelleAngriffeDatei(s[1]);
                    break;
                case Protokoll.REGISTRIERUNG:
                    if (s[1].Equals("erfolgrreich"))
                        OnRegistVersucht.Invoke(this, new OnRegistVersuchtEventArgs(true, null));
                    else OnRegistVersucht.Invoke(this, new OnRegistVersuchtEventArgs(false, s[2]));
                    break;
                default:
                    OnClientError.Invoke(this, new OnClientErrorEventArgs(m));
                    break;
            }
        }
        public void HerstellenSpieler(string m)
        {
            Console.WriteLine("HerstellenSpieler: " + m);
            Spieler s = JsonConvert.DeserializeObject<Spieler>(m);
            Console.WriteLine("Hergestelleter Spieler: " + s.Name);
            sendMessage(Protokoll.ABMELDUNG);
            client.Close();
            OnSpielerErhalten.Invoke(this, new OnSpielerErhaltenEventArgs(s));

        }
        public void ErfrageDaten()
        {
            Console.WriteLine("Erfrage Arten");
            sendMessage(Protokoll.ARTEN + Protokoll.TRENN);
            sendMessage(Protokoll.ANGRIFFE + Protokoll.TRENN);

            //OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());
        }
        private void ErstelleArtenDatei(string m)
        {
            
            List<Monsterart> arten = JsonConvert.DeserializeObject<List<Monsterart>>(m);
            FileStream fs = new FileStream(pathArten, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(typeof(List<Monsterart>));
            xml.Serialize(fs, arten);
            OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());

        }

        private void ErstelleAngriffeDatei(string m)
        {
            List<Angriff> angriffe = JsonConvert.DeserializeObject<List<Angriff>>(m);
            FileStream fs = new FileStream(pathAngriffe, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(typeof(List<Angriff>));
            xml.Serialize(fs, angriffe);
            Console.WriteLine("Angriffe erstellt");
            OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());

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
            if (File.Exists(Protokoll.GetPathArten()) && File.Exists(Protokoll.GetPathAngriffe())) Complete = true;
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

    public class OnRegistVersuchtEventArgs : EventArgs
    {
        bool erfolg;
        string fehler;

        public OnRegistVersuchtEventArgs(bool b, string s)
        {
            Erfolg = b;
            Fehler = s;
        }
        public bool Erfolg { get => erfolg; set => erfolg = value; }
        public string Fehler { get => fehler; set => fehler = value; }
    }
}