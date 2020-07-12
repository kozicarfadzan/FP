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
    public class OpremaController : Controller
    {

        private readonly MyContext db;

        public OpremaController(MyContext db)
        {
            this.db = db;
        }


        public IActionResult KupiOprema(int Id)
        {
            KupiOpremaVM VM = new KupiOpremaVM
            {
                Oprema = db.Oprema
                .Include(x => x.Proizvodjac)
                .Include(x => x.OpremaStanje)
                .Where(x => x.OpremaId == Id).FirstOrDefault()
            };

            if (VM.Oprema == null)
            {
                return Redirect("/Oprema");
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = VM.Oprema.OpremaStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.OpremaId == Id).Sum(x => x.Kolicina);

            VM.KolicinaNaStanju = ukupno_u_skladistu - ukupno_u_kosarici;

            return View(VM);
        }


        [HttpPost]
        public StatusCodeResult KupiOprema(int Id, int kolicina)
        {
            Oprema Oprema = db.Oprema
                .Include(x => x.OpremaStanje)
                .Where(x => x.OpremaId == Id).FirstOrDefault();

            if (Oprema == null)
            {
                return new NotFoundResult();// 404
            }

            if (kolicina < 1)
            {
                return new BadRequestResult();
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = Oprema.OpremaStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.OpremaId == Id).Sum(x => x.Kolicina);
            if (kolicina > ukupno_u_skladistu - ukupno_u_kosarici)
            {
                return new BadRequestResult(); // 400
            }


            var PostojecaStvaka = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.OpremaId == Id).FirstOrDefault();
            if (PostojecaStvaka != null)
            {
                PostojecaStvaka.Kolicina += kolicina;
            }
            else
            {
                db.KorpaStavka.Add(new KorpaStavka
                {
                    KlijentId = Klijent.Id,
                    OpremaId = Id,
                    Kolicina = kolicina
                });
            }
            db.SaveChanges();

            return new OkResult(); // 200
        }


    }
}