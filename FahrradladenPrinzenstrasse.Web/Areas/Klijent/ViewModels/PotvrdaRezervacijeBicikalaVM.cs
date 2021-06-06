using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Klijent.ViewModels
{
    public class PotvrdaRezervacijeBicikalaVM
    {
        public List<TerminStavka> Stavke { get; set; }
        public KorisnickiPodaci Podaci { get; set; }
        public double Osnovica { get; set; }
        public double UkupniIznos { get; set; }
        public double UkupnoPoreza { get; set; }

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
