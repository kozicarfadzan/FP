using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class OcjenaProizvoda
    {
        public int Id { get; set; }
        public int Ocjena { get; set; }
        public int? BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }
        public int? DioId { get; set; }
        public Dio Dio { get; set; }
        public int? OpremaId { get; set; }
        public Oprema Oprema { get; set; }
        public DateTime DatumOcjene { get; set; }
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }
    }
}
