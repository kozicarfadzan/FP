using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
    public class RezervacijaIznajmljenaBicikla
    {
        public int RezervacijaIznajmljenaBiciklaID { get; set; }
        public int RezervacijaId { get; set; }
        public int BiciklStanjeId { get; set; }
        public BiciklStanje BiciklStanje { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumVracanja { get; set; }

        public bool Isteklo { get; set; }
        public bool IsZavrseno { get; set; }
        public int? OcjenaKorisnika { get; set; }

    }
}
