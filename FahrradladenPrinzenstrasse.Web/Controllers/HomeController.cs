using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace FahrradladenPrinzenstrasse.Web.Controllers
{
    public class HomeController : Controller
         
    {
        private readonly MyContext db;

        public HomeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var model = new HomeVM
            {
                PopularniProizvodi = new List<PreporuceniProizvod>()
            };

            var popularna_bicikla = db.Bicikl.Where(x => (x.Stanje == Stanje.Novo || x.Stanje == Stanje.Polovno) && x.OcjenaProizvoda.Any())
                .Where(x => x.BiciklStanje.Where(y => y.Aktivan).Any(y => y.Kolicina > 0))
                .Where(x => x.Aktivan)
                .Include(x=>x.Model.Proizvodjac)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(2)
                .Select(x => new PreporuceniProizvod
            {
                Id = x.BiciklId,
                Naziv = x.PuniNaziv,
                Cijena = x.Cijena.Value,
                Slika = x.Slika,
                Tip = TipProizvoda.Bicikl
            }).ToList();

            var popularni_dijelovi = db.Dio.Where(x => x.IsDeleted == false).Where(x => x.OcjenaProizvoda.Any())
                .Where(x => x.DioStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaDio.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(2)
                .Select(x => new PreporuceniProizvod
            {
                Id = x.DioId,
                Naziv = x.Naziv,
                Cijena = x.Cijena,
                Slika = x.Slika,
                Tip = TipProizvoda.Dio
            }).ToList();

            var popularna_oprema = db.Oprema.Where(x => x.IsDeleted == false).Where(x => x.OcjenaProizvoda.Any())
                .Where(x => x.OpremaStanje.Where(y => y.Aktivan).Where(y => y.RezervacijaProdajaOprema.Count() == 0).Any())
                .Where(x => x.Aktivan)
                .OrderByDescending(x => x.OcjenaProizvoda.Average(x => x.Ocjena))
                .Take(2)
                .Select(x => new PreporuceniProizvod
            {
                Id = x.OpremaId,
                Naziv = x.Naziv,
                Cijena = x.Cijena,
                Slika = x.Slika,
                Tip = TipProizvoda.Oprema
            }).ToList();

            model.PopularniProizvodi.AddRange(popularna_bicikla);
            model.PopularniProizvodi.AddRange(popularni_dijelovi);
            model.PopularniProizvodi.AddRange(popularna_oprema);

            return View(model);
        }
       
    }
}
