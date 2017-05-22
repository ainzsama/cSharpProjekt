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
    }
}