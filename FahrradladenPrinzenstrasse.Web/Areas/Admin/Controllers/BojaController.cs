﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Autorizacija(administrator: true)]
    public class BojaController : Controller
    {

        private readonly MyContext db;

        public BojaController(MyContext db)
        {
            this.db = db;
        }




        public IActionResult Index(string Pretraga)
        {
            List<Boja> vm = db.Boja
                .Where(x => x.IsDeleted == false)
                .Where(x => x.Naziv.Contains(Pretraga) || Pretraga == null)
                .ToList();

            return View(vm);
        }

        public IActionResult Dodaj()
        {
            return View("DodajUredi", new Boja());
        }

        public IActionResult Snimi(Boja vm)
        {
            Boja novi;
            if (vm.BojaId == 0)
            {
                novi = new Boja();
                db.Boja.Add(novi);
            }
            else
            {
                novi = db.Boja.Where(x => x.BojaId == vm.BojaId).FirstOrDefault();
            }
            novi.Naziv = vm.Naziv;

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Obrisi(int Id)
        {
            Boja temp = db.Boja.Where(x => x.BojaId == Id).FirstOrDefault();

            temp.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Uredi(int Id)
        {
            Boja vm = db.Boja.Where(x => x.BojaId == Id).FirstOrDefault();

            return View("DodajUredi", vm);
        }

    }
}