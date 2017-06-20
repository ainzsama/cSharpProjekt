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
        private int hp;  //wau wau
       
        
        private String nickname;
        private Marker marker;

        
        private Angriff angriff; //attake
        private int maxhp; //anzeigen
        private Monsterart art; //Basismonster
        private int lvl;
        private int xp;
        private int benoetigteXp;
      
        //public static Monster GetTestMonster() //meins(LL)
        //{
        //    Monster m = new Monster();
        //    m.MonsterId = 1;
        //    m.Hp = 100;
        //    m.atk = 10;
        //    m.Name = "test";
        //    m.Nickname = "testNic";
        //    m.Angriff = Angriff.GetTestAngriff();
        //    m.Art = Monsterart.GetTestMonsterart();
        //    m.Typ = Typ.GetTestTyp();

        //    return m;
        //}
        public static Monster GetTestMonster()
        {
            Monster m = new Monster();
            m.MonsterId = 1;
            m.Hp = 100;
            m.Maxhp = 100;
            m.Angriff = new Angriff(1, "Biss", 20, Typen.ErstelleTypen().ElementAt<Typ>(0));
            m.Art = new Monsterart(-1, "Beiserchen", 100, Resource.Drawable.monster1, 10, Typen.ErstelleTypen().ElementAt<Typ>(0));

            return m;
        }
        public static Monster GetTestMonster2()
        {
            Monster m = new Monster();
            m.MonsterId = 1;
            m.Hp = 100;
            m.Maxhp = 100;
            m.Angriff = new Angriff(2, "Pusten", 20, Typen.ErstelleTypen().ElementAt<Typ>(3));
            m.Art = new Monsterart(-2, "Flämchen", 100, Resource.Drawable.monster2, 10, Typen.ErstelleTypen().ElementAt<Typ>(1));
            return m;
        }

        public Monster(MonsterUebertragung m)
        {
            MonsterId = m.MonsterId;
            Art = m.Art;
            Marker = null;
            Angriff = m.Angriff;
            Nickname = m.Nickname;
            Hp = m.Hp;
            Maxhp = m.Maxhp;
            Lvl = m.Lvl;
            Xp = m.Xp;
            BenoetigteXp = m.BenoetigteXp;
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
        public bool Verteidigen(Angriff a)
        {
            Hp = hp - a.Gegen(this.Art.Typ);
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

       

        //public string Name
        //{
        //    get
        //    {
        //        return name;
        //    }

        //    set
        //    {
        //        name = value;
        //    }
        //}

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

       

    }
}