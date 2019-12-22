using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Data;

namespace FahrradladenPrinzenstraße.Web.Controllers
{
    public class HomeController : Controller
         
    {
        private readonly MyContext db;

        public HomeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

      

      

       
    }
}
