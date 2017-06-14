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
    class Protokoll
    {
        public const string TRENN = "#";
        public const string SPIELER = "SPL";
        public const string ANMELDUNG = "ANM";
        public const string ABMELDUNG = "ABM";
        public const string REGISTRIERUNG = "REG";
        public const string ERROR = "ERROR";
        public const string ARTEN = "ARTEN";
        public const string ANGRIFFE = "ANGRIFFE";

        public const string PFADART = "Monsterarten.xml";
        public const string PFADANGR = "Angriffe.xml";
    }
}