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
    [Activity(Label = "ActivityMap")]
    public class ActivityMap : Activity
    {
        TextView tvUebergeben;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Map);
            tvUebergeben = (TextView)FindViewById(Resource.Id.textViewFromMain);
            String text = Intent.GetStringExtra("MyData") ?? "Data not available";
            tvUebergeben.Text = text;
        }
    }
}