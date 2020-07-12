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
    public class BiciklController : Controller
    {

        private readonly MyContext db;

        public BiciklController(MyContext db)
        {
            this.db = db;
        }


        public IActionResult KupiBicikl(int Id)
        {
            KupiBiciklVM VM = new KupiBiciklVM
            {
                Bicikl = db.Bicikl
                .Include(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Model).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Model).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Boja)
                .Include(x => x.BiciklStanje)
                .Where(x => x.BiciklId == Id)
                .Where(x => x.Stanje == Stanje.Novo || x.Stanje == Stanje.Polovno)
                .FirstOrDefault()
            };

            if (VM.Bicikl == null)
            {
                return Redirect("/Bicikl");
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = VM.Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == Id).Sum(x => x.Kolicina);

            VM.KolicinaNaStanju = ukupno_u_skladistu - ukupno_u_kosarici;

            return View(VM);
        }


        [HttpPost]
        public StatusCodeResult KupiBicikl(int Id, int kolicina)
        {
            Bicikl Bicikl = db.Bicikl
                .Include(x => x.BiciklStanje)
                .Where(x => x.BiciklId == Id)
                .Where(x => x.Stanje == Stanje.Novo || x.Stanje == Stanje.Polovno)
                .FirstOrDefault();

            if (Bicikl == null)
            {
                return new NotFoundResult();// 404
            }

            if (kolicina < 1)
            {
                return new BadRequestResult();
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = Bicikl.BiciklStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == Id).Sum(x => x.Kolicina);
            if (kolicina > ukupno_u_skladistu - ukupno_u_kosarici)
            {
                return new BadRequestResult(); // 400
            }

            var PostojecaStvaka = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.BiciklId == Id).FirstOrDefault();
            if (PostojecaStvaka != null)
            {
                PostojecaStvaka.Kolicina += kolicina;
            }
            else
            {
                db.KorpaStavka.Add(new KorpaStavka
                {
                    KlijentId = Klijent.Id,
                    BiciklId = Id,
                    Kolicina = kolicina
                });
            }
            db.SaveChanges();

            return new OkResult(); // 200
        }


    }
}