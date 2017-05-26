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
    [Activity(Label = "ActivityMap")]
    public class ActivityMap : Activity
    {
        TextView tvUebergeben;

        Spieler spieler;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Map);
            tvUebergeben = (TextView)FindViewById(Resource.Id.textViewFromMain);

            //Entgegennehmen Spieler
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));

            tvUebergeben.Text = spieler.Name;
        }


        private void starteKampf()
        {
            Intent actKampf = new Intent(this, typeof(ActivityKampf));
            //Übergabe Spieler
            actKampf.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            //Übergabe Gegner
            actKampf.PutExtra("gegner", JsonConvert.SerializeObject(Monster.getTestMonster()));
            StartActivity(actKampf);
        }
    }



}