using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
  public  class RezervacijaProdajaBicikla
    {
        public int RezervacijaProdajaBiciklaID { get; set; }
        public int RezervacijaId { get; set; }
        public int BiciklStanjeId { get; set; }
        public BiciklStanje BiciklStanje { get; set; }

        public int? OcjenaKorisnika { get; set; }
    }
}
