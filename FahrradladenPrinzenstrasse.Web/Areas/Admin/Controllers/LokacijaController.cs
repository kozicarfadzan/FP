using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;
using FahrradladenPrinzenstrasse.Web.Helper;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.Controllers
{
    [Area("Admin")] [Autorizacija(administrator:true)]
    public class LokacijaController : Controller
    {


        private readonly MyContext db;

        public LokacijaController(MyContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string pretraga)
        {
            List<Lokacija> vm = db.Lokacija
                .Where(x => x.IsDeleted == false).Where(x => x.Naziv.Contains(pretraga) || pretraga==null).ToList();


            
            return View(vm);
        }
        public IActionResult Dodaj()
        {
            return View("DodajUredi", new Lokacija());
        }

        public IActionResult Snimi(Lokacija vm)
        {
            Lokacija novi;
            if (vm.LokacijaId == 0)
            {
                novi = new Lokacija();
                db.Lokacija.Add(novi);
            }
            else
            {
                novi = db.Lokacija.Where(x => x.LokacijaId == vm.LokacijaId).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Lokacija temp = db.Lokacija.Where(x => x.LokacijaId == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            Lokacija vm = db.Lokacija.Where(x => x.LokacijaId == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }
    }
}