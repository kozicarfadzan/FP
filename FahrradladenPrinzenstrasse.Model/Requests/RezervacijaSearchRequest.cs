using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class RezervacijaSearchRequest
    {
        public DateTime? DatumDo { get; set; }
        public DateTime? DatumOd { get; set; }
    }
}
