﻿using Android.App;
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
        EditText stPw;
        Button btnLogin;
        Button btnRegist;

        Spieler spieler;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            etName = (EditText)FindViewById(Resource.Id.editTextName);
            stPw = (EditText)FindViewById(Resource.Id.editTextPassword);
            btnLogin = (Button)FindViewById(Resource.Id.buttonLogin);
            btnRegist = (Button)FindViewById(Resource.Id.buttonRegist);
            btnLogin.Click += OnBtnLoginClick;
            btnRegist.Click += OnBtnRegistClick;
            //Testspieler einfügen 
            spieler = Spieler.GetTestSpieler();
            spieler.Monster.Add(Monster.GetTestMonster());
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

