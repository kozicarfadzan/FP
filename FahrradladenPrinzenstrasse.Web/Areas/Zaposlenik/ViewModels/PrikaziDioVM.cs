using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels
{
    public class PrikaziDioVM
    {
        // Filteri
        public string Pretraga { get; set; }
        public int ProizvodjacId { get; set; }
        public bool Aktivan { get; set; }

        public List<SelectListItem> Proizvodjaci { get; set; }

        public List<Row> Dijelovi { get; set; }
        public class Row
        {
            public int DioId { get; set; }
            public string Naziv { get; set; }
            public double Cijena { get; set; }
            public string Proizvodjac { get; set; }
            public byte[] Slika { get; set; }
            public int Kolicina { get; internal set; }
            public bool Aktivan { get; set; }
            public string Opis { get; internal set; }
        }


    }

}

