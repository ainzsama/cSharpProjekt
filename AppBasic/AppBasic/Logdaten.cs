using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppBasic
{
    public class Logdaten
    {
        private int logdatenId;
        private int kaempfeGesamt;
        private int kaempfeGewonnen;
        private DateTime spielzeit;

        public int KaempfeGesamt { get => kaempfeGesamt; set => kaempfeGesamt = value; }
        public int KaempfeGewonnen { get => kaempfeGewonnen; set => kaempfeGewonnen = value; }
        public DateTime Spielzeit { get => spielzeit; set => spielzeit = value; }
        public int LogdatenId { get => logdatenId; set => logdatenId = value; }

        public static Logdaten getTestLogdaten()
        {
            Logdaten l = new Logdaten();
            l.KaempfeGesamt = 10;
            l.KaempfeGewonnen = 5;
            l.LogdatenId = 1;
            l.Spielzeit = DateTime.Now;

            return l;
        }
    }
}