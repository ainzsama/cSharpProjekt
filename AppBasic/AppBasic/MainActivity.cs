using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppBasic
{
    [Activity(Label = "AppBasic", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button btnLogin;
        Button btnRegist;
        Button btnDialog;
        Spieler spieler;
        List<Monsterart> monsterarten;
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
            //Testspieler einfügen 
            spieler = Spieler.GetTestSpieler();
            spieler.Monster.Add(Monster.GetTestMonster());
            EinlesenMonsterarten(); //Später in Anm_OnAnmeldungComplete
        }

        private void StartLogIn(object sender, EventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogAnmeldung anm = new DialogAnmeldung();
            anm.Show(trans, "AnmeldeDialog");
            anm.OnAnmeldungComplete += Anm_OnAnmeldungComplete;
        }

        private void EinlesenMonsterarten()
        {
            monsterarten = new List<Monsterart>();

            //XML Datei suchen -> aus datei lesen oder von Server anfordern und XML erstellen
            for (int i = 0; i < 20; i++)
            {
                Monsterart m = Monsterart.GetTestMonsterart();
                m.Name += i.ToString();
                monsterarten.Add(m);
            }

            
        }
        private void Anm_OnAnmeldungComplete(object sender, OnSingnUpEventArgs e)
        {
           
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
            actMap.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            actMap.PutExtra("monsterarten", JsonConvert.SerializeObject(monsterarten));
            StartActivity(actMap);
        }

        
    }
}

