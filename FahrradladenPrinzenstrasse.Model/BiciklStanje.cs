using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
    public class BiciklStanje
    {
        public int BiciklStanjeId { get; set; }
        public int BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }
        public int LokacijaId { get; set; }
        public Lokacija Lokacija { get; set; }
        //public string Sifra { get; set; }
        
        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;

        //[JsonIgnore]
        //public Klijent Kupac { get; set; }
        //public int? KupacId { get; set; }

        //[JsonIgnore]
        //public List<RezervacijaIznajmljenaBicikla> RezervacijaIznajmljenaBicikla { get; set; }
        //[JsonIgnore]
        //public List<RezervacijaProdajaBicikla> RezervacijaProdajaBicikla { get; set; }
    }
}
