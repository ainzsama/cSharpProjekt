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
        private List<Monster> monster = new List<Monster>();
        private Logdaten logdaten;

        public string Name { get => name; set => name = value; }
        public Logdaten Logdaten { get => logdaten; set => logdaten = value; }
        public List<Monster> Monster { get => monster; set => monster = value; }
        public int SpielerId { get => spielerId; set => spielerId = value; }
    }
}