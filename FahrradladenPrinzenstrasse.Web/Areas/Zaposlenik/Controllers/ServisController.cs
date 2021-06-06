using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{

    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class ServisController : Controller
    {

        private readonly MyContext db;

        public ServisController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string pretraga)
        {
            PrikaziServiseVM vm=new PrikaziServiseVM();
            vm.Servisi = db.Servis
                .Where(x => x.IsDeleted == false).Where(x => x.Naziv.Contains(pretraga) || pretraga == null).ToList();

            return View(vm);
        }

        [HttpGet]
       public IActionResult Dodaj()
        {
            DodajServisVM Model = new DodajServisVM();
            return View("Dodaj",Model);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajServisVM vm)
        {
            if (ModelState.IsValid)
            {
                Servis servis = new Servis()
                {
                    Naziv = vm.Naziv,
                    Opis = vm.Opis,
                    Cijena = vm.Cijena

                };
                db.Servis.Add(servis);
                db.SaveChanges();
            }
            else
            {
                return View("Dodaj", vm);
            }


            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Uredi(int Id)
        {
            var servis = db.Servis.Where(x => x.ServisId == Id).FirstOrDefault();
            if (servis == null)
                return RedirectToAction("Index");

            UrediServisVM model = new UrediServisVM(db, servis);
            return View(model);
        }
       
        [HttpPost]
        public ActionResult Uredi(UrediServisVM model)
        {
            var servis = db.Servis.Where(x => x.ServisId == model.ServisId).FirstOrDefault();

         

            if (ModelState.IsValid)
            {
                servis.Naziv = model.Naziv;
                servis.Opis = model.Opis;
                servis.Cijena = model.Cijena;
                servis.Trajanje = model.Trajanje;

                db.SaveChanges();
            }
            else
            {
                model = new UrediServisVM(db, servis);
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}