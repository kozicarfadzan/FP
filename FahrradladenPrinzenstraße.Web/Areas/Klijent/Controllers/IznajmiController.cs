using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class IznajmiController : Controller
    {
        private readonly MyContext db;

        public IznajmiController(MyContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult OdaberiTermin(int Id)
        {
            IznajmiBiciklVM VM = new IznajmiBiciklVM
            {
                Bicikl = db.Bicikl
                .Include(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.StarosnaGrupa)
                .Include(x => x.VelicinaOkvira)
                .Include(x => x.Boja)
                .Include(x => x.BiciklStanje)
                .Where(x => x.BiciklId == Id && x.Stanje == Stanje.Korišteno)
                .FirstOrDefault()
            };

            if (VM.Bicikl == null)
            {
                return Redirect("/Iznajmljivanje");
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = VM.Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);

            VM.KolicinaNaStanju = ukupno_u_skladistu;

            int Kolicina = 1;
            VM.RezervisaniTermini = GetDaneBezDostupnihTermina(Id, Klijent.Id, Kolicina, ukupno_u_skladistu);

            return View(VM);
        }

        [HttpGet]
        public IActionResult GetZauzeteTermine(int Id, int Kolicina)
        {
            var Bicikl = db.Bicikl
                .Include(x => x.BiciklStanje)
                .Where(x => x.BiciklId == Id && x.Stanje == Stanje.Korišteno)
                .FirstOrDefault();

            if (Bicikl == null)
            {
                return Json(new { error = "Biciklo nije pronađeno" });
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);

            var RezervisaniTermini = GetDaneBezDostupnihTermina(Id, Klijent.Id, Kolicina, ukupno_u_skladistu);

            return Json(new { rezervisani_termini = RezervisaniTermini });
        }

        private List<string> GetDaneBezDostupnihTermina(int Id, int KlijentId, int Kolicina, int ukupno_u_skladistu)
        {
            var rezervisani_termini = new List<string>();

            var aktivne_rezervacije = db.RezervacijaIznajmljenaBicikla
                .Where(x => x.BiciklStanje.BiciklId == Id)
                .Where(x => x.DatumPreuzimanja.Date >= DateTime.Now.Date || x.DatumVracanja.Date >= DateTime.Now.Date)
                .ToList();

            Dictionary<DateTime, int> broj_rezervacija_po_danima = new Dictionary<DateTime, int>();

            foreach (var item in aktivne_rezervacije)
            {
                foreach (DateTime date in DateTimeHelper.EachDay(item.DatumPreuzimanja, item.DatumVracanja))
                {
                    if (broj_rezervacija_po_danima.ContainsKey(date))
                        broj_rezervacija_po_danima[date]++;
                    else
                        broj_rezervacija_po_danima[date] = 1;
                }
            }

            var termini_u_kosarici = db.TerminStavka.Where(x => x.KlijentId == KlijentId && x.BiciklId == Id).ToList();
            foreach (var termin_u_kosarici in termini_u_kosarici)
            {
                foreach (DateTime date in DateTimeHelper.EachDay(termin_u_kosarici.DatumPreuzimanja, termin_u_kosarici.DatumVracanja))
                {
                    if (broj_rezervacija_po_danima.ContainsKey(date))
                        broj_rezervacija_po_danima[date] += termin_u_kosarici.Kolicina;
                    else
                        broj_rezervacija_po_danima[date] = termin_u_kosarici.Kolicina;
                }
            }

            foreach (var item in broj_rezervacija_po_danima)
            {
                DateTime date = item.Key;
                int broj_rezervacija_za_dan = item.Value;
                int preostalo_bicikala = ukupno_u_skladistu - broj_rezervacija_za_dan;

                if (preostalo_bicikala < Kolicina)
                    rezervisani_termini.Add(date.Month + "/" + date.Day + "/" + date.Year);
            }

            return rezervisani_termini;
        }

        [HttpPost]
        public StatusCodeResult OdaberiTermin(IznajmiBiciklVM VM)
        {
            Bicikl Bicikl = db.Bicikl
                .Include(x => x.BiciklStanje)
                .Where(x => x.BiciklId == VM.Id)
                .Where(x => x.Stanje == Stanje.Korišteno)
                .FirstOrDefault();

            if (Bicikl == null)
            {
                return new NotFoundResult();// 404
            }

            if (!ModelState.IsValid || VM.DatumPreuzimanja.Date < DateTime.Now.Date || VM.DatumPreuzimanja.Date > VM.DatumVracanja.Date)
            {
                return new UnprocessableEntityResult(); // 422
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            int ukupno_u_skladistu = Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            int ukupno_u_kosarici = db.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == VM.Id)
                .Where(x =>
                   (
                       (x.DatumPreuzimanja.Date >= VM.DatumPreuzimanja.Date && x.DatumPreuzimanja.Date <= VM.DatumVracanja.Date)
                       || (x.DatumVracanja.Date >= VM.DatumPreuzimanja.Date && x.DatumVracanja.Date <= VM.DatumVracanja.Date)
                   )
                   ||
                   (
                       (VM.DatumPreuzimanja.Date >= x.DatumPreuzimanja.Date && VM.DatumPreuzimanja.Date <= x.DatumVracanja.Date)
                       || (VM.DatumVracanja.Date >= x.DatumPreuzimanja.Date && VM.DatumVracanja.Date <= x.DatumVracanja.Date)
                   ))
                   .Sum(x => x.Kolicina);

            var broj_termina_kolizija = db.RezervacijaIznajmljenaBicikla
                .Where(x => x.BiciklStanje.BiciklId == VM.Id)
                .Where( x =>
                    (
                        (x.DatumPreuzimanja.Date >= VM.DatumPreuzimanja.Date && x.DatumPreuzimanja.Date <= VM.DatumVracanja.Date)
                        || (x.DatumVracanja.Date >= VM.DatumPreuzimanja.Date && x.DatumVracanja.Date <= VM.DatumVracanja.Date)
                    )
                    ||
                    (
                        (VM.DatumPreuzimanja.Date >= x.DatumPreuzimanja.Date && VM.DatumPreuzimanja.Date <= x.DatumVracanja.Date)
                        || (VM.DatumVracanja.Date >= x.DatumPreuzimanja.Date && VM.DatumVracanja.Date <= x.DatumVracanja.Date)
                    )
                )
                .ToList();

            int ukupno_dostupno = ukupno_u_skladistu - ukupno_u_kosarici - broj_termina_kolizija.Count();
            if (VM.Kolicina > ukupno_dostupno)
            {
                if(ukupno_dostupno == 0)
                    return new BadRequestResult(); // 400

                return new StatusCodeResult(417);
            }

            var PostojecaStvaka = db.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == VM.Id && x.DatumPreuzimanja == VM.DatumPreuzimanja && x.DatumVracanja == VM.DatumVracanja).FirstOrDefault();
            if (PostojecaStvaka != null)
            {
                PostojecaStvaka.Kolicina += VM.Kolicina;
            }
            else
            {
                db.TerminStavka.Add(new TerminStavka
                {
                    KlijentId = Klijent.Id,
                    BiciklId = VM.Id,
                    Kolicina = VM.Kolicina,
                    DatumPreuzimanja = VM.DatumPreuzimanja,
                    DatumVracanja = VM.DatumVracanja.AddHours(23).AddMinutes(59).AddSeconds(59)
                });
            }
            db.SaveChanges();

            return new OkResult(); // 200
        }
    }
}