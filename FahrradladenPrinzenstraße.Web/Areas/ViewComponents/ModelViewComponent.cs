using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.ViewComponents
{
    public class ModelViewComponent : ViewComponent {


        public IViewComponentResult Invoke(int modelId)
        {
            MyContext db = new MyContext();
            ModelVM model = new ModelVM
            {
                rows = db.Modeli.Where(x => x.ModelId == modelId).Select(
               x => new ModelVM.Row
               {
                   Naziv = x.Naziv,
                   Brzina = x.Brzina,
                   MaterijalOkvira = x.MaterijalOkvira,
                   Proizvodjac = x.Proizvodjac,
                   SpolBicikl = x.SpolBicikl,
                   StarosnaGrupa = x.StarosnaGrupa,
                   Suspenzija = x.Suspenzija,
                   Tip = x.Tip,
                   VelicinaOkvira = x.VelicinaOkvira

               }
            ).ToList()
            };


            return View("ModelVC", model);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(int modelId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
      MyContext db = new MyContext();
            ModelVM model = new ModelVM
            {
                rows = db.Modeli.Where(x=>x.ModelId==modelId).Select(
               x => new ModelVM.Row
               {
                   Naziv=x.Naziv,
                   Brzina=x.Brzina,
                   MaterijalOkvira=x.MaterijalOkvira,
                   Proizvodjac=x.Proizvodjac,
                   SpolBicikl=x.SpolBicikl,
                   StarosnaGrupa=x.StarosnaGrupa,
                   Suspenzija=x.Suspenzija,
                   Tip=x.Tip,
                   VelicinaOkvira=x.VelicinaOkvira

               }
            ).ToList()
            };


            return View("ModelVC", model);
        }
               
        }
    }

