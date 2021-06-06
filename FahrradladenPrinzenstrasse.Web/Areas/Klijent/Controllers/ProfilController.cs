using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class ProfilController : Controller
    {

        private readonly MyContext db;

        public ProfilController(MyContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            PrikaziProfilVM Model = new PrikaziProfilVM
            {

                Korisnici = db.Korisnik
                .Include(x => x.Klijent).Where(x => x.KorisnikID == Klijent.Id)
                .Include(x => x.Grad)
                .ToList()
            };
            return View(Model);


        }

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            var korisnik = db.Korisnik.Where(x => x.KorisnikID == id).Include(x => x.Klijent).FirstOrDefault();


            UrediProfilVM model = new UrediProfilVM(db, korisnik);
            return View(model);
        }
        [HttpPost]
        public ActionResult Uredi(UrediProfilVM model)
        {
            var korisnik = db.Korisnik.Where(x => x.KorisnikID == model.KorisnikId).Include(x => x.Klijent).FirstOrDefault();

            bool izmjenaLozinke = false;
            if (!string.IsNullOrEmpty(model.Lozinka) && model.Lozinka.Equals(model.LozinkaPotvrda))
            {
                izmjenaLozinke = true;
            }

            if (ModelState.IsValid)
            {
                model.Spolovi = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value="M",
                    Text="Muski"

                },
                 new SelectListItem()
                {
                    Value="Z",
                    Text="Zenski"

                }
            };
                model.Gradovi = new List<SelectListItem>();
                model.Gradovi.AddRange(db.Grad
                .Where(x => x.IsDeleted == false)
                .Select(x => new SelectListItem()
                {
                    Value = x.GradID.ToString(),
                    Text = x.Naziv
                }));

                korisnik.Ime = model.Ime;
                korisnik.Prezime = model.Prezime;
                korisnik.AdresaStanovanja = model.AdresaStanovanja;
                korisnik.BrojTelefona = model.BrojTelefona;
                korisnik.Email = model.Email;
                korisnik.KorisnickoIme = model.KorisnickoIme;
                korisnik.Aktivan = true;
                korisnik.Spol = model.Spol;
                korisnik.GradID = model.GradID;
                if (izmjenaLozinke)
                    korisnik.SetLozinka(model.Lozinka);

                db.SaveChanges();
            }
            else
            {
                model = new UrediProfilVM(db, korisnik);
                return View(model);
            }

            return RedirectToAction("Index");
        }


    }
}