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
                .Include(x => x.Model).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Model).ThenInclude(x => x.VelicinaOkvira)
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
            var ukupno_u_kosarici = db.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == Id).Sum(x => x.Kolicina);

            VM.KolicinaNaStanju = ukupno_u_skladistu - ukupno_u_kosarici;

            VM.RezervisaniTermini = new List<string>();

            var aktivne_rezervacije = db.RezervacijaIznajmljenaBicikla
                .Where(x => x.BiciklStanje.BiciklId == Id)
                .Where(x => x.DatumPreuzimanja.Date >= DateTime.Now.Date || x.DatumVracanja.Date >= DateTime.Now.Date)
                .ToList();

            foreach (var item in aktivne_rezervacije)
            {
                foreach (DateTime date in DateTimeHelper.EachDay(item.DatumPreuzimanja, item.DatumVracanja))
                {
                    VM.RezervisaniTermini.Add(date.Month + "/" + date.Day + "/" + date.Year);
                }
            }

            return View(VM);
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

            var ukupno_u_skladistu = Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == VM.Id).Sum(x => x.Kolicina);
            if (VM.Kolicina > ukupno_u_skladistu - ukupno_u_kosarici)
            {
                return new BadRequestResult(); // 400
            }

            var PostojecaStvaka = db.TerminStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == VM.Id).FirstOrDefault();
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
                    DatumVracanja = VM.DatumVracanja
                });
            }
            db.SaveChanges();

            return new OkResult(); // 200
        }
    }
}