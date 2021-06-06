using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Servis
    {
        public int ServisId { get; set; }
        [DisplayName("Tip servisa")]
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public double Trajanje { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
    }
}
