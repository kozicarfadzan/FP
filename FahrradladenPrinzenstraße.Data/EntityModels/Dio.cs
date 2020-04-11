using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class Dio
    {
        public int DioId { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public int ProizvodjacID { get; set; }
        public Proizvodjac Proizvodjac { get; set; }
    }
}
