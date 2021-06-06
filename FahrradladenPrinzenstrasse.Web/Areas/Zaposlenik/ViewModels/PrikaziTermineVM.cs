using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels
{
    public class PrikaziTermineVM
    {
        public List<RezervacijaServis> Termini { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumOd { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumDo { get; set; }
        public int KlijentId { get; set; }
        public List<SelectListItem> Klijenti { get; set; }
    }
}
