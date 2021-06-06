using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class DodajZaposlenikaVM
    {

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        [Display(Name = "Korisničko Ime")]
        public string KorisnickoIme { get; set; }

        [Required]
        public string Lozinka { get; set; }
        [Required]
        [Display(Name = "Potvrda Lozinke")]
        public string LozinkaPotvrda { get; set; }

        public bool Aktivan { get; set; } = true;

        [Display(Name = "Adresa Stanovanja")]
        public string AdresaStanovanja { get; set; }
        [Display(Name = "Broj Telefona")]
        public string BrojTelefona { get; set; }
        [Display(Name = "Email Adresa")]
        public string Email { get; set; }
        public string Spol { get; set; }
        public List<SelectListItem> Spolovi { get; set; }

        [Display(Name = "Grad")]
        [Required]
        public int GradId { get; set; }


        public List<SelectListItem> Gradovi { get; set; }
    }
}
