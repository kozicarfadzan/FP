using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels
{
    public class PosaljiSMSVM
    {
        public List<Korisnik> Klijenti { get; set; }
        [DisplayName("Klijenti")]
        public List<int> OdabraniKlijenti { get; set; }

        public string Sadrzaj { get; set; }

    }
}
