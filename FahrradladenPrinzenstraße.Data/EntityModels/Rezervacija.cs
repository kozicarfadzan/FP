using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class Rezervacija
    {
        public int RezervacijaId { get; set; }
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }
        public DateTime? DatumRezervacije { get; set; }

        public List<RezervacijaIznajmljenaBicikla> RezervacijaIznajmljenaBicikla { get; set; }
        public List<RezervacijaProdajaBicikla> RezervacijaProdajaBicikla { get; set; }
        public List<RezervacijaProdajaDio> RezervacijaProdajaDio { get; set; }
        public List<RezervacijaProdajaOprema> RezervacijaProdajaOprema { get; set; }
        public List<RezervacijaServis> RezervacijaServis { get; set; }

    }
}
