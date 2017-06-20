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


        private Spieler spieler;
        private List<Monsterart> monsterarten;

        internal Spieler Spieler { get => spieler; set => spieler = value; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

           
            btnLogin = (Button)FindViewById(Resource.Id.buttonLogin);
            btnRegist = (Button)FindViewById(Resource.Id.buttonRegist);
            btnLogin.Click += OnBtnLoginClick;
            btnRegist.Click += OnBtnRegistClick;
            btnDialog = (Button)FindViewById(Resource.Id.buttonLogDia);



            btnDialog.Click += StartLogIn;
            btnTestKampf = FindViewById<Button>(Resource.Id.buttonStarteTestKampf);
            btnTestKampf.Click += BtnTestKampf_Click;
            //Testspieler einfügen 
            Spieler = Spieler.GetTestSpieler();
            Spieler.Monster.Add(Monster.GetTestMonster());
            Spieler.Monster.Add(Monster.GetTestMonster());
           
        }

       
        private void BtnTestKampf_Click(object sender, EventArgs e)
        {
            Intent actTestKampf = new Intent(this, typeof(ActivityKampf));

            //Übergabe Spieler
            actTestKampf.PutExtra("spieler", JsonConvert.SerializeObject(Spieler));
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
            reg.OnRegistrierungComplete += Anm_OnAnmeldungComplete;
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


    }
}

