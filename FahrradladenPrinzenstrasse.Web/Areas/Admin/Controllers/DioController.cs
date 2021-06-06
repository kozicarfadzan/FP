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
    public class DioController : Controller
    {
        private readonly MyContext db;

        public DioController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string pretraga)
        {
            PrikaziDioVM Model = new PrikaziDioVM
            {
                Proizvodjaci = db.Proizvodjac
                .Where(x => x.IsDeleted == false)
                .Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.ProizvodjacId.ToString()
                }).ToList()
            };

            return View(Model);
        }

        public ActionResult UcitajListuDijelova(PrikaziDioVM VM)
        {
            PrikaziDioVM Model = new PrikaziDioVM
            {
                Dijelovi = db.Dio.Where(x => x.IsDeleted == false)
               .Where(x => VM.ProizvodjacId == 0 || VM.ProizvodjacId == x.ProizvodjacID)
               .Where(x => VM.Aktivan == x.Aktivan)
               .Where(x => x.Naziv.Contains(VM.Pretraga) || VM.Pretraga == null)
               .Select(
                  x => new PrikaziDioVM.Row
                  {
                      DioId = x.DioId,
                      Naziv = x.Naziv,
                      Cijena = x.Cijena,
                      Proizvodjac = x.Proizvodjac.Naziv,
                      Slika = x.Slika,
                      Kolicina = x.DioStanje.Where(y => y.Aktivan && y.KupacId == null).Count(),
                      Aktivan = x.Aktivan
                  }
               ).ToList()

            };

            return PartialView(Model);
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            DodajDioVM vm = new DodajDioVM();

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajDioVM model, IFormFile Slika)
        {
            if (ModelState.IsValid)
            {
                Dio dio = new Dio()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                    Cijena = model.Cijena,
                    ProizvodjacID = model.ProizvodjacId,
                };

                if (Slika == null || Slika.Length == 0)
                    dio.Slika = new byte[0];
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        Slika.CopyTo(ms);
                        dio.Slika = ms.ToArray();
                    }
                }


                db.Dio.Add(dio);
                db.SaveChanges();
                if (model.DioStanja_Lokacije != null && model.DioStanja_Sifre != null)
                {
                    for (int i = 0; i < model.DioStanja_Lokacije.Count; i++)
                    {
                        DioStanje stanje = new DioStanje
                        {
                            DioId = dio.DioId,
                            LokacijaId = model.DioStanja_Lokacije[i],
                            Sifra = model.DioStanja_Sifre[i]
                        };
                        db.DioStanje.Add(stanje);
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
            DodajDioVM model = db.Dio.Where(x => x.DioId == id)
                .Select(
                x => new DodajDioVM
                {
                    DioId = x.DioId,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Opis = x.Opis,
                    ProizvodjacId = x.ProizvodjacID,
                    Slika = x.Slika,
                    DioStanje = x.DioStanje.Select(y => new DioStanje
                    {
                        DioStanjeId = y.DioStanjeId,
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
        public ActionResult Uredi(UrediDioVM model, IFormFile Slika)
        {
            var dio = db.Dio.Where(x => x.DioId == model.DioId).Include(x => x.DioStanje).FirstOrDefault();

            dio.Naziv = model.Naziv;
            dio.Cijena = model.Cijena;
            dio.Opis = model.Opis;
            dio.ProizvodjacID = model.ProizvodjacId;
           

            if (model.DioStanja_Lokacije != null && model.DioStanja_Sifre != null)
            {
                for (int i = 0; i < model.DioStanja_Sifre.Count; i++)
                {
                    var novoStanje_Sifra = model.DioStanja_Sifre[i];
                    var novoStanje_LokacijaId = model.DioStanja_Lokacije[i];

                    bool pronadjeno = false;
                    foreach (var postojeceStanje in dio.DioStanje)
                    {
                        if (postojeceStanje.Sifra == novoStanje_Sifra)
                        {
                            postojeceStanje.LokacijaId = novoStanje_LokacijaId;
                            pronadjeno = true;
                            break;
                        }
                    }

                    if (!pronadjeno)
                        db.DioStanje.Add(new DioStanje
                        {
                            DioId = dio.DioId,
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
                    dio.Slika = ms.ToArray();
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult DeaktivirajDioStanje(int Id)
        {
            var stanje = db.DioStanje.Find(Id);
            if (stanje != null && stanje.Aktivan == true)
            {
                stanje.Aktivan = false;
                db.SaveChanges();
            }
            return Json("OK");
        }
        [HttpGet]
        public ActionResult AktivirajDioStanje(int Id)
        {
            var stanje = db.DioStanje.Find(Id);
            if (stanje != null && stanje.Aktivan == false)
            {
                stanje.Aktivan = true;
                db.SaveChanges();
            }
            return Json("OK");
        }

        [HttpGet]
        public ActionResult DeaktivirajDio(int Id)
        {
            var Dio = db.Dio.Find(Id);
            if (Dio != null)
            {
                Dio.Aktivan = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult IzbrisiDio(int Id)
        {
            var Dio = db.Dio.Find(Id);
            if (Dio != null)
            {
                //if (db.DioStanje.Where(x => x.DioId == Dio.DioId && x.KupacId != null).Any())
                //{
                //    return new BadRequestResult();
                //}

                Dio.IsDeleted = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AktivirajDio(int Id)
        {
            var Dio = db.Dio.Find(Id);
            if (Dio != null)
            {
                Dio.Aktivan = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}