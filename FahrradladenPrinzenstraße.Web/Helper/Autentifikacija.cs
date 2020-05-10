using FahrradladenPrinzenstraße.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FahrradladenPrinzenstraße.Data;

namespace FahrradladenPrinzenstraße.Web.Helper
{
    public static class Autentifikacija
    {

        private const string LogiraniKorisnik = "logirani_korisnik";

        public static void SetLogiraniKorisnik(this HttpContext context, Korisnik korisnik, bool zapamtiSesiju = false)
        {

            MyContext db = context.RequestServices.GetService<MyContext>();

            string stariToken = context.Request.GetCookieJson<string>(LogiraniKorisnik);
            if (stariToken != null)
            {
                AutorizacijskiToken obrisati = db.AutorizacijskiToken.FirstOrDefault(x => x.Vrijednost == stariToken);
                if (obrisati != null)
                {
                    db.AutorizacijskiToken.Remove(obrisati);
                    db.SaveChanges();
                }
            }

            if (korisnik != null)
            {

                string token = Guid.NewGuid().ToString();
                db.AutorizacijskiToken.Add(new AutorizacijskiToken
                {
                    Vrijednost = token,
                    KorisnikId = korisnik.KorisnikID,
                    VrijemeEvidentiranja = DateTime.Now
                });
                db.SaveChanges();
                context.Response.SetCookieJson(LogiraniKorisnik, token, zapamtiSesiju);

            }
        }

        public static string GetTrenutniToken(this HttpContext context)
        {
            return context.Request.GetCookieJson<string>(LogiraniKorisnik);
        }

        public static Korisnik GetLogiraniKorisnik(this HttpContext context)
        {
            MyContext db = context.RequestServices.GetService<MyContext>();

            string token = context.Request.GetCookieJson<string>(LogiraniKorisnik);
            if (token == null)
                return null;

            int KorisnikId = db.AutorizacijskiToken
                .Where(x => x.Vrijednost == token)
                .Select(s => s.KorisnikId).FirstOrDefault();
            if (KorisnikId != 0)
            {
                return db.Korisnik
                .Include(x => x.Zaposlenik)
                .Include(x => x.Administrator)
                .Include(x => x.Klijent)
                .Where(x => x.KorisnikID == KorisnikId)
                .SingleOrDefault();
            }

            return null;

        }

        public static double GetUkupnoKosarica(this HttpContext context)
        {
            Korisnik korisnik = context.GetLogiraniKorisnik();
            MyContext db = context.RequestServices.GetService<MyContext>();

            double ukupno = 0;
            if (korisnik != null && korisnik.Klijent != null)
            {
                var stavke = db.KorpaStavka
                    .Include(x => x.Bicikl)
                    .Include(x => x.Oprema)
                    .Include(x => x.Dio)
                    .Where(x => x.KlijentId == korisnik.Klijent.Id)
                    .ToList();
                foreach (var stavka in stavke)
                {
                    double cijena = 0;
                    if (stavka.Bicikl != null)
                        cijena = stavka.Bicikl.Cijena.Value;
                    else if (stavka.Oprema != null)
                        cijena = stavka.Oprema.Cijena;
                    else if (stavka.Dio != null)
                        cijena = stavka.Dio.Cijena;

                    ukupno += cijena * stavka.Kolicina;
                }

            }
            return ukupno;

        }
        public static int GetBrojStavkiKosarica(this HttpContext context)
        {
            Korisnik korisnik = context.GetLogiraniKorisnik();
            MyContext db = context.RequestServices.GetService<MyContext>();

            int broj_stavki = 0;
            if (korisnik != null && korisnik.Klijent != null)
            {
                broj_stavki = db.KorpaStavka
                    .Count(x => x.KlijentId == korisnik.Klijent.Id);
            }
            return broj_stavki;

        }
    }

}