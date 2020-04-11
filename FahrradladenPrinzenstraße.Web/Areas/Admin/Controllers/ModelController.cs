﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FahrradladenPrinzenstraße.Web.Helper;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.Controllers
{
    [Area("Admin")] [Autorizacija(administrator:true)]
    public class ModelController : Controller
    {

        private readonly MyContext db;

        public ModelController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<Model> vm = db.Modeli
                 .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                 .ToList();

            return View(vm);
        }


        public IActionResult Detalji(int Id, int partial)
        {
            Model vm = db.Modeli
                 .Where(x => x.ModelId == Id)
                 .Include(x=>x.MaterijalOkvira)
                 .Include(x=>x.StarosnaGrupa)
                 .Include(x=>x.Proizvodjac)
                 .Include(x=>x.VelicinaOkvira)
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

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Model temp = db.Modeli.Where(x => x.ModelId == Id).FirstOrDefault();

            db.Modeli.Remove(temp);
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