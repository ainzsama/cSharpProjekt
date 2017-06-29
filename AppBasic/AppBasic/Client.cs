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
using Android.Util;

namespace AppBasic
{
    public class Client
    {
        private static Client instance;
        private TcpClient client;
        private NetworkStream stream;
        private string ip;
        private int port;
        private string pathArten;
        private string pathAngriffe;
        private bool angriffeComplete;
        private bool artenComplete;
        private Thread listenThread;

        public event EventHandler<OnAnmeldungEventArgs> OnAnmeldung;
        public event EventHandler<OnMessageReceivedEventArgs> OnMessageRecieved;
        public event EventHandler<OnDatenCompleteEventArgs> OnDatenComplete;
        public event EventHandler<OnSpielerErhaltenEventArgs> OnSpielerErhalten;
        public event EventHandler<OnClientErrorEventArgs> OnClientError;
        public event EventHandler<OnRegistVersuchtEventArgs> OnRegistVersucht;
        public event EventHandler<EventArgs> OnAbmeldungComplete;
        
        public Client()
        {
            connect();
        }

        public static Client GetInstance(string ip, int port)
        {
            if (instance == null) instance = new Client(ip, port);
            return instance;
        }
        public Client(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            angriffeComplete = false;
            artenComplete = false;
            pathArten = Protokoll.GetPathArten();
            pathAngriffe = Protokoll.GetPathAngriffe();
            connect();
        }

        public bool IsConnected()
        {
            return client.Connected;
        }
        public void connect()
        {
            client = new TcpClient();
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(ip), 10000);
            try
            {
                client.Connect(ipEnd);
                if (client.Connected)
                {
                    stream = client.GetStream();
                    listenThread = new Thread(new ThreadStart(receivedMessage));
                    listenThread.Start();
                    OnAnmeldung.Invoke(this, new OnAnmeldungEventArgs(true));

                }
                else
                {
                    OnAnmeldung.Invoke(this, new OnAnmeldungEventArgs(false));

                }
            }
            catch (Exception ex)
            {
               

            }

        }
        public void sendMessage(string m)
        {
            //sendThread = new Thread(SendMessageInThread);
            //sendThread.Start(m);
            SendMessageInThread(m);
            
        }

        private void SendMessageInThread(object input)
        {
            String m = (string)input;
            Log.Debug("SendMessageInThread", "Zu versendende Nachricht: " + m);
            byte[] message = Encoding.Unicode.GetBytes(m);
            stream.Write(message, 0, message.Length);
            Log.Debug("SendMessageInThread", "Nachricht verschickt: " + m);
            new Thread(receivedMessage).Start();
        }
        private void receivedMessage()
        {
            Log.Debug("ReceivedMessage", "Nachricht erhalten");
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int data = stream.Read(buffer, 0, client.ReceiveBufferSize);
            string message = Encoding.Unicode.GetString(buffer, 0, data);
            Log.Debug("ReceivedMessage", "Nachricht erhalten: " + message);
            CheckMessage(message);
        }

        private void CheckMessage(string m)
        {
            Log.Debug("Debug", "Nachricht erhalten: " + m);
            string[] s = m.Split(new Char[] { Convert.ToChar(Protokoll.TRENN) });

            switch (s[0])
            {
                case Protokoll.SPIELER:
                    Log.Debug("CheckMessage", "Spieler erhalten...");
                    EmpfangeSpieler(s[1]);
                    break;
                case Protokoll.ERROR:
                    Log.Debug("CheckMessage", "Error erhalten...");
                    //Fehler aufgetreten
                    break;
                case Protokoll.ARTEN:
                    Log.Debug("CheckMessage", "Arten erhalten...");
                    ErstelleArtenDatei(s[1]);
                    break;
                case Protokoll.ANGRIFFE:
                    Log.Debug("CheckMessage", "Angriffe erhalten...");
                    ErstelleAngriffeDatei(s[1]);
                    break;
                case Protokoll.REGISTRIERUNG:
                    if (s[1].Equals("erfolgreich"))
                        OnRegistVersucht.Invoke(this, new OnRegistVersuchtEventArgs(true, null));
                    else OnRegistVersucht.Invoke(this, new OnRegistVersuchtEventArgs(false, s[2]));
                    break;
                case Protokoll.DATEN:
                    Log.Debug("CheckMessage", "Daten erhalten...");
                    ErstelleArtenDatei(s[1]);
                    ErstelleAngriffeDatei(s[2]);
                    Log.Debug("CheckMessage", "Dateien erstellt...");
                    break;
                case Protokoll.ANMELDUNG:
                    Log.Debug("CheckMessage", "Anmeldung erfolgt, warte auf Spieler...");
                    break;
                case Protokoll.ABMELDUNG:
                    Log.Debug("CheckMessage", "Abmeldung erfolgt...");
                    OnAbmeldungComplete.Invoke(this, new EventArgs());
                    
                    break;
                default:
                    OnClientError.Invoke(this, new OnClientErrorEventArgs(m));
                    break;
            }
        }
        public void EmpfangeSpieler(string m)
        {
            Log.Debug("EmpfangeSpieler", "Spieler empfangen...");
            Spieler s = HerstellenSpieler(JsonConvert.DeserializeObject<SpielerUebertragung>(m));
            
            OnSpielerErhalten.Invoke(this, new OnSpielerErhaltenEventArgs(s));

        }

        private Spieler HerstellenSpieler(SpielerUebertragung su)
        {
            Log.Debug("HerstellenSpieler", "Spieler herstellen...");
            Spieler s = new Spieler();

            s.Logdaten = su.Logdaten;
            s.Name = su.Name;
            s.SpielerId = su.SpielerId;
            s.Monster = HerstellenMonster(su.Monster);

            return s;
        }

        private List<Monster> HerstellenMonster(List<MonsterUebertragung> mul)
        {
            Log.Debug("HerstellenMonster", "Monster herstellen...");
            List<Monster> ml = new List<Monster>();

            foreach(MonsterUebertragung mu in mul)
            {
                Monster m = new Monster();
                m.Angriff = mu.Angriff;
                m.Art = mu.Art;
                m.BenoetigteXp = mu.BenoetigteXp;
                m.Hp = mu.Hp;
                m.Lvl = mu.Lvl;
                m.Maxhp = mu.Maxhp;
                m.MonsterId = mu.MonsterId;
                m.Nickname = mu.Nickname;
                m.Xp = mu.Xp;

                ml.Add(m);
            }
            return ml;
        }
        public void ErfrageDaten()
        {
            sendMessage(Protokoll.DATEN + Protokoll.TRENN);

            //OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());
        }
        private void ErstelleArtenDatei(string m)
        {
            
            List<Monsterart> arten = JsonConvert.DeserializeObject<List<Monsterart>>(m);
            FileStream fs = new FileStream(pathArten, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(typeof(List<Monsterart>));
            xml.Serialize(fs, arten);
            if (angriffeComplete) OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());

        }

        private void ErstelleAngriffeDatei(string m)
        {
            List<Angriff> angriffe = JsonConvert.DeserializeObject<List<Angriff>>(m);
            FileStream fs = new FileStream(pathAngriffe, FileMode.Create);
            XmlSerializer xml = new XmlSerializer(typeof(List<Angriff>));
            xml.Serialize(fs, angriffe);
            angriffeComplete = true;
            if(artenComplete) OnDatenComplete.Invoke(this, new OnDatenCompleteEventArgs());

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