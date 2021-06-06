using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;

namespace FahrradladenPrinzenstrasse.Web.Controllers
{

    [ApiController]
    public class PaymentIntentApiController : ControllerBase
    {
        private readonly MyContext db;

        public PaymentIntentApiController(MyContext db)
        {
            this.db = db;
        }

        [HttpPost]
        [Route("create-payment-intent")]
        public ActionResult Create(PaymentIntentCreateRequest request)
        {
            var paymentIntents = new PaymentIntentService();
            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(request.RezervacijaId),
                Currency = "bam",
            });
            return new JsonResult(new { clientSecret = paymentIntent.ClientSecret });
        }
        [HttpPost]
        [Route("confirm-payment-intent")]
        public ActionResult Confirm(PaymentIntentConfirmRequest request)
        {
            var service = new PaymentIntentService();
            var paymentIntent = service.Get(request.PaymentIntentId);

            var narudzba = db.Rezervacija
                .Where(x => x.RezervacijaId == request.RezervacijaId)
                .Include(x=>x.RezervacijaIznajmljenaBicikla)
                .Include(x=>x.RezervacijaServis)
                .FirstOrDefault();
            var narudzba_iznos = (int)narudzba.UkupniIznos * 100;

            if (paymentIntent.Amount == narudzba_iznos && paymentIntent.Status == "succeeded")
            {
                if (narudzba.StanjeRezervacije == StanjeRezervacije.Čekanje_uplate)
                {
                    narudzba.StanjeRezervacije = StanjeRezervacije.U_obradi;
                    if (narudzba.DatumUplate is null)
                        narudzba.DatumUplate = DateTime.Now;

                    bool IsServisRezervacija = narudzba.RezervacijaServis.Any(),
                         IsTerminRezervacija = narudzba.RezervacijaIznajmljenaBicikla.Any();

                    if(IsServisRezervacija || IsTerminRezervacija || narudzba.NacinPlacanja == "online")
                    {
                        var zaposlenici = db.Zaposlenik.Where(x => x.Korisnik.Aktivan == true).ToList();
                        foreach (var zaposlenik in zaposlenici)
                        {
                            var notifikacija = new Notifikacija
                            {
                                ZaposlenikId = zaposlenik.Id,
                                Rezervacija = narudzba,
                                DatumVrijeme = DateTime.Now
                            };
                            if (IsServisRezervacija)
                                notifikacija.Tip = TipNotifikacije.Novi_Servis;
                            else if (IsTerminRezervacija)
                                notifikacija.Tip = TipNotifikacije.Novi_Termin;
                            else
                                notifikacija.Tip = TipNotifikacije.Nova_Narudzba;

                            db.Notifikacija.Add(notifikacija);
                        }
                    }
                    db.SaveChanges();
                }
            }

            return new JsonResult(new { success = true });
        }
        private int CalculateOrderAmount(int RezervacijaId)
        {
            return (int)db.Rezervacija.Where(x => x.RezervacijaId == RezervacijaId).FirstOrDefault().UkupniIznos * 100;
        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty]
            public int RezervacijaId { get; set; }
        }
        public class PaymentIntentConfirmRequest
        {
            [JsonProperty]
            public string PaymentIntentId { get; set; }
            [JsonProperty]
            public int RezervacijaId { get; set; }
        }
    }
}