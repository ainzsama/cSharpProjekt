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
using Newtonsoft.Json;

namespace AppBasic
{
    [Activity(Label = "Activity1")]
    public class ActivityKampf : Activity
    {
        private Spieler spieler;
        private Monster gegner;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Kampf);

            //Entgegennehmen Spieler und Gegner/Monster
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
            gegner = JsonConvert.DeserializeObject<Monster>(Intent.GetStringExtra("gegner"));
            // Create your application here
        }
    }
}