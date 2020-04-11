using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
  public  class RezervacijaProdajaBicikla
    {
        public int RezervacijaProdajaBiciklaID { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int BiciklStanjeId { get; set; }
        public BiciklStanje BiciklStanje { get; set; }
       
    }
}
