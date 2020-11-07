using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;

namespace FahrradladenPrinzenstraße.Web.Controllers
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

            var narudzba = db.Rezervacija.Where(x => x.RezervacijaId == request.RezervacijaId).FirstOrDefault();
            var narudzba_iznos = (int)narudzba.UkupniIznos * 100;

            if (paymentIntent.Amount == narudzba_iznos && paymentIntent.Status == "succeeded")
            {
                if (narudzba.StanjeRezervacije == Data.EntityModels.StanjeRezervacije.Čekanje_uplate)
                {
                    narudzba.StanjeRezervacije = Data.EntityModels.StanjeRezervacije.U_obradi;
                    if (narudzba.DatumUplate is null)
                        narudzba.DatumUplate = DateTime.Now;
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