using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
    public class Klijent
    {
        [ForeignKey("Korisnik")]
        public int Id { get; set; }

        public Korisnik Korisnik { get; set; }

        public DateTime DatumRegistracije { get; set; }
    }
}
