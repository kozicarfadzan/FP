using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class BiciklOdaberiTerminRequest
    {
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
