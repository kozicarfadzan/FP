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
    public class StarosnaGrupaController : Controller
    {
        
        private readonly MyContext db;

        public StarosnaGrupaController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<StarosnaGrupa> vm = db.StarosnaGrupa
                .Where(x => x.IsDeleted == false)
                 .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }

        public IActionResult Dodaj()
        {
            return View("DodajUredi", new StarosnaGrupa());
        }

        public IActionResult Snimi(StarosnaGrupa vm)
        {
            StarosnaGrupa novi;
            if (vm.StarosnaGrupaId == 0)
            {
                novi = new StarosnaGrupa();
                db.StarosnaGrupa.Add(novi);
            }
            else
            {
                novi = db.StarosnaGrupa.Where(x => x.StarosnaGrupaId == vm.StarosnaGrupaId).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            StarosnaGrupa temp = db.StarosnaGrupa.Where(x => x.StarosnaGrupaId == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            StarosnaGrupa vm = db.StarosnaGrupa.Where(x => x.StarosnaGrupaId == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        } 
         

    }
}