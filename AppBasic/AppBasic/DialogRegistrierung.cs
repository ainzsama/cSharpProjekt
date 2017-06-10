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

        public event EventHandler<OnSingnUpEventArgs> OnRegistrierungComplete;

        private Client client;

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
            btnReg = View.FindViewById<Button>(Resource.Id.buttonReg);
            btnReg.Enabled = false;
            btnReg.Click += BtnReg_Click;



            return view;
        }

        private void BtnReg_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnCon_Click(object sender, EventArgs e)
        {
            client = new Client(etIp.Text, 10000);
            client.OnAnmeldung += Client_OnAnmeldung;
            client.OnMessage += Client_OnMessage;
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
    }
}