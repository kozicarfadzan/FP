using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
   public class RezervacijaProdajaDio
    {
       
        public int RezervacijaProdajaDioId { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int DioStanjeId { get; set; }
        public DioStanje DioStanje { get; set; }
    }
}
