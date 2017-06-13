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
using System.Threading;

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
        private LinearLayout hauptlayout;
        private LinearLayout monsteranzeige;
        private List<AuswahlMonster> monsterauswahl; //Muss List sein
        private Button buttonup;
        private int gezeigt;
        private Button buttondown;
        private TableLayout menu;
        private TextView label;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Kampf);

            //Entgegennehmen Spieler und Gegner/Monster
            spieler = JsonConvert.DeserializeObject<Spieler>(Intent.GetStringExtra("spieler"));
            gegner = JsonConvert.DeserializeObject<Monster>(Intent.GetStringExtra("gegner"));
            spieler.Monster.Add(Monster.GetTestMonster());
            /*spieler = new Spieler();
            spieler.Monster = new List<Monster>();
            spieler.Monster.Add(Monster.GetTestMonster());
            gegner = Monster.GetTestMonster();*/

            // Create your application here
            hauptlayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            gegnerLeben = FindViewById<TextView>(Resource.Id.textViewLebenGegner);
            gegnerBild = FindViewById<ImageView>(Resource.Id.ImageViewGegner);
            spielerBild = FindViewById<ImageView>(Resource.Id.ImageViewEigenesMonster);
            spielerLeben = FindViewById<TextView>(Resource.Id.textViewLebenSpieler);
            menu = FindViewById<TableLayout>(Resource.Id.tableLayoutMenu);
            
            angriff = FindViewById<Button>(Resource.Id.buttonAngriff);
            FindViewById<Button>(Resource.Id.buttonMonster).Click += delegate { MonsterWechseln(); };
            FindViewById<Button>(Resource.Id.buttonFlucht).Click += delegate { Beenden(); };
            
            ausgewaehltesMonster = spieler.Monster.ElementAt<Monster>(0);
            AnzeigenLeben();
            AnzeigenBilder();
            spieler.Typen = Typen.ErstelleTypen();
            angriff.Click += delegate
            {
                Angriff();
            };
            foreach(Monster m in spieler.Monster)
            {
                m.Hp = m.Maxhp;
            }
        }

        private void Hoch()
        {
            if (gezeigt > 0)
            {
                monsteranzeige.RemoveAllViews();
                gezeigt--;
                for (int i = gezeigt; i < gezeigt + 5; i++)
                {
                    monsteranzeige.AddView(monsterauswahl[i].Button);
                }
            }
        }

        private void Runter()
        {
            if (gezeigt < monsterauswahl.Count - 6)
            {
                monsteranzeige.RemoveAllViews();
                gezeigt++;
                for (int i = gezeigt; i < gezeigt + 5; i++)
                {
                    monsteranzeige.AddView(monsterauswahl[i].Button);
                }
            }
        }
        private void Textanzeigen(String text, int dauer)
        {
            hauptlayout.RemoveView(angriff);
            hauptlayout.RemoveView(menu);
            label = new TextView(this);
            label.Click += delegate { Thread.CurrentThread.Interrupt(); };
            label.Text = text;
            hauptlayout.AddView(label);
            try
            {
                Thread.Sleep(dauer);
            }
            catch (Exception)
            {
            }
            Textverschwindet();
        }
        private void Textverschwindet()
        {
            hauptlayout.RemoveView(label);
            hauptlayout.AddView(menu);
            hauptlayout.AddView(angriff);
        }
        private void Angriff()
        {
            if (gegner.Verteidigen(ausgewaehltesMonster.Angriff))
            {
                Textanzeigen("Dein Monster greift an",2000);
                if (ausgewaehltesMonster.Verteidigen(gegner.Angriff))
                {
                    Textanzeigen("Das gegnerische Monster greift an", 2000);
                    AnzeigenLeben();
                }
                else
                {
                    Textanzeigen("Dein Monster wurde geschlagen", 2000);
                    foreach (Monster m in spieler.Monster)
                    {
                        if (m.Hp > 0)
                        {
                            MonsterWechseln();
                            return;
                        }
                    }
                    Textanzeigen("Du hast verloren", 2000);
                    Beenden();

                }
            }
            else
            {
                Textanzeigen("Dein Monster hat gewonnen", 2000);
                ausgewaehltesMonster.Xp += gegner.Art.Sterbexp;
                if (ausgewaehltesMonster.BenoetigteXp <= ausgewaehltesMonster.Xp)
                {
                    ausgewaehltesMonster.Xp = ausgewaehltesMonster.Xp - ausgewaehltesMonster.BenoetigteXp;
                    ausgewaehltesMonster.Lvl++;
                    ausgewaehltesMonster.Hp += ausgewaehltesMonster.Art.Hpzunahme;
                    ausgewaehltesMonster.Maxhp += ausgewaehltesMonster.Art.Hpzunahme;
                 
                }
                spieler.Monster.Add(gegner);
                spieler.Logdaten.KaempfeGewonnen++;
                Beenden();
               
            }
        }
        private void Beenden()
        {
            spieler.Logdaten.KaempfeGesamt++;
            Textanzeigen("Der Kampf ist vorbei", 2000);
            Intent actMap = new Intent(this, typeof(ActivityMap));
            //Übergabe Spieler
            actMap.PutExtra("spieler", JsonConvert.SerializeObject(spieler));
            StartActivity(actMap);

        }

        private void MonsterWechseln()
        {
            SetContentView(Resource.Layout.Monsterwaehlen);
            monsteranzeige = FindViewById<LinearLayout>(Resource.Id.LayoutMonsterWaehlen);
            buttondown = FindViewById<Button>(Resource.Id.ButtonRunter);
            buttonup = FindViewById<Button>(Resource.Id.ButtonHoch);
            buttondown.Click += delegate
            {
                Runter();
            };
            buttonup.Click += delegate
            {
                Hoch();
            };
            monsterauswahl = new List<AuswahlMonster>();
            int i = 0;
            gezeigt = 0;
            foreach (Monster m in spieler.Monster)
            {
                monsterauswahl.Add(new AuswahlMonster(this, m, i));
                
                if (i < 5)
                {
                    monsteranzeige.AddView(monsterauswahl[i].Button);
                }
                i++;
            }


        }

        public void Wechsel(Monster m)
        {
            ausgewaehltesMonster = m;
            if (ausgewaehltesMonster.Hp != 0)
            {
                SetContentView(Resource.Layout.Kampf);
                gegnerLeben = FindViewById<TextView>(Resource.Id.textViewLebenGegner);
                gegnerBild = FindViewById<ImageView>(Resource.Id.ImageViewGegner);
                spielerBild = FindViewById<ImageView>(Resource.Id.ImageViewEigenesMonster);
                spielerLeben = FindViewById<TextView>(Resource.Id.textViewLebenSpieler);


                angriff = FindViewById<Button>(Resource.Id.buttonAngriff);
                FindViewById<Button>(Resource.Id.buttonMonster).Click += delegate { MonsterWechseln(); };
                FindViewById<Button>(Resource.Id.buttonFlucht).Click += delegate { Beenden(); };

                
                AnzeigenLeben();
                AnzeigenBilder();
                spieler.Typen = Typen.ErstelleTypen();
                angriff.Click += delegate
                {
                    Angriff();
                };
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