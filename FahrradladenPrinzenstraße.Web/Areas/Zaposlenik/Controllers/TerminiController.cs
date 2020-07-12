using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstraße.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Web.Areas.Zaposlenik.Controllers
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
            if (VM.Datum.HasValue)
                query = query.Where(x => x.DatumServisiranja == VM.Datum.Value);
            if (!string.IsNullOrWhiteSpace(VM.Klijent))
                query = query.Where(x =>
                (x.Rezervacija.Klijent.Korisnik.Ime + " " + x.Rezervacija.Klijent.Korisnik.Prezime).Contains(VM.Klijent) ||
                (x.Rezervacija.Klijent.Korisnik.Prezime + " " + x.Rezervacija.Klijent.Korisnik.Ime).Contains(VM.Klijent));

            query = query.Include(x => x.Servis)
                .Include(x => x.Rezervacija.Klijent.Korisnik);

            PrikaziTermineVM Model = new PrikaziTermineVM
            {
                Termini = query.ToList(),
                Klijent = VM.Klijent,
                Datum = VM.Datum
            };

            return View(Model);
        }

        [HttpGet]
        public IActionResult Detalji(int Id)
        {
            var termin = db.RezervacijaServis.Where(x => x.RezervacijaServisId == Id)
                .Include(x=>x.Servis)
                .Include(x=>x.Rezervacija.Klijent.Korisnik)
                .FirstOrDefault();
            if (termin == null)
                return RedirectToAction("Index");

            return View(termin);
        }
       
       

    }
}