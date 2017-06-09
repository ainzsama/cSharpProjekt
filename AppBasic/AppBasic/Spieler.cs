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
    class Spieler 
    {
        private int spielerId;
        private String name;
        private List<Monster> monster;
        private Logdaten logdaten;
        private List<Typ> typen;

        public Spieler()
        {
            monster = new List<AppBasic.Monster>();
        }

        public string Name { get => name; set => name = value; }
        public Logdaten Logdaten { get => logdaten; set => logdaten = value; }
        public List<Monster> Monster { get => monster; set => monster = value; }
        public int SpielerId { get => spielerId; set => spielerId = value; }
        internal List<Typ> Typen { get => typen; set => typen = value; }

        public static Spieler GetTestSpieler()
        {
            Spieler s = new Spieler();
            s.SpielerId = 1;
            s.Name = "Test";
            s.Logdaten = Logdaten.getTestLogdaten();
            
            return s;

        }
    }
}