using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Newtonsoft.Json;

namespace AppBasic
{
    [Activity(Label = "AppBasic", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText etName;
        EditText etPw;
        Button btnLogin;
        Button btnRegist;
        Button btnDialog;
        Spieler spieler;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            etName = (EditText)FindViewById(Resource.Id.editTextName);
            etPw = (EditText)FindViewById(Resource.Id.editTextPassword);
            btnLogin = (Button)FindViewById(Resource.Id.buttonLogin);
            btnRegist = (Button)FindViewById(Resource.Id.buttonRegist);
            btnLogin.Click += OnBtnLoginClick;
            btnRegist.Click += OnBtnRegistClick;
            btnDialog = (Button)FindViewById(Resource.Id.buttonLogDia);


            btnDialog.Click += StartLogIn;
            //Testspieler einfügen 
            spieler = Spieler.GetTestSpieler();
            spieler.Monster.Add(Monster.GetTestMonster());
        }

        private void StartLogIn(object sender, EventArgs e)
        {
            FragmentTransaction trans = FragmentManager.BeginTransaction();
            DialogAnmeldung anm = new DialogAnmeldung();
            anm.Show(trans, "AnmeldeDialog");
            anm.OnAnmeldungComplete += Anm_OnAnmeldungComplete;
        }

        private void Anm_OnAnmeldungComplete(object sender, OnSingnUpEventArgs e)
        {
            etName.Text = e.Name;
            etPw.Text = e.Pw;
        }

        private void OnBtnRegistClick(object sender, EventArgs e)
        {
            
        }

        private void OnBtnLoginClick(object sender, EventArgs e)
        {
            //Starte nächste Activity

            Intent actMap = new Intent(this, typeof(ActivityMap));
            
            //Übergabe Spieler
            actMap.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            StartActivity(actMap);
        }

        
    }
}

