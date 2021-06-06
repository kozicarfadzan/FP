using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.ViewModels
{
    public class RegistracijaVM
    {
        public const string RULE_ALPHA_REGULAR = @"^[A-Za-z]{3,}$";
        public const string RULE_ALPHANUM_REGULAR = @"[A-Za-z0-9]{3,}$";
        public const string RULE_ADRESA = @"^(?=.*[A-Za-z])[A-Za-z0-9 \-.]{3,}$";
        public const string RULE_PASSWORD = @"^[a-zA-Z0-9.\-#_?*]{3,}$";
        public const string RULE_TELEFON = @"^[\d ]{6,12}$";

        [Required]
        [RegularExpression(RULE_ALPHA_REGULAR, ErrorMessage = "Ime ne smije biti kraće od 3 karaktera i smije sadržati samo slova.")]
        public string Ime { get; set; }

        [Required]
        [RegularExpression(RULE_ALPHA_REGULAR, ErrorMessage = "Prezime ne smije biti kraće od 3 karaktera i smije sadržati samo slova.")]
        public string Prezime { get; set; }

        [Required]
        [RegularExpression(RULE_ALPHANUM_REGULAR, ErrorMessage = "Korisničko ime ne smije biti kraće od 3 karaktera i smije sadržati samo slova i brojeve.")]
        [Display(Name = "Korisničko Ime")]
        public string KorisnickoIme { get; set; }

        [Required]
        [RegularExpression(RULE_PASSWORD, ErrorMessage = "Polje password ne smije biti kraće od 3 karaktera i smije sadržati samo slova, brojeve i znakove .-#_?*")]
        public string Lozinka { get; set; }

        [Required]
        [Display(Name = "Potvrda Lozinke")]
        public string LozinkaPotvrda { get; set; }

        public bool Aktivan { get; set; } = true;

        [Display(Name = "Adresa Stanovanja")]
        [RegularExpression(RULE_ADRESA, ErrorMessage = "Adresa ne smije biti kraća od 3 karaktera i smije sadržati samo slova, brojeve  i znakove .-")]
        public string AdresaStanovanja { get; set; }

        [Display(Name = "Broj Telefona")]
        [RegularExpression(RULE_TELEFON, ErrorMessage = "Telefon ne smije biti kraći od 6 ili duže od 12 karaktera i smije sadržati samo brojeve!")]
        public string BrojTelefona { get; set; }

        [Required]
        [Display(Name = "Email Adresa")]
        [EmailAddress(ErrorMessage = "Email adresa nije u ispravnom formatu.")]
        public string Email { get; set; }
        public string Spol { get; set; }
        public List<SelectListItem> Spolovi { get; set; }

        [Display(Name = "Grad")]
        [Required]
        public int GradId { get; set; }


        public List<SelectListItem> Gradovi { get; set; }
    }
}
