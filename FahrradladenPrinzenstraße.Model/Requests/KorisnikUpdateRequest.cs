using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstraße.Model.Requests
{
    public class KorisnikUpdateRequest
    {
        [Required]
        public string Ime
        {
            get; set;
        }
        [Required]
        public string Prezime
        {
            get; set;
        }
        [Required]
        [RegularExpression(@"[A-Za-z0-9_]+", ErrorMessage = "Korisničko ime može sadržavati samo slova, brojeve and donje crtice.")]
        public string KorisnickoIme
        {
            get; set;
        }
        [MinLength(4)]
        public string Lozinka
        {
            get; set;
        }
        public string LozinkaPotvrda
        {
            get; set;
        }
        public string AdresaStanovanja
        {
            get; set;
        }
        public string BrojTelefona
        {
            get; set;
        }

        [Required]
        [EmailAddress]
        public string Email
        {
            get; set;
        }
        [Required]
        public string Spol
        {
            get; set;
        }
        [Required]
        public int GradID
        {
            get; set;
        }
    }
}
