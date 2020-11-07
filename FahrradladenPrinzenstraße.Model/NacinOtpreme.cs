using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
    public class NacinOtpreme
    {
        public int NacinOtpremeId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public double Cijena { get; set; }

        public string CijenaStr => Cijena.ToString("0.00") + " KM";
        public byte[] Slika { get; set; }

    }
}
