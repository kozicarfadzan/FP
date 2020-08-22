using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FahrradladenPrinzenstraße.Web.Areas.Zaposlenik.Controllers
{
    
        [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
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
                    Proizvodjaci = db.Proizvodjac.Select(x => new SelectListItem
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
                          Slika = x.Slika,
                          Stanje = x.Stanje,
                          Kolicina = x.BiciklStanje.Where(y => y.Aktivan).Count(),
                          Aktivan = x.Aktivan
                      }
                   ).ToList()

                };

                return PartialView(Model);
            }
        }
}