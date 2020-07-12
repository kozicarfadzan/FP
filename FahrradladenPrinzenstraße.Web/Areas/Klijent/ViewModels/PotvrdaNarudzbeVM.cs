using FahrradladenPrinzenstraße.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Klijent.ViewModels
{
    public class PotvrdaNarudzbeVM
    {
        public List<KorpaStavka> Stavke { get; set; }
        public KorisnickiPodaci Podaci { get; set; }
        public double UkupnoProizvodi { get; internal set; }
        public double Osnovica { get; internal set; }
        public double UkupniIznos { get; internal set; }
        public double UkupnoPoreza { get; internal set; }
        public double Postarina { get; internal set; }
        public List<NacinOtpreme> NaciniOtpreme { get; set; }
        public int NacinOtpremeId { get; set; }
        public string NacinPlacanja { get; set; }
        public int BrojKupljenihProizvoda { get; set; }

        public class KorisnickiPodaci
        {
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string AdresaStanovanja { get; set; }
            public string BrojTelefona { get; set; }
            public string Email { get; set; }
            public string Grad { get; set; }
            public string Pokrajina { get; set; }
            public string PostanskiKod { get; set; }
            public string Država { get; set; }

        }
    }
}
