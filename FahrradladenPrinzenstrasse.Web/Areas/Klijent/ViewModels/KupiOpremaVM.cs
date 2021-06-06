using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels
{
    public class KupiOpremaVM
    {
        public Oprema Oprema { get; set; }
        public int KolicinaNaStanju { get; set; }
    }
}
