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
    public class Monsterart
    {
        private int maxhp;
        private String name;
        private int hpzunahme;
        private int pic;
        private Typ typ;
        private int sterbexp;
        public Monsterart(String name, int maxhp, int pic,int hpzunahme,Typ typ)
        {
            Typ = typ;
            Hpzunahme = hpzunahme;
            Pic = pic;
            Name = name;
            Maxhp = maxhp;
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Maxhp
        {
            get { return maxhp; }
            set { maxhp = value; }
        }

        public int Pic
        {
            get
            {
                return pic;
            }

            set
            {
                pic = value;
            }
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

        public int Hpzunahme
        {
            get
            {
                return hpzunahme;
            }

            set
            {
                hpzunahme = value;
            }
        }

        public int Sterbexp { get => sterbexp; set => sterbexp = value; }
    }
}