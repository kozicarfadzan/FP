using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{

    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class BiciklGPSLokacijeController : Controller
    {
        private readonly MyContext db;

        public BiciklGPSLokacijeController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(PrikaziBiciklGPSLokacijeVM VM)
        {
            VM.Bicikla = db.Bicikl
               .Where(x => x.Stanje == Stanje.Korišteno)
               .Where(x => x.Aktivan == true)
               .Include(x => x.Model.Proizvodjac)
               .Select(
                  x => new SelectListItem
                  {
                      Text = x.PuniNaziv,
                      Value = x.BiciklId.ToString()
                  }
               ).ToList();
            VM.GPSLokacije = db.BiciklGPSLokacije
                .Where(x => x.BiciklId == VM.BiciklId)
                .Where(x => VM.Datum == null || VM.Datum.Value.Date == x.DateReported.Date)
                .OrderByDescending(x=>x.DateReported)
                .ToList();

            return View(VM);
        }


    }
}