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
    public class ProizvodjacController : Controller
    {

        private readonly MyContext db;

        public ProizvodjacController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<Proizvodjac> vm = db.Proizvodjac
                .Where(x => x.IsDeleted == false)
                .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }

        public IActionResult Dodaj()
        {
            return View("DodajUredi", new Proizvodjac());
        }

        public IActionResult Snimi(Proizvodjac vm)
        {
            Proizvodjac novi;
            if (vm.ProizvodjacId == 0)
            {
                novi = new Proizvodjac();
                db.Proizvodjac.Add(novi);
            }
            else
            {
                novi = db.Proizvodjac.Where(x => x.ProizvodjacId == vm.ProizvodjacId).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Proizvodjac temp = db.Proizvodjac.Where(x => x.ProizvodjacId == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            Proizvodjac vm = db.Proizvodjac.Where(x => x.ProizvodjacId == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }
    }
}