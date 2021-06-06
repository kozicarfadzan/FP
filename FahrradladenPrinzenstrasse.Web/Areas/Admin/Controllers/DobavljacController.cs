using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FahrradladenPrinzenstrasse.Web.Helper;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Autorizacija(administrator: true)]
    public class DobavljacController : Controller
    {

        private readonly MyContext db;

        public DobavljacController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<Dobavljac> vm = db.Dobavljac
                 .Where(x => x.IsDeleted == false)
                 .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }


        public IActionResult Detalji(int Id, int partial)
        {
            Dobavljac vm = db.Dobavljac
                 .Where(x => x.DobavljacID == Id)
                 
                 .FirstOrDefault();
            if (partial == 1)
                return PartialView(vm);
            return View(vm);
        }

        public IActionResult Dodaj()
        {

            return View("DodajUredi", new Dobavljac());
        }

        public IActionResult Snimi(Dobavljac vm)
        {
            Dobavljac novi;
            if (vm.DobavljacID == 0)
            {
                novi = new Dobavljac();
                db.Dobavljac.Add(novi);
            }
            else
            {
                novi = db.Dobavljac.Where(x => x.DobavljacID == vm.DobavljacID).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;
           

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Dobavljac temp = db.Dobavljac.Where(x => x.DobavljacID == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            Dobavljac vm = db.Dobavljac.Where(x => x.DobavljacID == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }
    }
}