using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
   public class RezervacijaProdajaOprema
    {
        public int RezervacijaProdajaOpremaId { get; set; }
        public int RezervacijaId { get; set; }
        public int OpremaStanjeId { get; set; }
        public OpremaStanje OpremaStanje { get; set; }
        public int? OcjenaKorisnika { get; set; }
    }
}
