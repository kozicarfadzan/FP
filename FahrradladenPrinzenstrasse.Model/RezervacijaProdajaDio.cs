using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
   public class RezervacijaProdajaDio
    {
       
        public int RezervacijaProdajaDioId { get; set; }
        public int RezervacijaId { get; set; }
        public int DioStanjeId { get; set; }
        public DioStanje DioStanje { get; set; }
        public int? OcjenaKorisnika { get; set; }
    }
}
