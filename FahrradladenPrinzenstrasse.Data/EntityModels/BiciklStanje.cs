using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class BiciklStanje
    {
        public int BiciklStanjeId { get; set; }
        public int BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }
        public int LokacijaId { get; set; }
        public Lokacija Lokacija { get; set; }
        [DisplayName("Šifra bicikla")]
        public string Sifra { get; set; }
        
        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;

        public Klijent Kupac { get; set; }
        public int? KupacId { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
        public List<RezervacijaIznajmljenaBicikla> RezervacijaIznajmljenaBicikla { get; set; }
        public List<RezervacijaProdajaBicikla> RezervacijaProdajaBicikla { get; set; }
    }
}
