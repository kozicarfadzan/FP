 using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FahrradladenPrinzenstrasse.Web.Helper;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Autorizacija(administrator: true)]
    public class BiciklController : Controller
    {
        private readonly MyContext db;

        public BiciklController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(PrikaziBiciklVM VM)
        {
            PrikaziBiciklVM Model = new PrikaziBiciklVM
            {
                Proizvodjaci = db.Proizvodjac
                .Where(x => x.IsDeleted == false).Select(x => new SelectListItem
                {
                    Text = x.Naziv,
                    Value = x.ProizvodjacId.ToString()
                }).ToList(),
                Stanja = Enum.GetValues(typeof(Stanje)).Cast<Stanje>().Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString()
                }).ToList()
            };
            return View(Model);
        }

        public ActionResult UcitajListuBicikala(PrikaziBiciklVM VM)
        {
            PrikaziBiciklVM Model = new PrikaziBiciklVM
            {
                Bicikla = db.Bicikl
               .Where(x => VM.Stanje == 0 || (Stanje)VM.Stanje == x.Stanje)
               .Where(x => VM.ProizvodjacId == 0 || VM.ProizvodjacId == x.Model.ProizvodjacId)
               .Where(x => VM.Aktivan == x.Aktivan)
               .Select(
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
                      //Slika = x.Slika,
                      Stanje = x.Stanje,
                      Kolicina = x.BiciklStanje.Where(y => y.Aktivan).Count(),
                      Aktivan = x.Aktivan
                  }
               ).ToList()

            };

            return PartialView(Model);
        }


        [HttpGet]
        public IActionResult Dodaj()
        {
            DodajBiciklVM vm = new DodajBiciklVM();
            vm.GodinaProizvodnje = (short)DateTime.Now.Year;

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
                    Stanje = model.Stanje,
                    Opis = model.Opis,
                    Aktivan = true
                };

                db.Bicikl.Add(NoviBicikl);

                if (Slika == null || Slika.Length == 0)
                    NoviBicikl.Slika = new byte[0];
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        Slika.CopyTo(ms);
                        NoviBicikl.Slika = ms.ToArray();
                    }
                }

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

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            UrediBiciklVM model = db.Bicikl.Where(x => x.BiciklId == id)
                .Select(
                x => new UrediBiciklVM
                {
                    BiciklId = x.BiciklId,
                    BojaId = x.BojaId,
                    StarosnaGrupaId = x.StarosnaGrupaId,
                    VelicinaOkviraId = x.VelicinaOkviraId,
                    Cijena = x.Cijena,
                    CijenaPoDanu = x.CijenaPoDanu,
                    GodinaProizvodnje = x.GodinaProizvodnje,
                    ModelId = x.ModelId,
                    Stanje = x.Stanje,
                    Slika = x.Slika,
                    NoznaKocnica = x.NoznaKocnica,
                    Opis = x.Opis,
                    BiciklStanje = x.BiciklStanje.Select(y => new BiciklStanje
                    {
                        BiciklStanjeId = y.BiciklStanjeId,
                        LokacijaId = y.LokacijaId,
                        Sifra = y.Sifra,
                        Aktivan = y.Aktivan,
                        KupacId = y.KupacId
                    }).ToList()
                })
                .FirstOrDefault();

            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpGet]
        public ActionResult DeaktivirajBiciklStanje(int Id)
        {
            var stanje = db.BiciklStanje.Find(Id);
            if (stanje != null && stanje.Aktivan == true)
            {
                stanje.Aktivan = false;
                db.SaveChanges();
            }
            return Json("OK");
        }
        [HttpGet]
        public ActionResult AktivirajBiciklStanje(int Id)
        {
            var stanje = db.BiciklStanje.Find(Id);
            if (stanje != null && stanje.Aktivan == false)
            {
                stanje.Aktivan = true;
                db.SaveChanges();
            }
            return Json("OK");
        }

        [HttpGet]
        public ActionResult DeaktivirajBicikl(int Id)
        {
            var bicikl = db.Bicikl.Find(Id);
            if (bicikl != null)
            {
                bicikl.Aktivan = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult AktivirajBicikl(int Id)
        {
            var bicikl = db.Bicikl.Find(Id);
            if (bicikl != null)
            {
                bicikl.Aktivan = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult Uredi(UrediBiciklVM model, IFormFile Slika)
        {
            var bicikl = db.Bicikl.Where(x => x.BiciklId == model.BiciklId).Include(x => x.BiciklStanje).FirstOrDefault();

            bicikl.BojaId = model.BojaId;
            bicikl.StarosnaGrupaId = model.StarosnaGrupaId;
            bicikl.VelicinaOkviraId = model.VelicinaOkviraId;
            bicikl.Cijena = model.Cijena;
            bicikl.CijenaPoDanu = model.CijenaPoDanu;
            bicikl.GodinaProizvodnje = model.GodinaProizvodnje;
            bicikl.ModelId = model.ModelId;
            bicikl.Stanje = model.Stanje;
            bicikl.NoznaKocnica = model.NoznaKocnica;
            bicikl.Opis = model.Opis;

            if (model.BiciklStanja_Lokacije != null && model.BiciklStanja_Sifre != null)
            {
                for (int i = 0; i < model.BiciklStanja_Sifre.Count; i++)
                {
                    var novoStanje_Sifra = model.BiciklStanja_Sifre[i];
                    var novoStanje_LokacijaId = model.BiciklStanja_Lokacije[i];

                    bool pronadjeno = false;
                    foreach (var postojeceStanje in bicikl.BiciklStanje)
                    {
                        if (postojeceStanje.Sifra == novoStanje_Sifra)
                        {
                            postojeceStanje.LokacijaId = novoStanje_LokacijaId;
                            pronadjeno = true;
                            break;
                        }
                    }

                    if (!pronadjeno)
                        db.BiciklStanje.Add(new BiciklStanje
                        {
                            BiciklId = bicikl.BiciklId,
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
                    bicikl.Slika = ms.ToArray();
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}