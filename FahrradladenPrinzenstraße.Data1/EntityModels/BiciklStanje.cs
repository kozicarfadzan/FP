using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class BiciklStanje
    {
        public int BiciklStanjeId { get; set; }
        public int BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }
        public int LokacijaId { get; set; }
        public Lokacija Lokacija { get; set; }
        public string Sifra { get; set; }
    }
}
