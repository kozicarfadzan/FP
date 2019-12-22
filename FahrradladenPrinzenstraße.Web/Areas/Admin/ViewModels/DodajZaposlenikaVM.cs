using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class DodajZaposlenikaVM
    {
       
        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        public string Lozinka { get; set; }
        [Required]
        public string LozinkaPotvrda { get; set; }

        [Required]
        public bool Aktivan { get; set; }

        public string AdresaStanovanja { get; set; }
        public string BrojTelefona { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Spol { get; set; }
        public List<SelectListItem> Spolovi { get; set; }
       
        [Required]
        public int GradId { get; set; }


        public List<SelectListItem> Gradovi { get; set; }
    }
}
