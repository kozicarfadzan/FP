using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
    public class RezervacijaIznajmljenaBicikla
    {
        public int RezervacijaIznajmljenaBiciklaID { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int BiciklStanjeId { get; set; }
        public BiciklStanje BiciklStanje { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumVracanja { get; set; }

        public bool Isteklo { get; set; }
        public bool IsZavrseno { get; set; }
    }
}
