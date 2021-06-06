using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class DodajServisVM
    {
 
        [Required(ErrorMessage = "Polje Naziv je obavezno.")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Polje Opis je obavezno.")]
        [StringLength(maximumLength: 1000, MinimumLength = 10, ErrorMessage = "Minimalno 10 karaktera.")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Polje Cijena je obavezno.")]
        [Range(1, 10000, ErrorMessage = "Cijena nije unesena ispravno.")]
        public double Cijena { get; set; }
        [Range(0.5, 4, ErrorMessage = "Trajanje mora biti između 0.5 i 4 sata.")]
        public double Trajanje { get; set; }
    }
}
