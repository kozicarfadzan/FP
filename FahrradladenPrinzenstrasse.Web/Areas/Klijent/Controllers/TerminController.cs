using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class TerminController : Controller
    {

        private readonly MyContext db;

        public TerminController(MyContext db)
        {
            this.db = db;
        }


        public IActionResult Stavke()
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            TerminStavkeVM VM = new TerminStavkeVM
            {
                Stavke = GetTerminStavke(Klijent)
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(VM);
            }
            return View(VM);
        }
        public IActionResult PotvrdaNarudzbe(PotvrdaNarudzbeVM Model)
        {
            var Korisnik = HttpContext.GetLogiraniKorisnik();
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            PotvrdaRezervacijeBicikalaVM VM = new PotvrdaRezervacijeBicikalaVM
            {
                Stavke = GetTerminStavke(Klijent)
            };

            if (VM.Stavke.Count == 0)
            {
                return RedirectToAction("Stavke");
            }

            var UneseniPodaci = Model?.Podaci;

            VM.Podaci = new PotvrdaRezervacijeBicikalaVM.KorisnickiPodaci
            {
                AdresaStanovanja = UneseniPodaci?.AdresaStanovanja ?? Korisnik.AdresaStanovanja,
                BrojTelefona = UneseniPodaci?.BrojTelefona ?? Korisnik.BrojTelefona,
                Država = UneseniPodaci?.Država ?? "",
                Email = UneseniPodaci?.Email ?? Korisnik.Email,
                Grad = UneseniPodaci?.Grad ?? Korisnik.Grad.Naziv,
                Ime = UneseniPodaci?.Ime ?? Korisnik.Ime,
                Prezime = UneseniPodaci?.Prezime ?? Korisnik.Prezime,
                Pokrajina = UneseniPodaci?.Pokrajina ?? "",
                PostanskiKod = UneseniPodaci?.PostanskiKod ?? ""
            };

            foreach (var stavka in VM.Stavke)
            {
                double cijena = 0;
                if (stavka.Bicikl != null)
                    cijena = stavka.Bicikl.CijenaPoDanu.Value * stavka.BrojDana;

                VM.UkupniIznos += cijena * stavka.Kolicina;
            }

            double stopa_poreza = 17.0;
            VM.Osnovica = VM.UkupniIznos / (1 + stopa_poreza / 100);
            VM.UkupnoPoreza = VM.UkupniIznos - VM.Osnovica;

            if (Request.Method == "POST")
            {
                var lista_rezervisanih_bicikala = new List<RezervacijaIznajmljenaBicikla>();

                try
                {
                    // Bicikla
                    foreach (var naruceno_biciklo in VM.Stavke.ToList())
                    {
                        var bicikl_stanja = db.BiciklStanje
                            .Where(x => x.BiciklId == naruceno_biciklo.BiciklId)
                            .Where(x => x.Aktivan == true)
                            .Where(x => x.Kolicina > 0)
                            .ToList();

                        if (bicikl_stanja.Sum(x=>x.Kolicina) < naruceno_biciklo.Kolicina)
                            throw new Exception("Naručeno biciklo " + naruceno_biciklo.Bicikl.PuniNaziv + " nije dostupno u traženoj kolicini!");

                        var trazena_kolicina = naruceno_biciklo.Kolicina;

                        foreach (var bicikl_na_stanju in bicikl_stanja)
                        {
                            var dostupna_kolicina = Math.Min(trazena_kolicina, bicikl_na_stanju.Kolicina);

                            bicikl_na_stanju.Kolicina -= dostupna_kolicina;
                            trazena_kolicina -= dostupna_kolicina;

                            for (int i = 0; i < dostupna_kolicina; i++)
                            {
                                lista_rezervisanih_bicikala.Add(new RezervacijaIznajmljenaBicikla
                                {
                                    BiciklStanjeId = bicikl_na_stanju.BiciklStanjeId,
                                    DatumPreuzimanja = naruceno_biciklo.DatumPreuzimanja,
                                    DatumVracanja = naruceno_biciklo.DatumVracanja
                                });
                            }

                            if (trazena_kolicina == 0)
                                break;
                        }

                    }

                }
                catch (Exception ex)
                {
                    TempData["error_message"] = ex.Message;

                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return PartialView(VM);
                    }
                    return View(VM);
                }

                Rezervacija narudzba = new Rezervacija
                {
                    AdresaStanovanja = VM.Podaci.AdresaStanovanja,
                    BrojTelefona = VM.Podaci.BrojTelefona,
                    Država = VM.Podaci.Država,
                    Email = VM.Podaci.Email,
                    Grad = VM.Podaci.Grad,
                    Ime = VM.Podaci.Ime,
                    Prezime = VM.Podaci.Prezime,
                    Pokrajina = VM.Podaci.Pokrajina,
                    PostanskiKod = VM.Podaci.PostanskiKod,
                    DatumRezervacije = DateTime.Now,
                    KlijentId = Klijent.Id,
                    NacinOtpremeId = Model.NacinOtpremeId,
                    NacinPlacanja = Model.NacinPlacanja,
                    Osnovica = VM.Osnovica,
                    UkupniIznos = VM.UkupniIznos,
                    UkupnoPoreza = VM.UkupnoPoreza,
                    RezervacijaIznajmljenaBicikla = lista_rezervisanih_bicikala,
                    StanjeRezervacije = StanjeRezervacije.Čekanje_uplate
                };

                if (narudzba.NacinOtpremeId == 0)
                    narudzba.NacinOtpremeId = db.NacinOtpreme
                        .Where(x => x.IsDeleted == false).First(x => x.Cijena == 0).NacinOtpremeId;

                db.Rezervacija.Add(narudzba);
                db.SaveChanges();

                var termin_stavke = db.TerminStavka.Where(x => x.KlijentId == Klijent.Id).ToList();
                foreach (var item in termin_stavke)
                {
                    db.TerminStavka.Remove(item);
                }
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
            var rezervacija_na_cekanju = db.Rezervacija
                .Where(x => x.KlijentId == Klijent.Id && x.StanjeRezervacije == StanjeRezervacije.Čekanje_uplate && x.RezervacijaId == RezervacijaId)
                .Where(x => x.RezervacijaIznajmljenaBicikla.Any())
                .FirstOrDefault();

            if (rezervacija_na_cekanju == null)
            {
                return Redirect("/");
            }

            return View(rezervacija_na_cekanju);
        }

        private List<TerminStavka> GetTerminStavke(Data.EntityModels.Klijent Klijent)
        {
            return db.TerminStavka
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Include(x => x.Bicikl).ThenInclude(x => x.BiciklStanje)
                .Where(x => x.KlijentId == Klijent.Id).ToList();
        }

        public IActionResult Obrisi(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var stavka = db.TerminStavka.Where(x => x.TerminStavkaId == Id && x.KlijentId == Klijent.Id).FirstOrDefault();
            if (stavka != null)
            {
                db.Remove(stavka);
                db.SaveChanges();

                return RedirectToAction("Stavke");
            }
            else
                return new NotFoundResult();
        }

        [HttpGet]
        public IActionResult GetUkupanBrojTermina()
        {
            return Json(new
            {
                UkupnoTermina = HttpContext.GetBrojTerminaKosarica(),
                UkupnaCijena = HttpContext.GetUkupnaCijenaTermina()
            });
        }


    }
}