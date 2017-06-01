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
    class Monster
    {
        private int monsterId;
        private int hp;
        private int atk;//wird durch Angriff entfernt
        private String name;
        private String nickname;
        private Marker marker;
        /*Die Pfeile kennt VS 15 nich
        public int Hp { get => hp; set => hp = value; }
        public int Atk { get => atk; set => atk = value; }
        public string Name { get => name; set => name = value; }
        public int MonsterId { get => monsterId; set => monsterId = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public Marker Marker { get => marker; set => marker = value; }
        */

        public static Monster getTestMonster()
        {
            Monster m = new Monster();
            m.MonsterId = 1;
            m.Hp = 100;
            m.Atk = 10;
            m.Name = "test";
            m.Nickname = "testNic";

            return m;
        }

        //Teil von Lukas Wölfle
        public Monster(Angriff angriff, Monsterart art)
        {
            Angriff = angriff;
            Art = art;
            Hp = art.Maxhp;
        }
        private Angriff angriff;
        private int Maxhp;
        private Monsterart art;
        private int lvl;
        private int xp;
        private int needxp;
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

        public int Maxhp1
        {
            get
            {
                return Maxhp;
            }

            set
            {
                Maxhp = value;
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

        public int Needxp
        {
            get
            {
                return needxp;
            }

            set
            {
                needxp = value;
            }
        }
    }
}