using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class RezervacijeController : Controller
    {

        private readonly MyContext db;

        public RezervacijeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(RezervacijePretragaVM VM)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var model = new RezervacijePretragaVM
            {
                DatumOd = VM?.DatumOd ?? DateTime.Now.Date.AddMonths(-1),
                DatumDo = VM?.DatumDo ?? DateTime.Now.Date,
                VrstaRezervacije = VM.VrstaRezervacije
            };

            IQueryable<Rezervacija> query = db.Rezervacija
                .Where(x => x.KlijentId == Klijent.Id)
                .Include(x => x.RezervacijaIznajmljenaBicikla)
                .Include(x => x.RezervacijaProdajaBicikla)
                .Include(x => x.RezervacijaProdajaDio)
                .Include(x => x.RezervacijaProdajaOprema)
                .Include(x => x.RezervacijaServis);

            if (model.DatumOd.HasValue && model.DatumDo.HasValue)
                query = query.Where(x => model.DatumOd <= x.DatumRezervacije.Date && x.DatumRezervacije.Date <= model.DatumDo.Value);

            if(model.VrstaRezervacije.HasValue)
            {
                switch (model.VrstaRezervacije.Value)
                {
                    case VrstaRezervacije.Kupovina:
                        query = query.Where(x => x.RezervacijaProdajaBicikla.Any() || x.RezervacijaProdajaDio.Any() || x.RezervacijaProdajaOprema.Any());
                        break;
                    case VrstaRezervacije.Servis:
                        query = query.Where(x => x.RezervacijaServis.Any());
                        break;
                    case VrstaRezervacije.Iznajmljivanje:
                        query = query.Where(x => x.RezervacijaIznajmljenaBicikla.Any());
                        break;
                    default:
                        break;
                }
            }


            model.Rezervacije = query.OrderByDescending(x => x.RezervacijaId).ToList();

            return View(model);
        }

        public IActionResult Detalji(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;


            Rezervacija vm = db.Rezervacija.Where(x => x.RezervacijaId == Id)
                .Where(x => x.KlijentId == Klijent.Id)
                .Include("RezervacijaIznajmljenaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaBicikla.BiciklStanje.Bicikl.Model.Proizvodjac")
                .Include("RezervacijaProdajaDio.DioStanje.Dio")
                .Include("RezervacijaProdajaOprema.OpremaStanje.Oprema")
                .Include("RezervacijaServis.Servis")
                .FirstOrDefault();

            return View(vm);
        }

    }
}