using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class DodajOpremuVM
    {
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Polje Opis je obavezno.")]
        [StringLength(maximumLength: 1000, MinimumLength = 10, ErrorMessage = "Minimalno 10 karaktera.")]
        public string Opis { get; set; }
        public double Cijena { get; set; }
        [Required]
        public int ProizvodjacId { get; set; }


        public List<SelectListItem> Proizvodjaci { get; set; }
    }
}
