using System;
using System.Collections.Generic;
using System.Linq;

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
        private int id;
        private String name;
        private int schaden;
        private Typ typ;

        public Angriff()
        { }
        public Angriff(int id, String name, int schaden, Typ typ)
        {
            Id = id;
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
            if (typ.Starkgegen == typ.Id)
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

        public int Id { get => id; set => id = value; }

        public static Angriff GetTestAngriff()
        {
            return new Angriff(-1, "Test", 22, Typ.GetTestTyp());
        }
    }
    public class Logdaten
    {
        private int logdatenId;
        private int kaempfeGesamt;
        private int kaempfeGewonnen;


        public int KaempfeGesamt { get => kaempfeGesamt; set => kaempfeGesamt = value; }
        public int KaempfeGewonnen { get => kaempfeGewonnen; set => kaempfeGewonnen = value; }

        public int LogdatenId { get => logdatenId; set => logdatenId = value; }

        public static Logdaten getTestLogdaten()
        {
            Logdaten l = new Logdaten();
            l.KaempfeGesamt = 10;
            l.KaempfeGewonnen = 5;
            l.LogdatenId = 1;


            return l;
        }

        public Logdaten(int id, int gesamt, int gewonnen)
        {
            LogdatenId = id;
            KaempfeGesamt = gesamt;
            KaempfeGewonnen = gewonnen;

        }
        public Logdaten()
        { }
    }
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
        { }
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
    }

    public class MonsterUebertragung
    {
        private int monsterId;
        private int hp;
        private String name;
        private String nickname;


        private Angriff angriff;
        private int maxhp;
        private Monsterart art;
        private int lvl;
        private int xp;
        private int benoetigteXp;

        public static MonsterUebertragung GetTestMonster()
        {
            MonsterUebertragung m = new MonsterUebertragung();
            m.MonsterId = 1;
            m.Hp = 100;

            m.Name = "test";
            m.Nickname = "testNic";
            m.Angriff = Angriff.GetTestAngriff();
            m.Art = Monsterart.GetTestMonsterart();


            return m;
        }



        public MonsterUebertragung()
        {

        }

        public MonsterUebertragung(int id, int maxhp, string name, string nic, int level, int xp, int benoetigteXp)
        {
            MonsterId = id;
            Maxhp = maxhp;
            Name = name;
            Nickname = nic;
            Lvl = level;
            Xp = xp;
            BenoetigteXp = benoetigteXp;
        }
        public MonsterUebertragung(Angriff angriff, Monsterart art)
        {
            Angriff = angriff;
            Art = art;
            Hp = art.Maxhp;

        }
        public bool Verteidigen(Angriff a)
        {
            Hp = hp - a.Gegen(this.Art.Typ);
            if (Hp > 0)
            {
                return true;
            }
            if (hp < 0)
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
    public class Protokoll
    {
        public const string TRENN = "#";
        public const string SPIELER = "SPL";
        public const string ANMELDUNG = "ANM";
        public const string ABMELDUNG = "ABM";
        public const string REGISTRIERUNG = "REG";
        public const string ERROR = "ERROR";
        public const string ARTEN = "ARTEN";
        public const string ANGRIFFE = "ANGRIFFE";
        public const string TYPEN = "TYPEN";




    }
    public class SpielerUebertragung
    {
        private int spielerId;
        private String name;
        private List<MonsterUebertragung> monster;
        private Logdaten logdaten;

        private string kennwort;

        public SpielerUebertragung()
        {

        }

        public string Name { get => name; set => name = value; }
        public Logdaten Logdaten { get => logdaten; set => logdaten = value; }
        public List<MonsterUebertragung> Monster { get => monster; set => monster = value; }
        public int SpielerId { get => spielerId; set => spielerId = value; }

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
    public class Typ
    {
        private int id;
        private String name;
        private int starkgegen;
        public Typ(int id, String name, int starkgegen)
        {
            Starkgegen = starkgegen;
            Name = name;
            Id = id;
        }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Starkgegen { get => starkgegen; set => starkgegen = value; }

        public static Typ GetTestTyp()
        {
            List<Typ> list = Typen.ErstelleTypen();
            return list.ElementAt(5);
        }
    }
    public class Typen
    {
        private static List<Typ> typen = new List<Typ>();

        public static List<Typ> ErstelleTypen()
        {
            typen.Add(new Typ(0, "Normal", 2));
            typen.Add(new Typ(1, "Feuer", 3));
            typen.Add(new Typ(2, "Wasser", 1));
            typen.Add(new Typ(3, "Luft", 0));
            typen.Add(new Typ(4, "Testtyp", 0));
            return typen;
        }
    }
}