using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
    public class Klijent
    {
        [ForeignKey("Korisnik")]
        public int Id { get; set; }

        public DateTime DatumRegistracije { get; set; }

        [JsonIgnore]
        public List<KorpaStavka> KorpaStavke { get; set; }
    }
}
