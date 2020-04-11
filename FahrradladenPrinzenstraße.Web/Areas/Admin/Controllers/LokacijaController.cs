using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc;
using FahrradladenPrinzenstraße.Web.Helper;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.Controllers
{
    [Area("Admin")] [Autorizacija(administrator:true)]
    public class LokacijaController : Controller
    {


        private readonly MyContext db;

        public LokacijaController(MyContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string pretraga)
        {
            List<Lokacija> vm = db.Lokacija.Where(x => x.Naziv.Contains(pretraga) || pretraga==null).ToList();


            
            return View(vm);
        }
    }
}