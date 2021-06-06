using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using FahrradladenPrinzenstrasse.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FahrradladenPrinzenstrasse.Web.Controllers
{
    public class DijeloviController : Controller
    {
        private readonly MyContext db;
        public DijeloviController(MyContext db) {

            this.db = db;
        }
        public IActionResult Index(PrikaziDijeloveVM VM)
        {

            PrikaziDijeloveVM Model = new PrikaziDijeloveVM
            {
                Proizvodjaci=db.Proizvodjac
                .Where(x => x.IsDeleted == false).ToList(),
                PopularniDijelovi = db.Dio.Where(x => x.IsDeleted == false).Where(x => x.OcjenaProizvoda.Any())
                .Where(x => x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(5)
                .Select(x => new PreporuceniProizvod
                {
                    Id = x.DioId,
                    Naziv = x.Naziv,
                    Cijena = x.Cijena,
                    Slika = x.Slika,
                    Tip = TipProizvoda.Dio
                }).ToList()
            };

            return View(Model);
        }

        public ActionResult UcitajListuDijelova(PrikaziDijeloveVM VM)
        {
            PrikaziDijeloveVM Model = new PrikaziDijeloveVM();

            IQueryable<Dio> DioQry = db.Dio.Where(x => x.IsDeleted == false).Where(x => x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Any()).Where(x => x.Aktivan);

            if (VM.ProizvodjacId != null)
            {
                DioQry = DioQry.Where(x => VM.ProizvodjacId.Contains(x.ProizvodjacID));
            }
          

            if (VM.Poredak.HasValue)
            {
                switch (VM.Poredak.Value)
                {
                    case 1: DioQry = DioQry.OrderBy(x => x.Naziv); break;
                    case 2: DioQry = DioQry.OrderByDescending(x => x.Naziv); break;
                    case 3: DioQry = DioQry.OrderByDescending(x => x.Cijena); break;
                    case 4: DioQry = DioQry.OrderBy(x => x.Cijena); break;
                }
            }

            Model.PagedResult = DioQry.Select(
              x => new PrikaziDijeloveVM.Row
              {
                  DioId = x.DioId,
                  Naziv = x.Naziv,

                  Cijena = x.Cijena,         
                  
                 
                  Slika = x.Slika,
                  Opis = x.Opis,
                  Kolicina = x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Count(),
                  Aktivan = x.Aktivan,
                  Proizvodjac=x.Proizvodjac
              }
           ).GetPaged(VM.Page, 6);

            if (VM.PrikaziKaoListu)
                return PartialView("UcitajListuDijelovaLista", Model);

            return PartialView(Model);

        }
    }
}