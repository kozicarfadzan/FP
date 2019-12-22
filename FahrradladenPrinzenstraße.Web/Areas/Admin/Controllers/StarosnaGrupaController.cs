using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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

            db.StarosnaGrupa.Remove(temp);
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