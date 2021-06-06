using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class BiciklGPSLokacije
    {
        public int BiciklGPSLokacijeId { get; set; }
        public int BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }

        [Column(TypeName = "decimal(10, 6)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(10, 6)")]
        public decimal Longitude { get; set; }
        public DateTime DateReported { get; set; }
    }
}
