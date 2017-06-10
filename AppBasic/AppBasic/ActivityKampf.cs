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
        private ImageView gegnerBild;
        private TextView gegnerLeben;
        private TextView spielerLeben;
        private Button angriff;
        private Monster ausgewaehltesMonster;
        private LinearLayout monsteranzeige;
        private AuswahlMonster[] monsterauswahl;
        private Button buttonup;
        private int gezeigt;
        private Button buttondown;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Kampf);

            //Entgegennehmen Spieler und Gegner/Monster
                 spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
                 gegner = JsonConvert.DeserializeObject<Monster>(Intent.GetStringExtra("gegner"));
            /*spieler = new Spieler();
            spieler.Monster = new List<Monster>();
            spieler.Monster.Add(Monster.GetTestMonster());
            gegner = Monster.GetTestMonster();*/
            // Create your application here
            gegnerLeben = FindViewById<TextView>(Resource.Id.textViewLebenGegner);
            gegnerBild = FindViewById<ImageView>(Resource.Id.ImageViewGegner);
            spielerBild = FindViewById<ImageView>(Resource.Id.ImageViewEigenesMonster);
            spielerLeben = FindViewById<TextView>(Resource.Id.textViewLebenSpieler);
            buttondown = FindViewById<Button>(Resource.Id.ButtonRunter);
            buttonup = FindViewById<Button>(Resource.Id.ButtonHoch);
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
            buttondown.Click += delegate
            {
                Runter();
            };
            buttonup.Click += delegate
            {
                Hoch();
            };
        }

        private void Hoch()
        {
            if(gezeigt > 0)
            {
                monsteranzeige.RemoveAllViews();
                gezeigt--;
                for(int i=gezeigt; i<gezeigt+5; i++)
                {
                    monsteranzeige.AddView(monsterauswahl[i].Button);
                }
            }
        }

        private void Runter()
        {
            if (gezeigt < monsterauswahl.Length-6)
            {
                monsteranzeige.RemoveAllViews();
                gezeigt++;
                for (int i = gezeigt; i < gezeigt + 5; i++)
                {
                    monsteranzeige.AddView(monsterauswahl[i].Button);
                }
            }
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
            gezeigt = 0;
            foreach(Monster m in spieler.Monster)
            {
                monsterauswahl[i] = new AuswahlMonster(this, m, i);
                i++;
                if (i < 5)
                {
                    monsteranzeige.AddView(monsterauswahl[i].Button);
                }
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