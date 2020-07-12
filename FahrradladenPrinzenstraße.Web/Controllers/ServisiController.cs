using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Helper;
using FahrradladenPrinzenstraße.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Controllers
{
    public class ServisiController : Controller
    {
        private readonly MyContext db;

        public ServisiController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index(PrikaziServisVM VM)
        {
            PrikaziServisVM Model = new PrikaziServisVM
            {
            };
            return View(Model);
        }

        public ActionResult UcitajListuServisa(PrikaziServisVM VM)
        {
            PrikaziServisVM Model = new PrikaziServisVM();

            IQueryable<Servis> ServisiQry = db.Servis;

            Model.PagedResult = ServisiQry.Select(
               x => new PrikaziServisVM.Row
               {
                   ServisId = x.ServisId,
                   Naziv = x.Naziv,
                   Opis = x.Opis,
                   Cijena = x.Cijena,
                   Trajanje = x.Trajanje
               }
            ).GetPaged(VM.Page, 6);

            return PartialView(Model);
        }

    }
}
