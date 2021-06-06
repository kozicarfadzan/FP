using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Notifikacija
    {
        public int NotifikacijaId { get; set; }
        public int ZaposlenikId { get; set; }
        public Zaposlenik Zaposlenik { get; set; }

        public int? RezervacijaIznajmljenaBiciklaId { get; set; }
        public RezervacijaIznajmljenaBicikla RezervacijaIznajmljenaBicikla { get; set; }

        public int? RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }

        public bool IsProcitano { get; set; }

        public TipNotifikacije Tip { get; set; }
        public DateTime DatumVrijeme { get; set; }
    }

    public enum TipNotifikacije
    {
        Istekao_Termin,
        Nova_Narudzba,
        Novi_Termin,
        Novi_Servis,
    }
}
