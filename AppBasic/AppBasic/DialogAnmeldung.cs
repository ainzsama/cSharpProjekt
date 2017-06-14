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

        public event EventHandler<OnSingnUpEventArgs> OnAnmeldungComplete;

        private Client client;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AnmeldungLayout, container, false);

            etIp = view.FindViewById<EditText>(Resource.Id.editTextIp);
            etName = view.FindViewById<EditText>(Resource.Id.editTextName_AnmDialog);
            etPw = view.FindViewById<EditText>(Resource.Id.editTextPW_AnmDialog);
            txtStatus = view.FindViewById<TextView>(Resource.Id.textViewStatus);

            //AnmeldeButton 
            btnAnm = view.FindViewById<Button>(Resource.Id.buttonLogIn);
            btnAnm.Enabled = false;
            btnAnm.Click += btnAnm_Click;
            
            //VerbindungsButton
            btnCon = view.FindViewById<Button>(Resource.Id.buttonVerbinden);
            btnCon.Click += BtnCon_Click;

            return view;
        }

        private void BtnCon_Click(object sender, EventArgs e)
        {
            client = new Client(etIp.Text, 10000);
            client.OnAnmeldung += Client_OnAnmeldung;
            client.OnMessageRecieved += Client_OnMessage;
            client.OnDatenComplete += Client_OnDatenComplete;
            client.OnSpielerErhalten += Client_OnSpielerErhalten;
            client.OnClientError += Client_OnClientError;
            client.connect();
        }

        private void Client_OnClientError(object sender, OnClientErrorEventArgs e)
        {
            txtStatus.Text = e.Message;
        }

        private void Client_OnSpielerErhalten(object sender, OnSpielerErhaltenEventArgs e)
        {
            txtStatus.Text = "Spieler erhalten";
            OnAnmeldungComplete.Invoke(this, new OnSingnUpEventArgs(e.Spieler.Name, e.Spieler));
        }

        private void Client_OnDatenComplete(object sender, OnDatenCompleteEventArgs e)
        {
            if (e.Complete)
            {
                txtStatus.Text = "Daten erhalten";
                btnAnm.Enabled = true;
            }
            else txtStatus.Text = "Fehler -> Daten nicht erhalten";//Fehler bei Dateien -> neu verbinden
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
                    GetData();
                else btnAnm.Enabled = true;
            }
            else txtStatus.Text = "Fehler";
        }

        private bool CheckData()
        {
            bool vorhanden = true;
            

            if (!File.Exists(Protokoll.PFADART) || !File.Exists(Protokoll.PFADANGR)) vorhanden = false;
          

            return vorhanden;
        }

        private void GetData()
        {
            client.ErfrageDaten();
        }

        private void btnAnm_Click(object sender, EventArgs e)
        {
            client.sendMessage(Protokoll.ANMELDUNG + Protokoll.TRENN + etName.Text + Protokoll.TRENN + etPw.Text);
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