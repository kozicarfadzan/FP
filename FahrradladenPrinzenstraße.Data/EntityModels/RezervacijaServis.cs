using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
   public class RezervacijaServis
    {
        public int RezervacijaServisId { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int ServisId { get; set; }
        public Servis Servis { get; set; }
        public DateTime DatumServisiranja { get; set; }
        public bool IsOdobreno { get; set; }
    }
}
