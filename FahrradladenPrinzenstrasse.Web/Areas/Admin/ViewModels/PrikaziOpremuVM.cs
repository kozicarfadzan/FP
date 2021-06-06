using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class PrikaziOpremuVM
    {
        // Filteri
        public string Pretraga { get; set; }
        public int ProizvodjacId { get; set; }
        public bool Aktivan { get; set; }

        public List<SelectListItem> Proizvodjaci { get; set; }
     

        public List<Row> Oprema { get; set; }
        public class Row
        {
            public int OpremaId { get; set; }
            public string Naziv { get; set; }
            public double Cijena { get; set; }
            public string Proizvodjac { get; set; }

            public byte[] Slika { get; set; }
            public int Kolicina { get; internal set; }
            public bool Aktivan { get; set; }
        }


    }

}

