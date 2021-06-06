using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model
{
    public class PreporuceniProizvod
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public TipProizvoda Tip { get; set; }
        public byte[] Slika { get; set; }
        public double Cijena { get; set; }
    }
    public enum TipProizvoda
    {
        Bicikl, Dio, Oprema
    }
}
