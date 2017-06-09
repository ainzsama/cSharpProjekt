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
using Android.Graphics;
using Newtonsoft.Json;

namespace AppBasic
{
    [Activity(Label = "Activity1")]
    public class ActivityKampf : Activity
    {
        private Spieler spieler;
        private Monster gegner;
        private ImageView spielerBild;

        internal void Wechseln(Monster m)
        {
            throw new NotImplementedException();
        }

        private ImageView gegnerBild;
        private TextView gegnerLeben;
        private TextView spielerLeben;
        private Button angriff;
        private Monster ausgewaehltesMonster;
        private LinearLayout monsteranzeige;
        private AuswahlMonster[] monsterauswahl;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Kampf);

            //Entgegennehmen Spieler und Gegner/Monster
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
            gegner = JsonConvert.DeserializeObject<Monster>(Intent.GetStringExtra("gegner"));
            // Create your application here
            gegnerLeben = FindViewById<TextView>(Resource.Id.textViewLebenGegner);
            gegnerBild = FindViewById<ImageView>(Resource.Id.ImageViewGegner);
            spielerBild = FindViewById<ImageView>(Resource.Id.ImageViewEigenesMonster);
            spielerLeben = FindViewById<TextView>(Resource.Id.textViewLebenSpieler);
            AnzeigenLeben();
            AnzeigenBilder();
            angriff = FindViewById<Button>(Resource.Id.buttonAngriff);
            FindViewById<Button>(Resource.Id.buttonMonster).Click += delegate{ MonsterWechseln(); };
            FindViewById<Button>(Resource.Id.buttonFlucht).Click += delegate { Beenden(); };
            monsteranzeige=FindViewById<LinearLayout>(Resource.Id.LayoutMonsterWaehlen);
            spieler.Typen = Typen.ErstelleTypen();
            ausgewaehltesMonster = spieler.Monster.ElementAt<Monster>(0);
            angriff.Click += delegate
            {
                Angriff();
            };
        }
        private void Angriff()
        {
            if (gegner.Verteidigen(ausgewaehltesMonster.Angriff))
            {
                if (ausgewaehltesMonster.Verteidigen(gegner.Angriff))
                {
                    AnzeigenLeben();
                }
                else
                {
                    foreach (Monster m in spieler.Monster)
                    {
                        if (m.Hp > 0)
                        {

                            MonsterWechseln();
                            break;
                        }
                    }

                }
            }
            else
            {
                ausgewaehltesMonster.Xp += gegner.Art.Sterbexp;
                if (ausgewaehltesMonster.BenoetigteXp <= ausgewaehltesMonster.Xp)
                {
                    ausgewaehltesMonster.Xp = ausgewaehltesMonster.Xp - ausgewaehltesMonster.BenoetigteXp;
                    ausgewaehltesMonster.Lvl++;
                    ausgewaehltesMonster.Hp += ausgewaehltesMonster.Art.Hpzunahme;
                    ausgewaehltesMonster.Maxhp += ausgewaehltesMonster.Art.Hpzunahme;
                    spieler.Monster.Add(gegner);
                    Beenden();
                }
            }
        }
        private void Beenden()
        {
            Intent actMap = new Intent(this, typeof(ActivityMap));
            //Übergabe Spieler
            actMap.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            StartActivity(actMap);
        }

        private void MonsterWechseln()
        {
            SetContentView(Resource.Layout.Monsterwaehlen);
            monsterauswahl = new AuswahlMonster[10];
            int i = 0;
            foreach(Monster m in spieler.Monster)
            {
                 monsterauswahl[i] = new AuswahlMonster(this, m, i);
                Button b = new Button(this);
                i++;
                monsteranzeige.AddView(b);
            }
            
           
        }

        public void Wechsel(Monster m)
        {
            ausgewaehltesMonster = m;
            if(ausgewaehltesMonster.Hp != 0)
            {
                SetContentView(Resource.Layout.Kampf);
                AnzeigenLeben();
                AnzeigenBilder();
            }
         
        }

        private void AnzeigenBilder()
        {
            gegnerBild.SetBackgroundResource(gegner.Art.Pic);
            spielerBild.SetBackgroundResource(ausgewaehltesMonster.Art.Pic);
        }

        private void AnzeigenLeben()
        {
            gegnerLeben.Text = "" + gegner.Hp + "/" + gegner.Maxhp;
            spielerLeben.Text = "" + ausgewaehltesMonster.Hp + "/" + ausgewaehltesMonster.Maxhp;
        }
    }
}