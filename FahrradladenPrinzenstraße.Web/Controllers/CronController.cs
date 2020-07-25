using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstraße.Web.Controllers
{
    public class CronController : Controller
    {
        private readonly MyContext db;

        public CronController(MyContext db)
        {
            this.db = db;
        }
        public StatusCodeResult NotifikacijeIznajmljivanja()
        {
            var istekli_termini = db.RezervacijaIznajmljenaBicikla.Where(x => x.Isteklo == false && x.DatumVracanja <= DateTime.Now).ToList();
            var zaposlenici = db.Zaposlenik.Where(x => x.Korisnik.Aktivan == true).ToList();
            foreach (var termin in istekli_termini)
            {
                termin.Isteklo = true;
                foreach (var zaposlenik in zaposlenici)
                {
                    var notifikacija = new Notifikacija
                    {
                        RezervacijaIznajmljenaBiciklaId = termin.RezervacijaIznajmljenaBiciklaID,
                        ZaposlenikId = zaposlenik.Id,
                        Tip = TipNotifikacije.Istekao_Termin,
                        DatumVrijeme = DateTime.Now
                    };
                    db.Notifikacija.Add(notifikacija);


                }
            }

            try
            {
                db.SaveChanges();

                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }

        }
    }
}