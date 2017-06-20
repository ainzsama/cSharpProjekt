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
using System.IO;

namespace AppBasic
{
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


        public static string GetPathArten()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "arten.xml");
        }

        public static string GetPathAngriffe()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "angriffe.xml");
        }
    }
}