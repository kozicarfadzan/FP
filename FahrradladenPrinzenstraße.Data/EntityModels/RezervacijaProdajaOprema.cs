using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
   public class RezervacijaProdajaOprema
    {
        public int RezervacijaProdajaOpremaId { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int OpremaStanjeId { get; set; }
        public OpremaStanje OpremaStanje { get; set; }
     
    }
}
