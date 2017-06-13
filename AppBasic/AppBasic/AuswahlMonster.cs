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
    class AuswahlMonster
    {
        private Button button;
        private Monster monster;
        public AuswahlMonster(ActivityKampf a, Monster m, int i)
        {

            Button = new Button(a);

            Button.Text = m.Art.Name + " lvl: " + m.Lvl;

            //  Button.SetHeight(200);
            Button.Click += delegate { a.Wechsel(m); };
        }

        public Button Button { get => button; set => button = value; }
        internal Monster Monster { get => monster; set => monster = value; }
    }
}