using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
   public class Model
    {
        public int ModelId { get; set; }
        public string Naziv { get; set; }

        public int ProizvodjacId { get; set; }
        public Proizvodjac Proizvodjac { get; set; }

        public int Brzina { get; set; }
        public Suspenzija Suspenzija { get; set; }
        public SpolBicikl SpolBicikl { get; set; }
        public Tip Tip { get; set; }

        public MaterijalOkvira MaterijalOkvira { get; set; }
        public int MaterijalOkviraId { get; set; }
    }
}
