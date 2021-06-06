using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{
    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class NarudzbeController : Controller
    {

        private readonly MyContext db;

        public NarudzbeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(RezervacijePretragaVM VM)
        {
            var model = new NarudzbeViewModel
            {
                DatumOd = VM?.DatumOd ?? DateTime.Now.Date.AddMonths(-1),
                DatumDo = VM?.DatumDo ?? DateTime.Now.Date,
            };

            IQueryable<Rezervacija> query = db.Rezervacija
                .Include(x => x.RezervacijaIznajmljenaBicikla)
                .Include(x => x.RezervacijaProdajaBicikla)
                .Include(x => x.RezervacijaProdajaDio)
                .Include(x => x.RezervacijaProdajaOprema)
                .Include(x => x.RezervacijaServis);

            if (model.DatumOd.HasValue && model.DatumDo.HasValue)
                query = query.Where(x => model.DatumOd <= x.DatumRezervacije.Date && x.DatumRezervacije.Date <= model.DatumDo.Value);
   
            query = query.Where(x => x.RezervacijaProdajaBicikla.Any() || x.RezervacijaProdajaDio.Any() || x.RezervacijaProdajaOprema.Any());

            model.Rezervacije = query.OrderByDescending(x => x.RezervacijaId).ToList();

            return View(model);
        }

        public IActionResult Detalji(int Id)
        {
            var zaposlenik = HttpContext.GetLogiraniKorisnik().Zaposlenik;
            var notifikacija = db.Notifikacija.Where(x => x.ZaposlenikId == zaposlenik.Id
                && x.Tip == TipNotifikacije.Nova_Narudzba
                && x.IsProcitano == false)
                .Where(x => x.Rezervacija.RezervacijaProdajaBicikla.Any() || x.Rezervacija.RezervacijaProdajaDio.Any() || x.Rezervacija.RezervacijaProdajaOprema.Any())
                .FirstOrDefault();
            if (notifikacija != null)
            {
                notifikacija.IsProcitano = true;
                db.SaveChanges();
            }

            Rezervacija vm = db.Rezervacija.Where(x => x.RezervacijaId == Id)
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.OcjenaProizvoda")
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.OcjenaProizvoda")
                .Include("RezervacijaProdajaDio.DioStanje.Dio")
                .Include("RezervacijaProdajaDio.DioStanje.Dio.OcjenaProizvoda")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema.OcjenaProizvoda")
                .Include("RezervacijaServis.Servis")
                .Where(x => x.RezervacijaProdajaBicikla.Any() || x.RezervacijaProdajaDio.Any() || x.RezervacijaProdajaOprema.Any())
                .FirstOrDefault();

            return View(vm);
        }


    }
}