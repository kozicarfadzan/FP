using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstraße.Model
{
    public class Rezervacija
    {
        public int RezervacijaId { get; set; }
        public int KlijentId { get; set; }
        public DateTime DatumRezervacije { get; set; }
        public DateTime? DatumUplate { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string AdresaStanovanja { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Grad { get; set; }
        public string Pokrajina { get; set; }
        public string PostanskiKod { get; set; }
        public string Država { get; set; }

        public double UkupnoProizvodi { get; set; }
        public double Osnovica { get; set; }
        public double UkupniIznos { get; set; }
        public double UkupnoPoreza { get; set; }
        public double Postarina { get; set; }
        public NacinOtpreme NacinOtpreme { get; set; }
        public int NacinOtpremeId { get; set; }
        public string NacinPlacanja { get; set; }

        public StanjeRezervacije StanjeRezervacije { get; set; }

        public bool IsTerminRezervacija { get; set; }
        public bool IsServisRezervacija { get; set; }

    }
    public enum StanjeRezervacije
    {
        Čekanje_uplate, U_obradi, Odbijena, Završena
    }
}
