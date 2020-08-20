
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

        public Korisnik Korisnik { get; set; }
    }

}
