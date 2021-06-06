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
    public class GradController : Controller
    {


        private readonly MyContext db;

        public GradController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<Grad> vm = db.Grad
                 .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }

        public IActionResult Dodaj()
        {
            return View("DodajUredi", new Grad());
        }

        public IActionResult Snimi(Grad vm)
        {
            Grad novi;
            if (vm.GradID == 0)
            {
                novi = new Grad();
                db.Grad.Add(novi);
            }
            else
            {
                novi = db.Grad.Where(x => x.GradID == vm.GradID).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Grad temp = db.Grad.Where(x => x.GradID == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            Grad vm = db.Grad.Where(x => x.GradID == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }
    }
 }
