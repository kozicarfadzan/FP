using FahrradladenPrinzenstraße.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Mobile.Models.PotvrdaNarudzbe
{
    public class DetaljiServisaMobile
    {
        public string Proizvodjac { get; set; }
        public string Model { get; set; }
        public string Boja { get; set; }
        public string Opis { get; set; }
        public Tip Tip { get; set; }
        public string DodatniTroskovi { get; set; }
    }
}
