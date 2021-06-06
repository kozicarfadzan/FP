using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class OcjenaKorisnikaInsertRequest
    {
        public int Ocjena { get; set; }
        public int? BiciklId { get; set; }
        public int? DioId { get; set; }
        public int? OpremaId { get; set; }
    }
}
