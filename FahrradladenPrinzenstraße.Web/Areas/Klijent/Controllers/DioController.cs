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
    public class DioController : Controller
    {

        private readonly MyContext db;

        public DioController(MyContext db)
        {
            this.db = db;
        }


        public IActionResult KupiDio(int Id)
        {
            KupiDioVM VM = new KupiDioVM
            {
                Dio = db.Dio
                .Include(x => x.Proizvodjac)
                .Include(x => x.DioStanje)
                .Where(x => x.DioId == Id).FirstOrDefault()
            };

            if (VM.Dio == null)
            {
                return Redirect("/Dio");
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = VM.Dio.DioStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.DioId == Id).Sum(x => x.Kolicina);

            VM.KolicinaNaStanju = ukupno_u_skladistu - ukupno_u_kosarici;

            return View(VM);
        }


        [HttpPost]
        public StatusCodeResult KupiDio(int Id, int kolicina)
        {
            Dio Dio = db.Dio
                .Include(x => x.DioStanje)
                .Where(x => x.DioId == Id).FirstOrDefault();

            if (Dio == null)
            {
                return new NotFoundResult();// 404
            }

            if(kolicina < 1)
            {
                return new BadRequestResult();
            }

            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;

            var ukupno_u_skladistu = Dio.DioStanje.Count(x => x.Aktivan == true && x.KupacId == null);
            var ukupno_u_kosarici = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.DioId == Id).Sum(x => x.Kolicina);
            if (kolicina > ukupno_u_skladistu - ukupno_u_kosarici)
            {
                return new BadRequestResult(); // 400
            }


            var PostojecaStvaka = db.KorpaStavka.Where(x => x.KlijentId == Klijent.Id && x.DioId == Id).FirstOrDefault();
            if(PostojecaStvaka != null)
            {
                PostojecaStvaka.Kolicina += kolicina;
            }
            else
            {
                db.KorpaStavka.Add(new KorpaStavka
                {
                    KlijentId = Klijent.Id,
                    DioId = Id,
                    Kolicina = kolicina
                });
            }
            db.SaveChanges();

            return new OkResult(); // 200
        }


    }
}