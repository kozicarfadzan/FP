using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Helper;
using FahrradladenPrinzenstraße.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FahrradladenPrinzenstraße.Web.Controllers
{
    public class OpremaController : Controller
    {
        private readonly MyContext db;
        public OpremaController(MyContext db)
        {

            this.db = db;
        }
        public IActionResult Index(PrikaziOpremuVM VM)
        {

            PrikaziOpremuVM Model = new PrikaziOpremuVM
            {
                Proizvodjaci = db.Proizvodjac.ToList(),
                PopularnaOprema = db.Oprema.Where(x => x.OcjenaProizvoda.Any())
                .Where(x => x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(5)
                .Select(x => new PreporuceniProizvod
                {
                    Id = x.OpremaId,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Slika = x.Slika,
                    Tip = TipProizvoda.Oprema
                }).ToList()

            };

            return View(Model);
        }

        public ActionResult UcitajListuOpreme(PrikaziOpremuVM VM)
        {
            PrikaziOpremuVM Model = new PrikaziOpremuVM();

            IQueryable<Oprema> OpremaQry = db.Oprema.Where(x => x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Any()).Where(x => x.Aktivan);

            if (VM.ProizvodjacId != null)
            {
                OpremaQry = OpremaQry.Where(x => VM.ProizvodjacId.Contains(x.ProizvodjacID));
            }


            if (VM.Poredak.HasValue)
            {
                switch (VM.Poredak.Value)
                {
                    case 1: OpremaQry = OpremaQry.OrderBy(x => x.Naziv); break;
                    case 2: OpremaQry = OpremaQry.OrderByDescending(x => x.Naziv); break;
                    case 3: OpremaQry = OpremaQry.OrderByDescending(x => x.Cijena); break;
                    case 4: OpremaQry = OpremaQry.OrderBy(x => x.Cijena); break;
                }
            }

            Model.PagedResult = OpremaQry.Select(
              x => new PrikaziOpremuVM.Row
              {
                  OpremaId = x.OpremaId,
                  Naziv = x.Naziv,

                  Cijena = x.Cijena,


                  Slika = x.Slika,
                  Opis = x.Opis,
                  Kolicina = x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Count(),
                  Aktivan = x.Aktivan,
                  Proizvodjac = x.Proizvodjac
              }
           ).GetPaged(VM.Page, 6);

            if (VM.PrikaziKaoListu)
                return PartialView("UcitajListuOpremeLista", Model);

            return PartialView(Model);

        }
    }
}