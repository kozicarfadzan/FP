
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Model
{
    public class Administrator
    {
        [ForeignKey("Korisnik")]
        public int Id { get; set; }
        [JsonIgnore]
        public Korisnik Korisnik { get; set; }
    }

}
