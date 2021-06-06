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

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.Controllers
{
    [Area("Klijent")]
    [Autorizacija(klijent: true)]
    public class KorpaController : Controller
    {

        private readonly MyContext db;

        public KorpaController(MyContext db)
        {
            this.db = db;
        }


        public IActionResult Stavke()
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            KorpaStavkeVM VM = new KorpaStavkeVM
            {
                Stavke = GetKorpaStavke(Klijent)
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
            PotvrdaNarudzbeVM VM = new PotvrdaNarudzbeVM
            {
                Stavke = GetKorpaStavke(Klijent),
                NaciniOtpreme = db.NacinOtpreme
                    .Where(x => x.IsDeleted == false).OrderBy(x => x.Cijena).ToList(),
                NacinOtpremeId = Model.NacinOtpremeId,
                NacinPlacanja = Model.NacinPlacanja
            };

            if (VM.Stavke.Count == 0)
            {
                return RedirectToAction("Stavke");
            }

            var UneseniPodaci = Model?.Podaci;

            VM.Podaci = new PotvrdaNarudzbeVM.KorisnickiPodaci
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

            VM.BrojKupljenihProizvoda = 0;

            foreach (var stavka in VM.Stavke)
            {
                double cijena = 0;
                if (stavka.Bicikl != null)
                    cijena = stavka.Bicikl.Cijena.Value;
                else if (stavka.Oprema != null)
                    cijena = stavka.Oprema.Cijena;
                else if (stavka.Dio != null)
                    cijena = stavka.Dio.Cijena;

                VM.BrojKupljenihProizvoda++;

                VM.UkupnoProizvodi += cijena * stavka.Kolicina;
            }

            double stopa_poreza = 17.0;
            VM.Postarina = 0.0;
            if (VM.NacinOtpremeId != 0)
                VM.Postarina = VM.NaciniOtpreme.Where(x => x.NacinOtpremeId == VM.NacinOtpremeId).Single()?.Cijena ?? 0.0;
            VM.UkupniIznos = VM.UkupnoProizvodi;
            VM.Osnovica = VM.UkupniIznos / (1 + stopa_poreza / 100);
            VM.UkupnoPoreza = VM.UkupniIznos - VM.Osnovica;
            VM.UkupniIznos += VM.Postarina;

            if (Request.Method == "POST")
            {
                var lista_rezervisanih_dijelova = new List<RezervacijaProdajaDio>();
                var lista_rezervisanih_bicikala = new List<RezervacijaProdajaBicikla>();
                var lista_rezervisane_opreme = new List<RezervacijaProdajaOprema>();

                try
                {
                    // Bicikla
                    foreach (var naruceno_biciklo in VM.Stavke.Where(x => x.BiciklId != null).ToList())
                    {
                        var bicikl_stanja = db.BiciklStanje
                            .Where(x => x.BiciklId == naruceno_biciklo.BiciklId)
                            .Where(x => x.Aktivan == true)
                            .Where(x => x.Kolicina > 0)
                            .ToList();

                        if (bicikl_stanja.Sum(x => x.Kolicina) < naruceno_biciklo.Kolicina)
                            throw new Exception("Naručeno biciklo " + naruceno_biciklo.Bicikl.PuniNaziv + " nije dostupno u traženoj kolicini! Da bi ste nastavili narudžbu, uklonite proizvod iz košarice.");

                        var trazena_kolicina = naruceno_biciklo.Kolicina;

                        foreach (var bicikl_na_stanju in bicikl_stanja)
                        {
                            var dostupna_kolicina = Math.Min(trazena_kolicina, bicikl_na_stanju.Kolicina);

                            bicikl_na_stanju.Kolicina -= dostupna_kolicina;
                            trazena_kolicina -= dostupna_kolicina;

                            for (int i = 0; i < dostupna_kolicina; i++)
                            {
                                lista_rezervisanih_bicikala.Add(new RezervacijaProdajaBicikla
                                {
                                    BiciklStanjeId = bicikl_na_stanju.BiciklStanjeId,
                                });
                            }

                            if (trazena_kolicina == 0)
                                break;
                        }
                    }
                    // Dijelovi
                    foreach (var naruceno_dio in VM.Stavke.Where(x => x.DioId != null).ToList())
                    {
                        var dio_stanja = db.DioStanje
                            .Where(x => x.DioId == naruceno_dio.DioId)
                            .Where(x => x.Aktivan == true)
                            .Where(x => x.KupacId == null)
                            .Take(naruceno_dio.Kolicina)
                            .ToList();

                        if (dio_stanja.Count < naruceno_dio.Kolicina)
                            throw new Exception("Naručeno dio " + naruceno_dio.Dio.Naziv + " nije dostupno u traženoj kolicini! Da bi ste nastavili narudžbu, uklonite proizvod iz košarice.");

                        foreach (var dio_na_stanju in dio_stanja)
                        {
                            lista_rezervisanih_dijelova.Add(new RezervacijaProdajaDio
                            {
                                DioStanjeId = dio_na_stanju.DioStanjeId
                            });
                            dio_na_stanju.Aktivan = false;
                            dio_na_stanju.KupacId = Klijent.Id;
                        }
                    }
                    // Oprema
                    foreach (var narucena_oprema in VM.Stavke.Where(x => x.OpremaId != null).ToList())
                    {
                        var oprema_stanja = db.OpremaStanje
                            .Where(x => x.OpremaId == narucena_oprema.OpremaId)
                            .Where(x => x.Aktivan == true)
                            .Where(x => x.KupacId == null)
                            .Take(narucena_oprema.Kolicina)
                            .ToList();

                        if (oprema_stanja.Count < narucena_oprema.Kolicina)
                            throw new Exception("Naručena oprema " + narucena_oprema.Oprema.Naziv + " nije dostupna u traženoj kolicini! Da bi ste nastavili narudžbu, uklonite proizvod iz košarice.");

                        foreach (var oprema_na_stanju in oprema_stanja)
                        {
                            lista_rezervisane_opreme.Add(new RezervacijaProdajaOprema
                            {
                                OpremaStanjeId = oprema_na_stanju.OpremaStanjeId
                            });
                            oprema_na_stanju.Aktivan = false;
                            oprema_na_stanju.KupacId = Klijent.Id;
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
                    Postarina = VM.Postarina,
                    UkupniIznos = VM.UkupniIznos,
                    UkupnoPoreza = VM.UkupnoPoreza,
                    UkupnoProizvodi = VM.UkupnoProizvodi,
                    RezervacijaProdajaBicikla = lista_rezervisanih_bicikala,
                    RezervacijaProdajaDio = lista_rezervisanih_dijelova,
                    RezervacijaProdajaOprema = lista_rezervisane_opreme,
                    StanjeRezervacije = StanjeRezervacije.Čekanje_uplate
                };

                if (narudzba.NacinOtpremeId == 0)
                    narudzba.NacinOtpremeId = db.NacinOtpreme
                        .Where(x => x.IsDeleted == false).First(x => x.Cijena == 0).NacinOtpremeId;

                db.Rezervacija.Add(narudzba);
                db.SaveChanges();

                var korpa_stavke = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id).ToList();
                foreach (var item in korpa_stavke)
                {
                    db.KorpaStavka.Remove(item);
                }

                db.SaveChanges();

                if (VM.NacinPlacanja == "online")
                {
                    return RedirectToAction("OnlineUplata", new { narudzba.RezervacijaId });
                }

                else
                {
                    var zaposlenici = db.Zaposlenik.Where(x => x.Korisnik.Aktivan == true).ToList();
                    foreach (var zaposlenik in zaposlenici)
                    {
                        var notifikacija = new Notifikacija
                        {
                            ZaposlenikId = zaposlenik.Id,
                            Tip = TipNotifikacije.Nova_Narudzba,
                            Rezervacija = narudzba,
                            DatumVrijeme = DateTime.Now
                        };
                        db.Notifikacija.Add(notifikacija);
                    }

                    db.SaveChanges();
                    return RedirectToAction("KrajNarudzbe");

                }
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
            var rezervacija_na_cekanju = db.Rezervacija.Where(x => x.KlijentId == Klijent.Id && x.StanjeRezervacije == StanjeRezervacije.Čekanje_uplate && x.RezervacijaId == RezervacijaId).FirstOrDefault();

            if (rezervacija_na_cekanju == null)
            {
                return Redirect("/");
            }

            return View(rezervacija_na_cekanju);
        }
        public IActionResult KrajNarudzbe(int RezervacijaId)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var rezervacija_na_cekanju = db.Rezervacija.Where(x => x.KlijentId == Klijent.Id && x.StanjeRezervacije == StanjeRezervacije.Čekanje_uplate && x.RezervacijaId == RezervacijaId).FirstOrDefault();


            return View("KrajNarudzbe");
        }
        private List<KorpaStavka> GetKorpaStavke(Data.EntityModels.Klijent Klijent)
        {
            return db.KorpaStavka
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Include(x => x.Bicikl).ThenInclude(x => x.BiciklStanje)
                .Include(x => x.Dio).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Oprema).ThenInclude(x => x.Proizvodjac)
                .Where(x => x.KlijentId == Klijent.Id).ToList();
        }

        public IActionResult Obrisi(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var stavka = db.KorpaStavka.Where(x => x.KorpaStavkaId == Id && x.KlijentId == Klijent.Id).FirstOrDefault();
            if (stavka != null)
            {
                db.Remove(stavka);
                db.SaveChanges();

                return RedirectToAction("Stavke");
            }
            else
                return new NotFoundResult();
        }
        public IActionResult Umanji(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var stavka = db.KorpaStavka.Where(x => x.KorpaStavkaId == Id && x.KlijentId == Klijent.Id).FirstOrDefault();
            if (stavka != null)
            {
                if (stavka.Kolicina > 1)
                    stavka.Kolicina--;
                db.SaveChanges();

                return RedirectToAction("Stavke");
            }
            else
                return new NotFoundResult();
        }
        public IActionResult Uvecaj(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var stavka = db.KorpaStavka
                .Include(x => x.Bicikl).ThenInclude(x => x.BiciklStanje)
                .Include(x => x.Oprema).ThenInclude(x => x.OpremaStanje)
                .Include(x => x.Dio).ThenInclude(x => x.DioStanje)
                .Where(x => x.KorpaStavkaId == Id && x.KlijentId == Klijent.Id)
                .FirstOrDefault();

            if (stavka != null)
            {
                int nova_kolicina = stavka.Kolicina + 1;

                if (stavka.Bicikl != null)
                {
                    var ukupno_u_skladistu = stavka.Bicikl.BiciklStanje.Where(x => x.Aktivan == true).Sum(x => x.Kolicina);
                    if (ukupno_u_skladistu < nova_kolicina)
                        return new BadRequestResult();
                }
                if (stavka.Dio != null)
                {
                    var ukupno_u_skladistu = stavka.Dio.DioStanje.Count(x => x.Aktivan == true && x.KupacId == null);
                    if (ukupno_u_skladistu < nova_kolicina)
                        return new BadRequestResult();
                }
                if (stavka.Oprema != null)
                {
                    var ukupno_u_skladistu = stavka.Oprema.OpremaStanje.Count(x => x.Aktivan == true && x.KupacId == null);
                    if (ukupno_u_skladistu < nova_kolicina)
                        return new BadRequestResult();
                }

                stavka.Kolicina++;
                db.SaveChanges();

                return RedirectToAction("Stavke");
            }
            else
                return new NotFoundResult();

        }

        [HttpGet]
        public IActionResult GetUkupnaCijenaIBrojArtikala()
        {
            return Json(new
            {
                UkupnoStavki = HttpContext.GetBrojStavkiKosarica(),
                UkupnaCijena = HttpContext.GetUkupnoKosarica()
            });
        }


    }
}