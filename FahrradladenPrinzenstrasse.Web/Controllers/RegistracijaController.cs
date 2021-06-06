using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using FahrradladenPrinzenstrasse.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FahrradladenPrinzenstrasse.Web.Controllers
{
    public class RegistracijaController : Controller
    {
        private readonly MyContext _db;

        public RegistracijaController(MyContext db)
        {
            _db = db;
        }

        private void PripremiStavke(RegistracijaVM Model)
        {
            Model.Spolovi = new List<SelectListItem>
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
            Model.Gradovi = new List<SelectListItem>();
            Model.Gradovi.AddRange(_db.Grad
            .Where(x => x.IsDeleted == false)
            .Select(x => new SelectListItem()
            {
                Value = x.GradID.ToString(),
                Text = x.Naziv
            }));


        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            RegistracijaVM vm = new RegistracijaVM();

            PripremiStavke(vm);

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(RegistracijaVM model)
        {
            if(model.Lozinka != model.LozinkaPotvrda)
                ModelState.AddModelError("LozinkaPotvrda", "Lozinke se ne podudaraju.");

            if (ModelState.IsValid)
            {

                Korisnik klijent = new Korisnik()
                {
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    AdresaStanovanja = model.AdresaStanovanja,
                    Aktivan =true,
                    BrojTelefona = model.BrojTelefona,
                    Email = model.Email,
                    GradID = model.GradId,
                    KorisnickoIme = model.KorisnickoIme,
                    Spol = model.Spol,
                                           
                    Klijent = new Data.EntityModels.Klijent { }
                };
                klijent.SetLozinka(model.Lozinka);

                _db.Korisnik.Add(klijent);
                _db.SaveChanges();
            }
            else
            {
                PripremiStavke(model);

                return View("Dodaj", model);
            }


            return RedirectToAction("Index","Autentifikacija");
        }
    }
}