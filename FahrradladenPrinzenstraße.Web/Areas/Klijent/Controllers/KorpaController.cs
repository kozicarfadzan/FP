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
using Microsoft.EntityFrameworkCore.Internal;

namespace FahrradladenPrinzenstraße.Web.Areas.Klijent.Controllers
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
                Stavke = db.KorpaStavka
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.MaterijalOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.StarosnaGrupa)
                .Include(x => x.Bicikl).ThenInclude(x => x.Model).ThenInclude(x => x.VelicinaOkvira)
                .Include(x => x.Bicikl).ThenInclude(x => x.Boja)
                .Include(x => x.Bicikl).ThenInclude(x => x.BiciklStanje)
                .Include(x => x.Dio).ThenInclude(x => x.Proizvodjac)
                .Include(x => x.Oprema).ThenInclude(x => x.Proizvodjac)
                .Where(x => x.KlijentId == Klijent.Id).ToList()
            };
            return View(VM);
        }

        public IActionResult Obrisi(int Id)
        {
            var Klijent = HttpContext.GetLogiraniKorisnik().Klijent;
            var stavka = db.KorpaStavka.Where(x => x.KorpaStavkaId == Id && x.KlijentId == Klijent.Id).FirstOrDefault();
            if (stavka != null)
            {
                db.Remove(stavka);
                db.SaveChanges();
            }
            return RedirectToAction("Stavke");

        }
    }
}