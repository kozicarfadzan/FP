using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Model.Requests
{
    public class PaymentIntentConfirmRequest
    {
        [JsonProperty]
        public string PaymentMethodId { get; set; }
        [JsonProperty]
        public int RezervacijaId { get; set; }
    }
}
