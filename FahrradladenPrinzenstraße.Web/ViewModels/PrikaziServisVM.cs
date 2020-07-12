using FahrradladenPrinzenstraße.Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.ViewModels
{
    public class PrikaziServisVM
    {
        public string Pretraga { get; set; }
        public bool PrikaziKaoListu { get; set; }

        public PagedResult<Row> PagedResult { get; set; }
        public int Page { get; set; } = 1;

 
        public class Row
        {
            public int ServisId { get; set; }
            public string Naziv { get; set; }
            public double Cijena { get; set; }
            public string Opis { get; set; }
            public double Trajanje { get; set; }
        }
    }
}
