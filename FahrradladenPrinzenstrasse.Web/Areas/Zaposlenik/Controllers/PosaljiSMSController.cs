using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels;
using FahrradladenPrinzenstrasse.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexmo.Api;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.Controllers
{

    [Area("Zaposlenik")]
    [Autorizacija(zaposlenik: true)]
    public class PosaljiSMSController : Controller
    {
        private readonly MyContext db;

        public PosaljiSMSController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult PosaljiSMS()
        {
            PosaljiSMSVM vm = new PosaljiSMSVM();
            UcitajVM(vm);

            return View(vm);
        }

        private void UcitajVM(PosaljiSMSVM vm)
        {

            
            vm.Klijenti =db.Korisnik.Include(x=>x.Klijent).Where(x=>x.Klijent.Id== x.KorisnikID)
              .ToList();
        }

        [HttpPost]
        public ActionResult PosaljiSMS(PosaljiSMSVM vm)
        {


            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "ff373c2a",
                ApiSecret = "F1szqiBrqYj98WdJ"
            });

            foreach (var klijentId in vm.OdabraniKlijenti)
            {
                Korisnik klijent = db.Korisnik.Find(klijentId);

                string SadrzajPoruke = "Pozdrav, " + klijent.Ime + "!\n" + vm.Sadrzaj;

                var results = client.SMS.Send(new SMS.SMSRequest
                {
                    from = "+38762279893",
                    to = klijent.BrojTelefona,
                    text = SadrzajPoruke
                });
            }

            UcitajVM(vm);


            return View(vm);
        }
    }
}