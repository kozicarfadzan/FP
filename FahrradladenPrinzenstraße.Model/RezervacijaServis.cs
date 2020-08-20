using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
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
        public bool IsZavrseno { get; set; }
        public string Proizvodjac { get; set; }
        public string Model { get; set; }
        public string Boja { get; set; }
        public string Opis { get; set; }
        public Tip Tip { get; set; }
        public int DodatniTroskovi { get; set; }
    }
}
