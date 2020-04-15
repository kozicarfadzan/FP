using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.EntityFrameworkCore;
using FahrradladenPrinzenstraße.Data.EntityModels;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.Controllers
{
    [Area("Admin")] [Autorizacija(administrator:true)]
    public class ZaposlenikController : Controller
    {
        private readonly MyContext db;

        public ZaposlenikController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            PrikaziZaposlenikVM Model = new PrikaziZaposlenikVM
            {
                Korisnici = db.Korisnik
                .Include(x => x.Zaposlenik)
                .Include(x => x.Grad)
                .ToList()
            };
            return View(Model);


        }
        private void PripremiStavke(DodajZaposlenikaVM Model)
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
            Model.Gradovi.AddRange(db.Grad.Select(x => new SelectListItem()
            {
                Value = x.GradID.ToString(),
                Text = x.Naziv
            }));


        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            DodajZaposlenikaVM vm = new DodajZaposlenikaVM();

            PripremiStavke(vm);

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajZaposlenikaVM model)
        {
            if (ModelState.IsValid)
            {
                Korisnik zaposlenik = new Korisnik()
                {
                    Ime = model.Ime,
                    Prezime = model.Prezime,
                    AdresaStanovanja = model.AdresaStanovanja,
                    Aktivan = model.Aktivan,
                    BrojTelefona = model.BrojTelefona,
                    Email = model.Email,
                    GradID = model.GradId,
                    KorisnickoIme = model.KorisnickoIme,
                    Spol = model.Spol,
                    Zaposlenik = new Zaposlenik { }
                };
                zaposlenik.SetLozinka(model.Lozinka);

                db.Korisnik.Add(zaposlenik);
                db.SaveChanges();
            }
            else
            {
                PripremiStavke(model);

                return View("Dodaj", model);
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            var korisnik = db.Korisnik.Where(x => x.KorisnikID == id).Include(x => x.Zaposlenik).FirstOrDefault();
            if (korisnik == null)
                return RedirectToAction("Index");

            UrediZaposlenikaVM model = new UrediZaposlenikaVM(db, korisnik);
            return View(model);
        }
        [HttpPost]
        public ActionResult Uredi(UrediZaposlenikaVM model)
        {
            var korisnik = db.Korisnik.Where(x=>x.KorisnikID == model.KorisnikId).Include(x=>x.Zaposlenik).FirstOrDefault();

            bool izmjenaLozinke = false;
            if (!string.IsNullOrEmpty(model.Lozinka) && model.Lozinka.Equals(model.LozinkaPotvrda))
            {
                izmjenaLozinke = true;
            }

            if (ModelState.IsValid)
            {
                korisnik.Ime = model.Ime;
                korisnik.Prezime = model.Prezime;
                korisnik.AdresaStanovanja = model.AdresaStanovanja;
                korisnik.BrojTelefona = model.BrojTelefona;
                korisnik.Email = model.Email;
                korisnik.KorisnickoIme = model.KorisnickoIme;
                korisnik.Aktivan = model.Aktivan;
                korisnik.Spol = model.Spol;
                korisnik.GradID = model.GradID;
                if (izmjenaLozinke)
                    korisnik.SetLozinka(model.Lozinka);

                db.SaveChanges();
            }
            else
            {
                model = new UrediZaposlenikaVM(db, korisnik);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Aktivan(int id)
        {

            var zaposlenik = db.Korisnik.Find(id);
            zaposlenik.Aktivan = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult NeAktivan(int id)
        {

            var zaposlenik = db.Korisnik.Find(id);
            zaposlenik.Aktivan = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}