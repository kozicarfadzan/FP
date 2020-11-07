using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Model.Requests
{
    public class KorpaStavkaInsertRequest
    {
        public int? BiciklId { get; set; }
        public int? OpremaId { get; set; }
        public int? DioId { get; set; }
        public int Kolicina { get; set; }
        public DateTime? DatumServisiranja { get; set; }
    }
}
