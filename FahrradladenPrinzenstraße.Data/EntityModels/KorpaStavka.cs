using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
   public class KorpaStavka
    {
        public int KorpaStavkaId { get; set; }
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }
        public int? BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }
        public int? OpremaId { get; set; }
        public Oprema Oprema { get; set; }
        public int? DioId { get; set; }
        public Dio Dio { get; set; }
        public int Kolicina { get; set; }
    }
}
