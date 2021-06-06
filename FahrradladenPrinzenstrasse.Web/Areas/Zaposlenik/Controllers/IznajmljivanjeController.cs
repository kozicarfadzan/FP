using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{

    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class IznajmljivanjeController : Controller
    {

        private readonly MyContext db;

        public IznajmljivanjeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(PrikaziIznajmljivanje VM)
        {

            IQueryable<RezervacijaIznajmljenaBicikla> query = db.RezervacijaIznajmljenaBicikla;

            var Model = new PrikaziIznajmljivanje
            {
                DatumOd = VM?.DatumOd ?? DateTime.Now.Date.AddDays(-3),
                DatumDo = VM?.DatumDo ?? DateTime.Now.Date,
                KlijentId = VM.KlijentId,
                Klijenti = db.Klijent.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Korisnik.Ime + " " + x.Korisnik.Prezime
                }).ToList()
            };

            if (Model.DatumOd.HasValue && Model.DatumDo.HasValue)
                query = query.Where(x => x.DatumPreuzimanja.Date >= Model.DatumOd.Value && x.DatumVracanja.Date <= Model.DatumDo.Value);
            if (Model.KlijentId != 0)
                query = query.Where(x => x.Rezervacija.KlijentId == Model.KlijentId);

            query = query.Include(x => x.BiciklStanje.Bicikl.Model.Proizvodjac)
                .Include(x => x.Rezervacija.Klijent.Korisnik);

            Model.Termini = query.OrderByDescending(x => x.DatumPreuzimanja).ToList();

            return View(Model);
        }

        [HttpGet]
        public IActionResult Detalji(int Id)
        {
            var termin = db.RezervacijaIznajmljenaBicikla.Where(x => x.RezervacijaIznajmljenaBiciklaID == Id)
                .Include(x => x.BiciklStanje.Bicikl.Model.Proizvodjac)
                .Include(x => x.Rezervacija.Klijent.Korisnik)
                .FirstOrDefault();
            if (termin == null)
                return RedirectToAction("Index");

            var zaposlenik = HttpContext.GetLogiraniKorisnik().Zaposlenik;
            var notifikacija = db.Notifikacija.Where(x => x.ZaposlenikId == zaposlenik.Id
                && x.Tip == TipNotifikacije.Istekao_Termin
                && x.RezervacijaIznajmljenaBiciklaId == Id
                && x.IsProcitano == false).FirstOrDefault();
            if(notifikacija != null)
            {
                notifikacija.IsProcitano = true;
                db.SaveChanges();
            }


            var notifikacija2 = db.Notifikacija.Where(x => x.ZaposlenikId == zaposlenik.Id
                && x.Tip == TipNotifikacije.Novi_Termin
                && x.Rezervacija.RezervacijaIznajmljenaBicikla.Any(y=>y.RezervacijaIznajmljenaBiciklaID == Id)
                && x.IsProcitano == false).FirstOrDefault();
            if (notifikacija2 != null)
            {
                notifikacija2.IsProcitano = true;
                db.SaveChanges();
            }

            return View(termin);
        }

        [HttpPost]
        public JsonResult Zavrseno(int Id, bool stanje)
        {
            var termin = db.RezervacijaIznajmljenaBicikla.Where(x => x.RezervacijaIznajmljenaBiciklaID == Id)
                .FirstOrDefault();
            if (termin == null)
                return Json(new { error = "Rezervacija nije pronađena." });

            termin.IsZavrseno = stanje;
            db.SaveChanges();

            return Json(new { });
        }



    }
}