using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstraße.Model.Requests
{
    public class KorisnikInsertRequest
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
        public string KorisnickoIme
        {
            get; set;
        }
        [Required]
        [MinLength(4)]
        public string Lozinka
        {
            get; set;
        }
        [Required]
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
