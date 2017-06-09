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
using Android.Gms.Maps.Model;

namespace AppBasic
{
    public class Monster
    {
        private int monsterId;
        private int hp;
        private int atk;
        private String name;
        private String nickname;
        private Marker marker;

       
        public static Monster GetTestMonster()
        {
            Monster m = new Monster();
            m.MonsterId = 1;
            m.Hp = 100;
            m.atk = 10;
            m.Name = "test";
            m.Nickname = "testNic";

            return m;
        }


        //Teil von Lukas Wölfle
        public Monster()
        {

        }
        public Monster(Angriff angriff, Monsterart art)
        {
            Angriff = angriff;
            Art = art;
            Hp = art.Maxhp;
  
        }
        private Angriff angriff;
        private int maxhp;
        private Monsterart art;
        private int lvl;
        private int xp;
        private int benoetigteXp;
        private Typ typ;
        public bool Verteidigen(Angriff a)
        {
            Hp = hp - a.Gegen(this.Typ);
            if(Hp>0)
            {
                return true;
            }
            if(hp < 0)
            {
                hp = 0;
            }
            return false;
        }
        public Angriff Angriff
        {
            get { return angriff; }
            set { angriff = value; }
        }

    

        public Monsterart Art
        {
            get { return art; }
            set { art = value; }
        }

        public int MonsterId
        {
            get
            {
                return monsterId;
            }

            set
            {
                monsterId = value;
            }
        }

        public int Hp
        {
            get
            {
                return hp;
            }

            set
            {
                hp = value;
            }
        }

        public int Atk
        {
            get
            {
                return atk;
            }

            set
            {
                atk = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Nickname
        {
            get
            {
                return nickname;
            }

            set
            {
                nickname = value;
            }
        }

        public Marker Marker
        {
            get
            {
                return marker;
            }

            set
            {
                marker = value;
            }
        }

        public int Maxhp
        {
            get
            {
                return maxhp;
            }

            set
            {
                maxhp = value;
            }
        }

        public int Lvl
        {
            get
            {
                return lvl;
            }

            set
            {
                lvl = value;
            }
        }

        public int Xp
        {
            get
            {
                return xp;
            }

            set
            {
                xp = value;
            }
        }

        public int BenoetigteXp
        {
            get
            {
                return benoetigteXp;
            }

            set
            {
                benoetigteXp = value;
            }
        }

        internal Typ Typ { get => typ; set => typ = value; }

    }
}