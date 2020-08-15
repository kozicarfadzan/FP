using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.ViewModels
{
    public class PreporuceniProizvod
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public TipProizvoda Tip { get; set; }
        public byte[] Slika { get; set; }
        public double Cijena { get; set; }

        public string Link { get
            {
                if (Tip == TipProizvoda.Bicikl)
                    return "/Klijent/Bicikl/KupiBicikl/" + Id;
                if (Tip == TipProizvoda.Dio)
                    return "/Klijent/Dio/KupiDio/" + Id;
                if (Tip == TipProizvoda.Oprema)
                    return "/Klijent/Oprema/KupiOprema/" + Id;
                return null;
            } }

  
    }
    public enum TipProizvoda
    {
        Bicikl, Dio, Oprema
    }
}
