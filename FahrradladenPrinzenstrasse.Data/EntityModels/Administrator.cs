
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Administrator
    {
        [ForeignKey("Korisnik")]
        public int Id { get; set; }

        public Korisnik Korisnik { get; set; }
    }

}
