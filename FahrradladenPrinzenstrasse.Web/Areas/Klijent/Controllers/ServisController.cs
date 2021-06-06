using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class ServisController : Controller
    {

        private readonly MyContext db;

        public ServisController(MyContext db)
        {
            this.db = db;
        }


        public IActionResult OdaberiTermin(int Id)
        {
            OdaberiTerminVM VM = new OdaberiTerminVM
            {
                Servis = db.Servis
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ServisId == Id).FirstOrDefault()
            };

            if (VM.Servis == null)
            {
                return Redirect("/Servis");
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            return View(VM);
        }


        [HttpPost]
        public StatusCodeResult OdaberiTermin(OdaberiTerminVM VM)
        {
            if (!ModelState.IsValid || VM.Datum.Date <= DateTime.Now.Date)
                return new UnprocessableEntityResult();

            Servis Servis = db.Servis
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ServisId == VM.Id).FirstOrDefault();
            if (Servis == null)
            {
                return new NotFoundResult();// 404
            }

            double TrajanjeServisa = Servis.Trajanje * VM.Kolicina;
            double SumServiceHours = db.RezervacijaServis.Where(x => x.DatumServisiranja.Date == VM.Datum).Sum(x => (double?)x.Servis.Trajanje ?? 0);
            if (SumServiceHours + TrajanjeServisa > 8)
            {
                return new BadRequestResult();
            }

            return new OkResult(); // 200
        }

        [HttpPost]
        public JsonResult DostupniTermini(OdaberiTerminVM VM)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var lista_termina = new List<string>();

            Servis Servis = db.Servis
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ServisId == VM.Id).FirstOrDefault();

            RadnoVrijeme radnoVrijeme = db.RadnoVrijeme
                .Where(x => x.IsDeleted == false).Where(x => x.DanUSedmici == VM.Datum.DayOfWeek).FirstOrDefault();

            if (radnoVrijeme == null || Servis == null)
                return Json(lista_termina);

            var rezervacije_za_dan = db.RezervacijaServis.Where(x => x.DatumServisiranja.Date == VM.Datum.Date).Include(x => x.Servis).ToList();

            int pocetak_minute = radnoVrijeme.Pocetak.Hours * 60 + radnoVrijeme.Pocetak.Minutes;
            int kraj_minute = radnoVrijeme.Kraj.Hours * 60 + radnoVrijeme.Kraj.Minutes;

            for (int pocetak_termina = pocetak_minute; pocetak_termina < kraj_minute; pocetak_termina += 30)
            {
                int termin_sati = pocetak_termina / 60;
                int termin_minute = pocetak_termina % 60;

                int kraj_termina = pocetak_termina + (int)(Servis.Trajanje * 60) * VM.Kolicina;

                string sati_string = termin_sati.ToString().PadLeft(2, '0');
                string minute_string = termin_minute.ToString().PadLeft(2, '0');

                bool kolizija = false;
                if (pocetak_termina + Servis.Trajanje * 60 > kraj_minute)
                {
                    kolizija = true;
                }
                else if(kraj_termina > kraj_minute)
                {
                    kolizija = true;
                }

                foreach (var rezervacija in rezervacije_za_dan)
                {
                    int pocetak_rezervacije = rezervacija.DatumServisiranja.Hour * 60 + rezervacija.DatumServisiranja.Minute;
                    int kraj_rezervacije = pocetak_rezervacije + (int)(rezervacija.Servis.Trajanje * 60);
                    if (pocetak_termina >= pocetak_rezervacije && pocetak_termina < kraj_rezervacije)
                        kolizija = true;

                    else if (kraj_termina > pocetak_rezervacije && kraj_termina <= kraj_rezervacije)
                        kolizija = true;
                    
                }

                if (!kolizija)
                    lista_termina.Add(sati_string + ":" + minute_string);

            }

            return Json(lista_termina);
        }

        public IActionResult PotvrdaRezervacijeTermina(int Id, PotvrdaRezervacijeTerminaVM VM)
        {
            var Korisnik = HttpContext.GetLogiraniKorisnik();
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            Servis Servis = db.Servis
                .Where(x => x.IsDeleted == false)
                .Where(x => x.ServisId == Id).FirstOrDefault();
            if (!ModelState.IsValid || VM.Datum.Date <= DateTime.Now.Date || Servis == null)
            {
                return Redirect("/Servisi");
            }

            double TrajanjeServisa = Servis.Trajanje * VM.Kolicina;
            if (TrajanjeServisa > 8)
            {
                return Redirect("/Servisi");
            }

            VM.UkupniIznos = Servis.Cijena * VM.Kolicina;
            VM.BrojServisStavki = VM.Kolicina;

            if (Request.Method == "POST")
            {
                double SumServiceHours = db.RezervacijaServis.Where(x => x.DatumServisiranja.Date == VM.Datum).Sum(x => (double?)x.Servis.Trajanje ?? 0);
                if (SumServiceHours + TrajanjeServisa > 8)
                {
                    TempData["error_message"] = "Odabrani termin servisa prekoracuje dostupno vrijeme za odabrani dan.";
                    return View(VM);
                }

                VM.Datum = VM.Datum.Add(VM.Satnica);

                var lista_servisa = new List<RezervacijaServis>();
                for (int i = 0; i < VM.DetaljiServisa.Length; i++)
                {
                    lista_servisa.Add(new RezervacijaServis
                    {
                        Boja = VM.DetaljiServisa[i].Boja,
                        Proizvodjac = VM.DetaljiServisa[i].Proizvodjac,
                        DodatniTroskovi = VM.DetaljiServisa[i].DodatniTroskovi,
                        Model = VM.DetaljiServisa[i].Model,
                        Opis = VM.DetaljiServisa[i].Opis,
                        ServisId = Id,
                        Tip = VM.DetaljiServisa[i].Tip,
                        DatumServisiranja = VM.Datum
                    });
                    VM.Datum = VM.Datum.AddHours(Servis.Trajanje);
                }

                Rezervacija narudzba = new Rezervacija
                {
                    AdresaStanovanja = Korisnik.AdresaStanovanja,
                    BrojTelefona = Korisnik.BrojTelefona,
                    Email = Korisnik.Email,
                    Grad = Korisnik.Grad.Naziv,
                    Ime = Korisnik.Ime,
                    Prezime = Korisnik.Prezime,

                    DatumRezervacije = DateTime.Now,
                    KlijentId = Klijent.Id,
                    NacinOtpremeId = db.NacinOtpreme
                        .Where(x => x.IsDeleted == false).Where(x => x.Cijena == 0).First().NacinOtpremeId,
                    NacinPlacanja = "online",
                    UkupniIznos = VM.UkupniIznos,
                    RezervacijaServis = lista_servisa,
                    StanjeRezervacije = StanjeRezervacije.Čekanje_uplate
                };

                db.Rezervacija.Add(narudzba);
                db.SaveChanges();

                return RedirectToAction("OnlineUplata", new { RezervacijaId = narudzba.RezervacijaId });
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView(VM);
                }
                return View(VM);
            }

        }

        public IActionResult OnlineUplata(int RezervacijaId)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var rezervacija_na_cekanju = db.Rezervacija.Where(x => x.KlijentId == Klijent.Id && x.StanjeRezervacije == StanjeRezervacije.Čekanje_uplate && x.RezervacijaId == RezervacijaId && x.RezervacijaServis.Any()).FirstOrDefault();

            if (rezervacija_na_cekanju == null)
            {
                return Redirect("/");
            }

            return View("OnlineUplata", rezervacija_na_cekanju);
        }

    }
}