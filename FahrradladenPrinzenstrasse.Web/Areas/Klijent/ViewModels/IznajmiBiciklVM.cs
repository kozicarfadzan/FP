using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels
{
    public class IznajmiBiciklVM
    {
        public Bicikl Bicikl { get; set; }
        public int KolicinaNaStanju { get; set; }
        public List<string> RezervisaniTermini { get; set; }

        [Required]
        public int Id { get; set; }
        [Required]
        public int Kolicina { get; set; }
        [Required]
        public DateTime DatumPreuzimanja { get; set; }
        [Required]
        public DateTime DatumVracanja { get; set; }
    }
}
