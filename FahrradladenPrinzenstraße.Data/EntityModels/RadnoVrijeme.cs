using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class RadnoVrijeme
    {
        public int Id { get; set; }
        public DayOfWeek DanUSedmici { get; set; }
        public TimeSpan Pocetak { get; set; }
        public TimeSpan Kraj { get; set; }
    }
}
