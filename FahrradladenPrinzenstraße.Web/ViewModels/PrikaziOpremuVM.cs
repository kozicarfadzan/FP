using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.ViewModels
{
    public class PrikaziOpremuVM
    {

        public string Pretraga { get; set; }
        public int? Poredak { get; set; }
        public bool PrikaziKaoListu { get; set; }

        public PagedResult<Row> PagedResult { get; set; }
        public int Page { get; set; } = 1;

        public List<Proizvodjac> Proizvodjaci { get; set; }
        public List<int> ProizvodjacId { get; set; }
        public List<PreporuceniProizvod> PopularnaOprema { get; internal set; }

        public class Row
        {
            public int OpremaId { get; set; }
            public string Naziv { get; set; }
            public double? Cijena { get; set; }
            public byte[] Slika { get; set; }
            public string Opis { get; set; }
            public int Kolicina { get; set; }


            public bool Aktivan { get; set; }
            public Proizvodjac Proizvodjac { get; set; }

        }
    }
}
