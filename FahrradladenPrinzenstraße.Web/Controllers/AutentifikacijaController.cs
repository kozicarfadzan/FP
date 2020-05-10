using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Data.EntityModels;
using FahrradladenPrinzenstraße.Web.Helper;
using FahrradladenPrinzenstraße.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FahrradladenPrinzenstraße.Web.Controllers
{
    public class AutentifikacijaController : Controller
    {
        private readonly MyContext _db;

        public AutentifikacijaController(MyContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(new LoginVM()
            {
                ZapamtiPassword = true,
            });
        }

        public IActionResult Login(LoginVM input)
        {
            Korisnik korisnik = _db.Korisnik
                .Include(x => x.Zaposlenik)
                .Include(x => x.Administrator)
                .Include(x => x.Klijent)
                .SingleOrDefault(x => x.KorisnickoIme == input.username);

            bool success = false;


            if (korisnik != null)
            {
                var newHash = PasswordHelper.GenerateHash(korisnik.LozinkaSalt, input.password);

                if (newHash == korisnik.LozinkaHash)
                {
                    success = true;
                }
            }

            if (success == false)
            {
                TempData["error_poruka"] = "Pogrešan username ili password";
                return View("Index", input);
            }

            HttpContext.SetLogiraniKorisnik(korisnik, input.ZapamtiPassword);

            if (korisnik.Administrator != null)
            {
                return Redirect("/Admin/Zaposlenik/Index");
            }
            if (korisnik.Zaposlenik != null)
            {
                return Redirect("/Zaposlenik/Home/Index");
            }
            if (korisnik.Klijent != null)
            {
                return Redirect("/");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SetLogiraniKorisnik(null);
            return RedirectToAction("Index", "Home");
        }
    }
}