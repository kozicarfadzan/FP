using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Autorizacija(administrator: true)]
    public class OpremaController : Controller
    {
        private readonly MyContext db;

        public OpremaController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string pretraga)
        {
            PrikaziOpremuVM Model = new PrikaziOpremuVM
            {
                Proizvodjaci = db.Proizvodjac
                .Where(x => x.IsDeleted == false).Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.ProizvodjacId.ToString()
                }).ToList()
            };

            return View(Model);
        }

        public ActionResult UcitajListuOpreme(PrikaziOpremuVM VM)
        {
            PrikaziOpremuVM Model = new PrikaziOpremuVM
            {
                Oprema = db.Oprema.Where(x => x.IsDeleted == false)
               .Where(x => VM.ProizvodjacId == 0 || VM.ProizvodjacId == x.ProizvodjacID)
               .Where(x => VM.Aktivan == x.Aktivan)
               .Where(x => x.Naziv.Contains(VM.Pretraga) || VM.Pretraga == null)
               .Select(
                  x => new PrikaziOpremuVM.Row
                  {
                      OpremaId = x.OpremaId,
                      Naziv = x.Naziv,
                      Cijena = x.Cijena,
                      Proizvodjac = x.Proizvodjac.Naziv,
                      Slika = x.Slika,
                      Kolicina = x.OpremaStanje.Where(y => y.Aktivan && y.KupacId == null).Count(),
                      Aktivan = x.Aktivan
                  }
               ).ToList()

            };

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            DodajOpremuVM vm = new DodajOpremuVM();

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajOpremuVM model, IFormFile Slika)
        {
            if (ModelState.IsValid)
            {
                Oprema Oprema = new Oprema()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                    Cijena = model.Cijena,
                    ProizvodjacID = model.ProizvodjacId,
                };

                if (Slika == null || Slika.Length == 0)
                    Oprema.Slika = new byte[0];
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        Slika.CopyTo(ms);
                        Oprema.Slika = ms.ToArray();
                    }
                }


                db.Oprema.Add(Oprema);
                db.SaveChanges();
                if (model.OpremaStanja_Lokacije != null && model.OpremaStanja_Sifre != null)
                {
                    for (int i = 0; i < model.OpremaStanja_Lokacije.Count; i++)
                    {
                        OpremaStanje stanje = new OpremaStanje
                        {
                            OpremaId = Oprema.OpremaId,
                            LokacijaId = model.OpremaStanja_Lokacije[i],
                            Sifra = model.OpremaStanja_Sifre[i]
                        };
                        db.OpremaStanje.Add(stanje);
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

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            DodajOpremuVM model = db.Oprema.Where(x => x.OpremaId == id)
                .Select(
                x => new DodajOpremuVM
                {
                    OpremaId = x.OpremaId,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Opis = x.Opis,
                    ProizvodjacId = x.ProizvodjacID,
                    Slika = x.Slika,
                    OpremaStanje = x.OpremaStanje.Select(y => new OpremaStanje
                    {
                        OpremaStanjeId = y.OpremaStanjeId,
                        LokacijaId = y.LokacijaId,
                        Sifra = y.Sifra,
                        KupacId = y.KupacId,
                        Aktivan = y.Aktivan
                    }).ToList()
                })
                .FirstOrDefault();

            if (model == null)
                return RedirectToAction("Index");

            return View("Dodaj", model);

        }



        [HttpPost]
        public ActionResult Uredi(UrediOpremuVM model, IFormFile Slika)
        {
            var Oprema = db.Oprema.Where(x => x.OpremaId == model.OpremaId).Include(x => x.OpremaStanje).FirstOrDefault();

            Oprema.Naziv = model.Naziv;
            Oprema.Cijena = model.Cijena;
            Oprema.Opis = model.Opis;
            Oprema.ProizvodjacID = model.ProizvodjacId;

            if (model.OpremaStanja_Lokacije != null && model.OpremaStanja_Sifre != null)
            {
                for (int i = 0; i < model.OpremaStanja_Sifre.Count; i++)
                {
                    var novoStanje_Sifra = model.OpremaStanja_Sifre[i];
                    var novoStanje_LokacijaId = model.OpremaStanja_Lokacije[i];

                    bool pronadjeno = false;
                    foreach (var postojeceStanje in Oprema.OpremaStanje)
                    {
                        if (postojeceStanje.Sifra == novoStanje_Sifra)
                        {
                            postojeceStanje.LokacijaId = novoStanje_LokacijaId;
                            pronadjeno = true;
                            break;
                        }
                    }

                    if (!pronadjeno)
                        db.OpremaStanje.Add(new OpremaStanje
                        {
                            OpremaId = Oprema.OpremaId,
                            Sifra = novoStanje_Sifra,
                            LokacijaId = novoStanje_LokacijaId
                        });
                }
            }


            if (Slika?.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    Slika.CopyTo(ms);
                    Oprema.Slika = ms.ToArray();
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult DeaktivirajOpremaStanje(int Id)
        {
            var stanje = db.OpremaStanje.Find(Id);
            if (stanje != null && stanje.Aktivan == true)
            {
                stanje.Aktivan = false;
                db.SaveChanges();
            }
            return Json("OK");
        }
        [HttpGet]
        public ActionResult AktivirajOpremaStanje(int Id)
        {
            var stanje = db.OpremaStanje.Find(Id);
            if (stanje != null && stanje.Aktivan == false)
            {
                stanje.Aktivan = true;
                db.SaveChanges();
            }
            return Json("OK");
        }

        [HttpGet]
        public ActionResult DeaktivirajOpremu(int Id)
        {
            var Oprema = db.Oprema.Find(Id);
            if (Oprema != null)
            {
                Oprema.Aktivan = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult IzbrisiOpremu(int Id)
        {
            var Oprema = db.Oprema.Find(Id);
            if (Oprema != null)
            {
                //if (db.OpremaStanje.Where(x => x.OpremaId == Oprema.OpremaId && x.KupacId != null).Any())
                //{
                //    return new BadRequestResult();
                //}

                Oprema.IsDeleted = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AktivirajOpremu(int Id)
        {
            var Oprema = db.Oprema.Find(Id);
            if (Oprema != null)
            {
                Oprema.Aktivan = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}