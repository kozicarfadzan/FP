using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
   public class DioStanje
    {
        public int DioStanjeId { get; set; }
        public int DioId { get; set; }
        public Dio Dio { get; set; }
        public int LokacijaId { get; set; }
        public Lokacija Lokacija { get; set; }
        public string Sifra { get; set; }

        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;

        public Klijent Kupac { get; set; }
        public int? KupacId { get; set; }

        public List<RezervacijaProdajaDio> RezervacijaProdajaDio { get; set; }
    }
}
