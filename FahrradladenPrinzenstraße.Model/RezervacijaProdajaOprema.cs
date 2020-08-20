using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
   public class RezervacijaProdajaOprema
    {
        public int RezervacijaProdajaOpremaId { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int OpremaStanjeId { get; set; }
        public OpremaStanje OpremaStanje { get; set; }
        public int? OcjenaKorisnika
        {
            get
            {
                return OpremaStanje?.Oprema?.OcjenaProizvoda?.Where(x => x.KlijentId == Rezervacija.KlijentId).FirstOrDefault()?.Ocjena ?? 0;
            }
        }

    }
}
