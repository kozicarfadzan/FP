using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class IzvjestajInventarPrikazVM
    {
        public List<InventarStavka> Stavke { get; set; }
        public int NacinPrikaza { get; set; }
        public class InventarStavka
        {
            public string Šifra { get; set; }
            public string VrstaStavke { get; set; }
            public string Naziv { get; set; }
            public double Cijena { get; set; }
            public int KolicinaNaStanju { get; set; }
        }
    }
}
