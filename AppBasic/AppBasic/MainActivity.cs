using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AppBasic
{
    [Activity(Label = "AppBasic", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button btnLogin;
        private Button btnRegist;
        private Button btnDialog;
        private Button btnTestKampf;
        private Button btnLogout;

        private Spieler spieler;
        private bool loggedIn;

        internal Spieler Spieler { get => spieler; set => spieler = value; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            loggedIn = false;
            btnLogin = (Button)FindViewById(Resource.Id.buttonLogin);
            btnRegist = (Button)FindViewById(Resource.Id.buttonRegist);
            btnLogin.Click += OnBtnLoginClick;
            btnRegist.Click += OnBtnRegistClick;
            btnDialog = (Button)FindViewById(Resource.Id.buttonLogDia);
            btnLogout = (Button)FindViewById(Resource.Id.buttonAbmeldenMain);
            btnLogout.Click += BtnLogout_Click;


            btnDialog.Click += StartLogIn;
            btnTestKampf = FindViewById<Button>(Resource.Id.buttonStarteTestKampf);
            btnTestKampf.Click += BtnTestKampf_Click;
            //Testspieler einfügen 
            Spieler = Spieler.GetTestSpieler();
            Spieler.Monster.Add(Monster.GetTestMonster());
            Spieler.Monster.Add(Monster.GetTestMonster());
           
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (loggedIn)
            {
                Client c = Client.GetInstance(Protokoll.IP, 10000);
                if (!c.IsConnected())
                    c.connect();
                c.OnAbmeldungComplete += (object s, EventArgs ea) => {
                    loggedIn = false;
                    spieler = null;
                };

                c.sendMessage(Protokoll.SPIELER + JsonConvert.SerializeObject(spieler));
               
            }
        }

        private void BtnTestKampf_Click(object sender, EventArgs e)
        {
            Intent actTestKampf = new Intent(this, typeof(ActivityKampf));

            //Übergabe Spieler
            actTestKampf.PutExtra("spieler", JsonConvert.SerializeObject(CreateTransferPlayer(Spieler)));
            actTestKampf.PutExtra("gegner", JsonConvert.SerializeObject(Monster.GetTestMonster()));
            StartActivity(actTestKampf);
        }

        private void StartLogIn(object sender, EventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogAnmeldung anm = new DialogAnmeldung();
            anm.OnAnmeldungComplete += Anm_OnAnmeldungComplete;
            anm.Show(trans, "AnmeldeDialog");
        }

        private void Anm_OnAnmeldungComplete(object sender, OnSingnUpEventArgs e)
        {
            loggedIn = true;
            Intent actMap = new Intent(this, typeof(ActivityMap));

            //Übergabe Spieler
            actMap.PutExtra("spieler", JsonConvert.SerializeObject(e.Spieler));
            //actMap.PutExtra("monsterarten", JsonConvert.SerializeObject(monsterarten));
            StartActivity(actMap);
        }

       
      

        private void OnBtnRegistClick(object sender, EventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            
            DialogRegistrierung reg = new DialogRegistrierung();
            reg.Show(trans, "Registrierungsdialog");
            reg.OnRegistrierungComplete += Reg_OnRegistrierungComplete; ;
        }

        private void Reg_OnRegistrierungComplete(object sender, OnRegistrierungEventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogAnmeldung anm = new DialogAnmeldung();
            anm.OnAnmeldungComplete += Anm_OnAnmeldungComplete;
            anm.Name = e.Name;
            anm.Pwd = e.Pwd;
            anm.Show(trans, "AnmeldeDialog");
        }

        private void OnBtnLoginClick(object sender, EventArgs e)
        {
            //Starte nächste Activity

            Intent actMap = new Intent(this, typeof(ActivityMap));

            //Übergabe Spieler
            actMap.PutExtra("spieler", JsonConvert.SerializeObject(Spieler));
            //actMap.PutExtra("monsterarten", JsonConvert.SerializeObject(monsterarten));
            StartActivity(actMap);
        }

        private SpielerUebertragung CreateTransferPlayer(Spieler s)
        {
            SpielerUebertragung su = new SpielerUebertragung();

            su.Logdaten = s.Logdaten;
            su.Name = s.Name;
            su.SpielerId = s.SpielerId;
            foreach (Monster m in s.Monster) su.Monster.Add(CreateTransferMonster(m));
            return su;
        }

        private MonsterUebertragung CreateTransferMonster(Monster m)
        {
            MonsterUebertragung mu = new MonsterUebertragung();

            mu.Nickname = m.Nickname;
            mu.MonsterId = m.MonsterId;
            mu.Maxhp = m.Maxhp;
            mu.Lvl = m.Lvl;
            mu.Angriff = m.Angriff;
            mu.Art = m.Art;
            mu.BenoetigteXp = m.BenoetigteXp;
            mu.Xp = m.Xp;

            return mu;
        }
    }
}

