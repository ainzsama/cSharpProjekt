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
using System.IO;
using System.Threading;

namespace AppBasic
{
    class DialogAnmeldung : DialogFragment
    {
        private EditText etIp;
        private EditText etName;
        private EditText etPw;
        private Button btnAnm;
        private Button btnCon;
        private TextView txtStatus;
        private string name;
        private string pwd;
        public event EventHandler<OnSingnUpEventArgs> OnAnmeldungComplete;

        private Client client;

        public string Name { get => name; set => name = value; }
        public string Pwd { get => pwd; set => pwd = value; }
        public Client Client { get => client; set => client = value; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AnmeldungLayout, container, false);

            etIp = view.FindViewById<EditText>(Resource.Id.editTextIp);
            etIp.Text = "192.168.178.97";
            etName = view.FindViewById<EditText>(Resource.Id.editTextName_AnmDialog);
            if (name != null) etName.Text = name;
            else etName.Text = "tester";

            etPw = view.FindViewById<EditText>(Resource.Id.editTextPW_AnmDialog);

            if (pwd != null) etPw.Text = pwd;
            else etPw.Text = "Test";

            txtStatus = view.FindViewById<TextView>(Resource.Id.textViewStatus);

            //AnmeldeButton 
            btnAnm = view.FindViewById<Button>(Resource.Id.buttonLogIn);
            btnAnm.Enabled = false;
            btnAnm.Click += btnAnm_Click;

            //VerbindungsButton
            //btnCon = view.FindViewById<Button>(Resource.Id.buttonVerbinden);
            //btnCon.Click += BtnCon_Click;
           
            Client = Client.GetInstance(etIp.Text, 10000);
            Client.OnAnmeldung += Client_OnAnmeldung;
            Client.OnMessageRecieved += Client_OnMessage;
            Client.OnDatenComplete += Client_OnDatenComplete;
            Client.OnSpielerErhalten += Client_OnSpielerErhalten;
            Client.OnClientError += Client_OnClientError;
            //Client.connect();
            if (Client.IsConnected()) Client_OnAnmeldung(this, new OnAnmeldungEventArgs(true));
          
            return view;
        }

        private void BtnCon_Click(object sender, EventArgs e)
        {
            //Client = new Client(etIp.Text, 10000);
            //Client.OnAnmeldung += Client_OnAnmeldung;
            //Client.OnMessageRecieved += Client_OnMessage;
            //Client.OnDatenComplete += Client_OnDatenComplete;
            //Client.OnSpielerErhalten += Client_OnSpielerErhalten;
            //Client.OnClientError += Client_OnClientError;
            //Client.connect();
        }

        private void Client_OnClientError(object sender, OnClientErrorEventArgs e)
        {
            ChangeStatusText(e.Message);
        }

        private void Client_OnSpielerErhalten(object sender, OnSpielerErhaltenEventArgs e)
        {
           ChangeStatusText("Spieler erhalten");
            
           OnAnmeldungComplete.Invoke(this, new OnSingnUpEventArgs(e.Spieler.Name, e.Spieler));
        }

        private void Client_OnDatenComplete(object sender, OnDatenCompleteEventArgs e)
        {
            if (e.Complete)
            {
                ChangeStatusText("Daten erhalten");
                ChangeStateBtnAnm(true);
            }
            else ChangeStatusText("Fehler -> Daten nicht erhalten"); //Fehler bei Dateien -> neu verbinden
        }

        private  void ChangeStatusText(string t)
        {
            Activity.RunOnUiThread(() => { txtStatus.Text = t; });
        }

        private void ChangeStateBtnAnm(bool b)
        {
            Activity.RunOnUiThread(() => { btnAnm.Enabled = true; });
        }
        private void Client_OnMessage(object sender, OnMessageReceivedEventArgs e)
        {
            txtStatus.Text = e.Message;
        }

        private void Client_OnAnmeldung(object sender, OnAnmeldungEventArgs e)
        {
            if (e.Erfolg)
            {
                txtStatus.Text = "erfolgreich";
                if (!CheckData())
                {
                    txtStatus.Text = "PruefeDaten";
                    DeleteData();
                }
                GetData();
                btnAnm.Enabled = true;
            }
            else txtStatus.Text = "Fehler";
        }

        private void DeleteData()
        {
            File.Delete(Protokoll.GetPathArten());
            File.Delete(Protokoll.GetPathAngriffe());
        }
        private bool CheckData()
        {
            bool vorhanden = true;
            

            if (!File.Exists(Protokoll.GetPathArten()) || !File.Exists(Protokoll.GetPathAngriffe())) vorhanden = false;
          

            return vorhanden;
        }

        private void GetData()
        {
            Client.ErfrageDaten();
        }

        private void btnAnm_Click(object sender, EventArgs e)
        {
            Client.sendMessage(Protokoll.ANMELDUNG + Protokoll.TRENN + etName.Text + Protokoll.TRENN + etPw.Text);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAnimation;
        }
    }

    public class OnSingnUpEventArgs : EventArgs
    {
        private string name;
     
        private Spieler spieler;

        public string Name { get => name; set => name = value; }
       
        public Spieler Spieler { get => spieler; set => spieler = value; }

        public OnSingnUpEventArgs(string name, Spieler s) : base()
        {
            Name = name;
         
            Spieler = s;
        }
    }
}