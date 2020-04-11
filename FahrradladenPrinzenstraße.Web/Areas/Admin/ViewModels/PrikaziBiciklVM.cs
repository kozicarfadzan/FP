using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class PrikaziBiciklVM
    {
        public string Pretraga { get; set; }

        public List<Row> Bicikla { get; set; }
        public List<SelectListItem> Proizvodjaci { get; set; }
        public int ProizvodjacId { get; set; }
        public List<SelectListItem> Stanja { get; set; }
        public int Stanje { get; set; }
        public bool Aktivan { get; set; }

        public class Row
        {
            public int BiciklId { get; set; }
            public Model Model { get; set; }
            public string PuniNaziv => Model.Tip + " " + Model.Naziv;
            public short GodinaProizvodnje { get; set; }
            public Stanje Stanje { get; set; }
            public byte[] Slika { get; set; }
            public double? CijenaPoDanu { get; set; }
            public double? Cijena { get; set; }
            public Boja Boja { get; set; }
            public bool NoznaKocnica { get; set; }
            public int Kolicina { get; internal set; }
            public bool Aktivan { get; set; }
        }
    }
}
