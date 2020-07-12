using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class Servis
    {
        public int ServisId { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public double Trajanje { get; set; }
    }
}
