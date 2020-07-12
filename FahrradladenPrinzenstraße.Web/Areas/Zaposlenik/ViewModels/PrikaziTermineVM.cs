using FahrradladenPrinzenstraße.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Zaposlenik.ViewModels
{
    public class PrikaziTermineVM
    {
        public List<RezervacijaServis> Termini { get; set; }
        public DateTime? Datum { get; set; }
        public string Klijent { get; set; }
    }
}
