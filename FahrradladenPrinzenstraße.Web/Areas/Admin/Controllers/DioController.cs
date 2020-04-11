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
    public class DioController : Controller
    {
        private readonly MyContext db;

        public DioController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(string pretraga)
        {
            PrikaziDioVM vm = new PrikaziDioVM();
            vm.Dio = db.Dio.Include(x => x.Proizvodjac).Where(x => x.Naziv.Contains(pretraga) || pretraga == null).ToList();


            return View(vm);
        }

        private void PripremiStavke(DodajDioVM Model)
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
            DodajDioVM vm = new DodajDioVM();

            PripremiStavke(vm);

            return View("Dodaj", vm);
        }

        [HttpPost]
        public IActionResult Dodaj(DodajDioVM model)
        {
            if (ModelState.IsValid)
            {
                Dio dio = new Dio()
                {
                    Naziv = model.Naziv,
                    Opis = model.Opis,
                    Cijena = model.Cijena,
                    ProizvodjacID = model.ProizvodjacId

                };


                db.Dio.Add(dio);
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
            var dio = db.Dio.Where(x => x.DioId == id).FirstOrDefault();
            if (dio == null)
                return RedirectToAction("Index");

            UrediDioVM model = new UrediDioVM(db, dio);
            return View(model);
        }



        [HttpPost]
        public ActionResult Uredi(UrediDioVM model)
        {
            var dio = db.Dio.Where(x => x.DioId == model.DioId).FirstOrDefault();



            if (ModelState.IsValid)
            {
                dio.Naziv = model.Naziv;
                dio.Opis = model.Opis;
                dio.Cijena = model.Cijena;


                db.SaveChanges();
            }
            else
            {
                model = new UrediDioVM(db, dio);
                return View(model);
            }

            return RedirectToAction("Index");
        }

    }
}