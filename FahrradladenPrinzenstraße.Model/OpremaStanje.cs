using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
  public  class OpremaStanje
    {
        public int OpremaStanjeId { get; set; }
        public int OpremaId { get; set; }
        public Oprema Oprema { get; set; }
        public int LokacijaId { get; set; }
        public Lokacija Lokacija { get; set; }
        public string Sifra { get; set; }

        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;

        public Klijent Kupac { get; set; }
        public int? KupacId { get; set; }

        public List<RezervacijaProdajaOprema> RezervacijaProdajaOprema { get; set; }
    }
}
