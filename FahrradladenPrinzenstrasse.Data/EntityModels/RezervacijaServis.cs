using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
   public class RezervacijaServis
    {
        public int RezervacijaServisId { get; set; }
        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
        public int ServisId { get; set; }
        public Servis Servis { get; set; }
        [DisplayName("Datum servisiranja")]
        public DateTime DatumServisiranja { get; set; }
        [DisplayName("Servis odobren")]
        public bool IsOdobreno { get; set; }
        [DisplayName("Servis završen")]
        public bool IsZavrseno { get; set; }
        [DisplayName("Proizvođač")]
        public string Proizvodjac { get; set; }
        public string Model { get; set; }
        public string Boja { get; set; }
        public string Opis { get; set; }
        public Tip Tip { get; set; }
        [DisplayName("Dodatni troškovi")]
        public int DodatniTroskovi { get; set; }
    }
}
