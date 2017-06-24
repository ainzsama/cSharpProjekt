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
using System.Threading;

namespace AppBasic
{
    class DialogRegistrierung: DialogFragment
    {
        private EditText etIp;
        private EditText etName;
        private EditText etPw;
        private EditText etPwConf;
        private Button btnReg;
        private Button btnCon;
        private TextView txtStatus;

        public event EventHandler<OnRegistrierungEventArgs> OnRegistrierungComplete;

        private Client client;

        public Client Client { get => client; set => client = value; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.RegistrierungLayout, container, false);

            etIp = view.FindViewById<EditText>(Resource.Id.editTextIpReg);
            etName = view.FindViewById<EditText>(Resource.Id.editTextName_RegDialog);
            etPw = view.FindViewById<EditText>(Resource.Id.editTextPW_RegDialog);
            etPwConf = view.FindViewById<EditText>(Resource.Id.editTextPwConfirm_RegDialog);
            txtStatus = view.FindViewById<TextView>(Resource.Id.textViewStatusReg);

            //VerbindungsButton
            btnCon = view.FindViewById<Button>(Resource.Id.buttonVerbindenReg);
            btnCon.Click += BtnCon_Click;

            //RegistrierungsButton
            btnReg = view.FindViewById<Button>(Resource.Id.buttonReg);
            btnReg.Enabled = true;
            btnReg.Click += BtnReg_Click;


            
            Client =  Client.GetInstance(etIp.Text, 10000);
            Client.OnAnmeldung += Client_OnAnmeldung;
            Client.OnMessageRecieved += Client_OnMessage;
            Client.OnRegistVersucht += Client_OnRegistVersucht;
           
            return view;
        }

        private void BtnReg_Click(object sender, EventArgs e)
        {
            Client.sendMessage(Protokoll.REGISTRIERUNG + Protokoll.TRENN + etName.Text + Protokoll.TRENN + etPw.Text);
        }

        private void BtnCon_Click(object sender, EventArgs e)
        {
            //(etIp.Text, 10000);
            //client.OnAnmeldung += Client_OnAnmeldung;
            //client.OnMessageRecieved += Client_OnMessage;
            //client.OnRegistVersucht += Client_OnRegistVersucht;
        }

        private void Client_OnRegistVersucht(object sender, OnRegistVersuchtEventArgs e)
        {
            if (e.Erfolg)
            {
                //txtStatus.Text = "Registrierung erfolgreich, bitte Anmeldedialog starten";
                OnRegistrierungComplete.Invoke(this, new OnRegistrierungEventArgs(etName.Text, etPw.Text, Client));
            }
            else txtStatus.Text = "Fehler;  " + e.Fehler;
        }

        private void Client_OnMessage(object sender, OnMessageReceivedEventArgs e)
        {
            txtStatus.Text = e.Message;
        }


        private void Client_OnAnmeldung(object sender, OnAnmeldungEventArgs e)
        {
            if (e.Erfolg)
            {
                txtStatus.Text = "Verb erfolgreich";
                btnReg.Enabled = true;
            }
            else txtStatus.Text = "Fehler";
        }
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAnimation;
        }

    }

    public class OnRegistrierungEventArgs : EventArgs
    {
        private string name;
        private string pwd;


        public OnRegistrierungEventArgs(string name, string pwd, Client client) : base()
        {
            Name = name;
            Pwd = pwd;
        }

        public string Name { get => name; set => name = value; }
        public string Pwd { get => pwd; set => pwd = value; }
    }
}