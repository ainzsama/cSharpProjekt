using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

namespace AppBasic
{
    public class Monsterart
    {
        private int id;
        private int maxhp;
        private String name;
        private int hpzunahme;
        private int pic;
        private Typ typ;
        private int sterbexp;

        public Monsterart()
        {
        }

        public Monsterart(int id, String name, int maxhp, int pic, int hpzunahme, Typ typ)
        {
            Id = id;
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
        public int Id { get => id; set => id = value; }

        public static Monsterart GetTestMonsterart()
        {
            return new Monsterart(-1, "testArt", 5, 3, 4, Typ.GetTestTyp());
        }
        //public class Monsterart
        //{
        //    private int id;
        //    private int maxhp;
        //    private String name;
        //    private int hpzunahme;
        //    private int pic;
        //    private Typ typ;
        //    private int sterbexp;
        //    public Monsterart(int id, String name, int maxhp, int pic, int hpzunahme, Typ typ)
        //    {
        //        Id = id;
        //        Typ = typ;
        //        Hpzunahme = hpzunahme;
        //        Pic = pic;
        //        Name = name;
        //        Maxhp = maxhp;
        //    }

        //    public String Name
        //    {
        //        get { return name; }
        //        set { name = value; }
        //    }

        //    public int Maxhp
        //    {
        //        get { return maxhp; }
        //        set { maxhp = value; }
        //    }

        //    public int Pic
        //    {
        //        get
        //        {
        //            return pic;
        //        }

        //        set
        //        {
        //            pic = value;
        //        }
        //    }



        //    public Typ Typ
        //    {
        //        get
        //        {
        //            return typ;
        //        }

        //        set
        //        {
        //            typ = value;
        //        }
        //    }

        //    public int Hpzunahme
        //    {
        //        get
        //        {
        //            return hpzunahme;
        //        }

        //        set
        //        {
        //            hpzunahme = value;
        //        }
        //    }

        //    public int Sterbexp { get => sterbexp; set => sterbexp = value; }
        //    public int Id { get => id; set => id = value; }

        //    public static Monsterart GetTestMonsterart()
        //    {
        //        return new Monsterart(-1, "testArt", 5, 3, 4, Typ.GetTestTyp());
        //    }
        }
}