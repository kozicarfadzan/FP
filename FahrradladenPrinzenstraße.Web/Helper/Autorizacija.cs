using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace FahrradladenPrinzenstraße.Web.Helper

{

    public class Autorizacija : TypeFilterAttribute
    {
        public Autorizacija(bool administrator = false, bool zaposlenik = false, bool klijent = false)
            : base(typeof(MyAuthorizeImpl))
        {
            Arguments = new object[] { administrator, zaposlenik, klijent };
        }
    }


    public class MyAuthorizeImpl : IAsyncActionFilter
    {
        public MyAuthorizeImpl(bool administrator, bool zaposlenik, bool klijent)
        {
            _administrator = administrator;
            _zaposlenik = zaposlenik;
            _klijent = klijent;
        }
        private readonly bool _administrator;

        private readonly bool _zaposlenik;
        private readonly bool _klijent;
        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            Korisnik k = filterContext.HttpContext.GetLogiraniKorisnik();

            if (k == null)
            {
                if (filterContext.Controller is Controller controller)
                {
                    controller.TempData["error_poruka"] = "Niste logirani";
                }

                filterContext.Result = new RedirectResult("/Autentifikacija/Index");
                return;
            }

            //Preuzimamo DbContext preko app services
            MyContext db = filterContext.HttpContext.RequestServices.GetService<MyContext>();
            if (_administrator && k.Administrator != null)
            {
                await next(); //ok - ima pravo pristupa
                return;
            }
            //zaposlenici mogu pristupiti
            if (_zaposlenik && k.Zaposlenik != null)
            {
                await next(); //ok - ima pravo pristupa
                return;
            }
            if (_klijent && k.Klijent != null)
            {
                await next(); //ok - ima pravo pristupa
                return;
            }
            if (filterContext.Controller is Controller c1)
            {
                c1.TempData["error_poruka"] = "Nemate pravo pristupa";
            }
            filterContext.Result = new RedirectResult("/Autentifikacija/Index");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // throw new NotImplementedException();
        }
    }
}

