using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
   public class RezervacijaProdajaDio
    {
       
        public int RezervacijaProdajaDioId { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int DioStanjeId { get; set; }
        public DioStanje DioStanje { get; set; }
        public int? OcjenaKorisnika
        {
            get
            {
                return DioStanje?.Dio?.OcjenaProizvoda?.Where(x => x.KlijentId == Rezervacija.KlijentId).FirstOrDefault()?.Ocjena ?? 0;
            }
        }
    }
}
