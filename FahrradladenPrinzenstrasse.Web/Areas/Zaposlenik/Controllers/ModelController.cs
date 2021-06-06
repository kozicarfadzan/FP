using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FahrradladenPrinzenstrasse.Web.Helper;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{
    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class ModelController : Controller
    {

        private readonly MyContext db;

        public ModelController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<Model> vm = db.Modeli.Include(x => x.MaterijalOkvira)
                 .Include(x => x.Proizvodjac)
                 .Where(x => x.IsDeleted == false)
                 .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }


        public IActionResult Detalji(int Id, int partial)
        {
            Model vm = db.Modeli
                 .Where(x => x.ModelId == Id)
                 .Include(x => x.MaterijalOkvira)
                 .Include(x => x.Proizvodjac)
                 .FirstOrDefault();
            if (partial == 1)
                return PartialView(vm);
            return View(vm);
        }

        public IActionResult Dodaj()
        {

            return View("DodajUredi", new Model());
        }

        public IActionResult Snimi(Model vm)
        {
            Model novi;
            if (vm.ModelId == 0)
            {
                novi = new Model();
                db.Modeli.Add(novi);
            }
            else
            {
                novi = db.Modeli.Where(x => x.ModelId == vm.ModelId).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;
            novi.ProizvodjacId = vm.ProizvodjacId;
            novi.MaterijalOkviraId = vm.MaterijalOkviraId;
            novi.Tip = vm.Tip;
            novi.Suspenzija = vm.Suspenzija;
            novi.Brzina = vm.Brzina;
            novi.SpolBicikl = vm.SpolBicikl;


            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Model temp = db.Modeli.Where(x => x.ModelId == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            Model vm = db.Modeli.Where(x => x.ModelId == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }
    }
}