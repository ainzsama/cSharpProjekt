using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;

namespace AppBasic
{
    [Activity(Label = "AppBasic", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText etName;
        EditText stPw;
        Button btnLogin;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            etName = (EditText)FindViewById(Resource.Id.editTextName);
            stPw = (EditText)FindViewById(Resource.Id.editTextPassword);
            btnLogin = (Button)FindViewById(Resource.Id.buttonLogin);
            btnLogin.Click += OnBtnLoginClick;
        }

        private void OnBtnLoginClick(object sender, EventArgs e)
        {
            //Starte nächste Activity
            Intent actMap = new Intent(this, typeof(ActivityMap));
            actMap.PutExtra("MyData", "Data from MainActivity");
            StartActivity(actMap);

        }
    }
}

