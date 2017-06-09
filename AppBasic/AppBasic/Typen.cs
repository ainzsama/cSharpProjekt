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
    class Typen
    {
        private static List<Typ> typen = new List<Typ>();

        public static List<Typ> ErstelleTypen()
        {
            typen.Add(new Typ(0,"Normal",2));
            typen.Add(new Typ(1,"Feuer",3));
            typen.Add(new Typ(2,"Wasser",1));
            typen.Add(new Typ(3,"Luft",0));
            return typen;
        }
    }
}