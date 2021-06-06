using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels
{
    public class KupiBiciklVM
    {
        public Bicikl Bicikl { get; set; }
        public int KolicinaNaStanju { get; set; }
    }
}
