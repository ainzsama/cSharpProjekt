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
    public class Angriff
    {
        private String name;
        private int schaden;
        private Typ typ;
        public Angriff(String name, int schaden,Typ typ)
        {
            Typ = typ;
            Schaden = schaden;
            Name = name;
        }
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
     
        public int Gegen(Typ typ)
        {
            int schaden = Schaden;
            if(Typ.Starkgegen==typ.Id)
            {
                schaden *= 2;
            }
            return schaden;
        }
        public int Schaden
        {
            get { return schaden; }
            set { schaden = value; }
        }

        public Typ Typ
        {
            get
            {
                return typ;
            }

            set
            {
                typ = value;
            }
        }

        public static Angriff GetTestAngriff()
        {
            return new Angriff("Test", 22, Typ.GetTestTyp());
        }
    }
}