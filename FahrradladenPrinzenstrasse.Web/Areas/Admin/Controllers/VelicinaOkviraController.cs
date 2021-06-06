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
    public class VelicinaOkviraController : Controller
    {
        private readonly MyContext db;

        public VelicinaOkviraController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<VelicinaOkvira> vm = db.VelicinaOkvira
                .Where(x => x.IsDeleted == false)
                 .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }

        public IActionResult Dodaj()
        {
            return View("DodajUredi", new VelicinaOkvira());
        }

        public IActionResult Snimi(VelicinaOkvira vm)
        {
            VelicinaOkvira novi;
            if (vm.VelicinaOkviraId == 0)
            {
                novi = new VelicinaOkvira();
                db.VelicinaOkvira.Add(novi);
            }
            else
            {
                novi = db.VelicinaOkvira.Where(x => x.VelicinaOkviraId == vm.VelicinaOkviraId).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            VelicinaOkvira temp = db.VelicinaOkvira.Where(x => x.VelicinaOkviraId == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            VelicinaOkvira vm = db.VelicinaOkvira.Where(x => x.VelicinaOkviraId == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }
    }
}