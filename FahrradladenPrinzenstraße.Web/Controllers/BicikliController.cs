using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Controllers
{
    public class BicikliController : Controller
    {
        private readonly MyContext db;

        public BicikliController(MyContext db)
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
                Stanja = Enum.GetValues(typeof(Stanje)).Cast<Stanje>().Where(x => x != Stanje.Korišteno).Select(x => new SelectListItem
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
               .Where(x => VM.Stanje == 0 || VM.Stanje == (int)x.Stanje)
               .Where(x => VM.ProizvodjacId == 0 || VM.ProizvodjacId == x.Model.ProizvodjacId)
               .Where(x => x.BiciklStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Any())
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
                      Kolicina = x.BiciklStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Count(),
                      Aktivan = x.Aktivan /*&& (x.BiciklStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaBicikla.Count() == 0).Count() - HttpContext.GetLogiraniKorisnik() > 0)*/
                  }
               ).ToList()

            };

            return PartialView(Model);
        }

    }
}
