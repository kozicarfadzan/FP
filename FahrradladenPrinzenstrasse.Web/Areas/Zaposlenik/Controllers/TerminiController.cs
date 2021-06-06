using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{

    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class TerminiController : Controller
    {

        private readonly MyContext db;

        public TerminiController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(PrikaziTermineVM VM)
        {

            IQueryable<RezervacijaServis> query = db.RezervacijaServis;

            var Model = new PrikaziTermineVM
            {
                DatumOd = VM?.DatumOd ?? DateTime.Now.Date.AddDays(-3),
                DatumDo = VM?.DatumDo ?? DateTime.Now.Date,
                KlijentId = VM.KlijentId,
                Klijenti = db.Klijent.Select(x=>new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Korisnik.Ime + " " + x.Korisnik.Prezime
                }).ToList()
            };

            if (Model.DatumOd.HasValue && Model.DatumDo.HasValue)
                query = query.Where(x => x.DatumServisiranja.Date >= Model.DatumOd.Value && x.DatumServisiranja.Date <= Model.DatumDo.Value);
            if (Model.KlijentId != 0)
                query = query.Where(x => x.Rezervacija.KlijentId == Model.KlijentId);

            query = query.Include(x => x.Servis)
                .Include(x => x.Rezervacija.Klijent.Korisnik);

            Model.Termini = query.OrderByDescending(x => x.DatumServisiranja).ToList();

            return View(Model);
        }

        [HttpGet]
        public IActionResult Detalji(int Id)
        {
            var termin = db.RezervacijaServis.Where(x => x.RezervacijaServisId == Id)
                .Include(x => x.Servis)
                .Include(x => x.Rezervacija.Klijent.Korisnik)
                .FirstOrDefault();
            if (termin == null)
                return RedirectToAction("Index");

            var zaposlenik = HttpContext.GetLogiraniKorisnik().Zaposlenik;
            var notifikacija = db.Notifikacija.Where(x => x.ZaposlenikId == zaposlenik.Id
                && x.Tip == TipNotifikacije.Novi_Servis
                && x.Rezervacija.RezervacijaServis.Any(y => y.RezervacijaServisId == Id)
                && x.IsProcitano == false).FirstOrDefault();
            if (notifikacija != null)
            {
                notifikacija.IsProcitano = true;
                db.SaveChanges();
            }

            return View(termin);
        }

        [HttpPost]
        public JsonResult Zavrseno(int Id, bool stanje)
        {
            var termin = db.RezervacijaServis.Where(x => x.RezervacijaServisId == Id)
                .FirstOrDefault();
            if (termin == null)
                return Json(new  { error = "Rezervacija nije pronađena." });

            termin.IsZavrseno = stanje;
            db.SaveChanges();

            return Json(new { });
        }



    }
}