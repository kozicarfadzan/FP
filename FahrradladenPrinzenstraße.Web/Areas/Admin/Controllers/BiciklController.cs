using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BiciklController : Controller
    {
        private readonly MyContext db;

        public BiciklController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            PrikaziBiciklVM Model = new PrikaziBiciklVM
            {
                Bicikla = db.Bicikl.Select(
                   x => new PrikaziBiciklVM.Row
                   {
                       BiciklId = x.BiciklId,
                       Boja = x.Boja,
                       Cijena = x.Cijena,
                       CijenaPoDanu = x.CijenaPoDanu,
                       GodinaProizvodnje = x.GodinaProizvodnje,
                       Model = new Data.EntityModels.Model
                       {
                           Naziv = x.Model.Naziv,
                           Proizvodjac = x.Model.Proizvodjac
                       },
                       NoznaKocnica = x.NoznaKocnica,
                       Slika = x.Slika,
                       Stanje = x.Stanje,
                   }
                ).ToList()
            };
            return View(Model);


        }


        [HttpGet]
        public IActionResult Dodaj()
        {
            DodajBiciklVM vm = new DodajBiciklVM();

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajBiciklVM model, IFormFile Slika)
        {
            if (ModelState.IsValid)
            {
                Bicikl NoviBicikl = new Bicikl()
                {
                    BojaId = model.BojaId,
                    Cijena = model.Cijena,
                    CijenaPoDanu = model.CijenaPoDanu,
                    GodinaProizvodnje = model.GodinaProizvodnje,
                    ModelId = model.ModelId,
                    NoznaKocnica = model.NoznaKocnica,
                    Slika = model.Slika,
                    Stanje = model.Stanje
                };

                db.Bicikl.Add(NoviBicikl);
                db.SaveChanges();

                if (model.BiciklStanja_Lokacije != null && model.BiciklStanja_Sifre != null)
                {
                    for (int i = 0; i < model.BiciklStanja_Lokacije.Count; i++)
                    {
                        BiciklStanje stanje = new BiciklStanje
                        {
                            BiciklId = NoviBicikl.BiciklId,
                            LokacijaId = model.BiciklStanja_Lokacije[i],
                            Sifra = model.BiciklStanja_Sifre[i]
                        };
                        db.BiciklStanje.Add(stanje);
                    }
                    db.SaveChanges();
                }

            }
            else
            {
                return View("Dodaj", model);
            }


            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public IActionResult Uredi(int id)
        //{
        //    var korisnik = db.Korisnik.Where(x => x.KorisnikID == id).Include(x => x.Bicikl).FirstOrDefault();
        //    if (korisnik == null)
        //        return RedirectToAction("Index");

        //    UrediBiciklaVM model = new UrediBiciklaVM(db, korisnik);
        //    return View(model);
        //}
        //[HttpPost]
        //public ActionResult Uredi(UrediBiciklaVM model)
        //{
        //    var korisnik = db.Korisnik.Where(x => x.KorisnikID == model.KorisnikId).Include(x => x.Bicikl).FirstOrDefault();

        //    bool izmjenaLozinke = false;
        //    if (!string.IsNullOrEmpty(model.Lozinka) && model.Lozinka.Equals(model.LozinkaPotvrda))
        //    {
        //        izmjenaLozinke = true;
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        korisnik.Ime = model.Ime;
        //        korisnik.Prezime = model.Prezime;
        //        korisnik.AdresaStanovanja = model.AdresaStanovanja;
        //        korisnik.BrojTelefona = model.BrojTelefona;
        //        korisnik.Email = model.Email;
        //        korisnik.KorisnickoIme = model.KorisnickoIme;
        //        korisnik.Aktivan = model.Aktivan;
        //        if (izmjenaLozinke)
        //            korisnik.SetLozinka(model.Lozinka);

        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        model = new UrediBiciklaVM(db, korisnik);
        //        return View(model);
        //    }

        //    return RedirectToAction("Index");
        //}



    }
}