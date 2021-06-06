using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Klijent
    {
        [ForeignKey("Korisnik")]
        public int Id { get; set; }

        public Korisnik Korisnik { get; set; }

        public DateTime DatumRegistracije { get; set; }

        public List<KorpaStavka> KorpaStavke { get; set; }
    }
}
