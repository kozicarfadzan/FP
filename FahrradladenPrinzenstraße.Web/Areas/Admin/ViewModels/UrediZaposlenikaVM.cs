using FahrradladenPrinzenstraße.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class UrediZaposlenikaVM
    {
        public UrediZaposlenikaVM() {
        }

        public UrediZaposlenikaVM(MyContext db, Data.EntityModels.Korisnik korisnik)
        {
            Gradovi = db.Grad.Select(g => new SelectListItem()
            {
                Value = g.GradID.ToString(),
                Text = g.Naziv
            }).ToList();

            Spolovi = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value="M",
                    Text="Muski",
                    Selected = korisnik.Spol == "M"
                },
                 new SelectListItem()
                {
                    Value="Z",
                    Text="Zenski",
                    Selected = korisnik.Spol == "Z"
                }
            };

            KorisnikId = korisnik.KorisnikID;
            Ime = korisnik.Ime;
            Prezime = korisnik.Prezime;
            AdresaStanovanja = korisnik.AdresaStanovanja;
            BrojTelefona = korisnik.BrojTelefona;
            Email = korisnik.Email;
            KorisnickoIme = korisnik.KorisnickoIme;

            Aktivan = korisnik.Aktivan;
            GradID = korisnik.GradID;
        }
        public int KorisnikId { get; set; }
        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        [Display(Name = "Korisničko Ime")]
        public string KorisnickoIme { get; set; }

        public string Lozinka { get; set; }
        [Display(Name = "Potvrda Lozinke")]
        public string LozinkaPotvrda { get; set; }

        public bool Aktivan { get; set; }

        [Display(Name = "Adresa Stanovanja")]
        public string AdresaStanovanja { get; set; }
        [Display(Name = "Broj Telefona")]
        public string BrojTelefona { get; set; }
        [Display(Name = "Email Adresa")]
        public string Email { get; set; }
        public string Spol { get; set; }
        public List<SelectListItem> Spolovi { get; set; }

        [Display(Name = "Grad")]
        public int GradID { get; set; }
        public List<SelectListItem> Gradovi { get; set; }
 
       
    }
}
