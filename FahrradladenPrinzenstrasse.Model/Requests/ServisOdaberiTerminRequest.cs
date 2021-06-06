using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class ServisOdaberiTerminRequest
    {
        public Servis Servis { get; set; }

        [Required]
        public int Id { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Kolicina nije uspravno unesena.")]
        public int Kolicina { get; set; }
        [Required]
        public DateTime Datum { get; set; }
    }
}
