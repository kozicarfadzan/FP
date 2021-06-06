using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class RezervacijaIznajmljenaBicikla
    {
        public int RezervacijaIznajmljenaBiciklaID { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int BiciklStanjeId { get; set; }
        public BiciklStanje BiciklStanje { get; set; }
        [DisplayName("Dan preuzimanja")]
        public DateTime DatumPreuzimanja { get; set; }
        [DisplayName("Dan vraćanja")]
        public DateTime DatumVracanja { get; set; }

        [DisplayName("Rezervacija istekla")]
        public bool Isteklo { get; set; }
        [DisplayName("Rezervacija završena")]
        public bool IsZavrseno { get; set; }

        public int BrojDana => (int)((DatumVracanja.Date - DatumPreuzimanja.Date).TotalDays + 1);

        public int? OcjenaKorisnika
        {
            get
            {
                return BiciklStanje?.Bicikl?.OcjenaProizvoda?.Where(x => x.KlijentId == Rezervacija.KlijentId).FirstOrDefault()?.Ocjena ?? 0;
            }
        }
    }
}
