using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
   public class TerminStavka
    {
        public int TerminStavkaId { get; set; }
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }
        public int BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }
        public int Kolicina { get; set; }
        public DateTime DatumPreuzimanja { get; set; }
        public DateTime DatumVracanja { get; set; }

        public int BrojDana => (int)((DatumVracanja.Date - DatumPreuzimanja.Date).TotalDays + 1);

        public double UkupnaCijena => Bicikl.CijenaPoDanu.Value * BrojDana;
    }
}
