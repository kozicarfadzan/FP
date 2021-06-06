using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels
{
    public class OdaberiTerminVM
    {
        public Servis Servis { get; set; }

        [Required]
        public int Id { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Kolicina nije uspravno unesena.")]
        public int Kolicina { get; set; }
        [Required]
        public DateTime Datum { get; set; }
        public TimeSpan Satnica { get; set; }
    }
}
