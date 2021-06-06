using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
    public class BiciklGPSLokacije
    {
        public int BiciklGPSLokacijeId { get; set; }
        public int BiciklId { get; set; }
        public Bicikl Bicikl { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateReported { get; set; }
    }
}
