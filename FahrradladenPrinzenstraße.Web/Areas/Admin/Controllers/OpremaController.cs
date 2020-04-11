using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels;
using FahrradladenPrinzenstraße.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Autorizacija(administrator: true)]
    public class OpremaController : Controller
    {
        private readonly MyContext db;

        public OpremaController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string pretraga)
        {
            PrikaziOpremuVM vm = new PrikaziOpremuVM();
            vm.Oprema = db.Oprema.Include(x=>x.Proizvodjac).Where(x => x.Naziv.Contains(pretraga) || pretraga == null).ToList();
        

            return View(vm);
        }

        private void PripremiStavke(DodajOpremuVM Model)
        {
            
            Model.Proizvodjaci = new List<SelectListItem>();
            Model.Proizvodjaci.AddRange(db.Proizvodjac.Select(x => new SelectListItem()
            {
                Value = x.ProizvodjacId.ToString(),
                Text = x.Naziv
            }));


        }
        [HttpGet]
        public IActionResult Dodaj()
        {
            DodajOpremuVM vm = new DodajOpremuVM();

            PripremiStavke(vm);

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajOpremuVM model)
        {
            if (ModelState.IsValid)
            {
                Oprema oprema = new Oprema()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                    Cijena = model.Cijena,
                    ProizvodjacID = model.ProizvodjacId

                };
                

                db.Oprema.Add(oprema);
                db.SaveChanges();
            }
            else
            {
                PripremiStavke(model);

                return View("Dodaj", model);
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Uredi(int id)
        {
            var oprema = db.Oprema.Where(x => x.OpremaId == id).FirstOrDefault();
            if (oprema == null)
                return RedirectToAction("Index");

            UrediOpremuVM model = new UrediOpremuVM(db, oprema);
            return View(model);
        }

       

        [HttpPost]
        public ActionResult Uredi(UrediOpremuVM model)
        {
            var oprema = db.Oprema.Where(x => x.OpremaId == model.OpremaId).FirstOrDefault();

           

            if (ModelState.IsValid)
            {
                oprema.Naziv = model.Naziv;
                oprema.Opis = model.Opis;
                oprema.Cijena = model.Cijena;
              

                db.SaveChanges();
            }
            else
            {
                model = new UrediOpremuVM(db, oprema);
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}