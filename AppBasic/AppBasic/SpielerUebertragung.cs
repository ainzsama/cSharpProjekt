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
    public class SpielerUebertragung
    {
        private int spielerId;
        private String name;
        private List<MonsterUebertragung> monster;
        private Logdaten logdaten;
        private List<Typ> typen;
        private string kennwort;

        public SpielerUebertragung()
        {
            monster = new List<MonsterUebertragung>();
        }

        public string Name { get => name; set => name = value; }
        public Logdaten Logdaten { get => logdaten; set => logdaten = value; }
        public List<MonsterUebertragung> Monster { get => monster; set => monster = value; }
        public int SpielerId { get => spielerId; set => spielerId = value; }
        internal List<Typ> Typen { get => typen; set => typen = value; }
        public string Kennwort { get => kennwort; set => kennwort = value; }

        public SpielerUebertragung(int i, string b, string k)
        {
            SpielerId = i;
            Name = b;
            Kennwort = k;
        }
        public SpielerUebertragung(string n, string k)
        {
            Name = n;
            Kennwort = k;
        }
    }
}
