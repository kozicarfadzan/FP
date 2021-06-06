using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstrasse.Data.EntityModels
{
    public class Rezervacija
    {
        public int RezervacijaId { get; set; }
        public int KlijentId { get; set; }
        public Klijent Klijent { get; set; }
        [DisplayName("Datum rezervacije")]
        public DateTime DatumRezervacije { get; set; }
        [DisplayName("Datum uplate")]
        public DateTime? DatumUplate { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        [DisplayName("Adresa stanovanja")]
        public string AdresaStanovanja { get; set; }
        [DisplayName("Broj telefona")]
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Grad { get; set; }
        public string Pokrajina { get; set; }
        [DisplayName("Poštanski kod")]
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

        public List<RezervacijaIznajmljenaBicikla> RezervacijaIznajmljenaBicikla { get; set; }
        public List<RezervacijaProdajaBicikla> RezervacijaProdajaBicikla { get; set; }
        public List<RezervacijaProdajaDio> RezervacijaProdajaDio { get; set; }
        public List<RezervacijaProdajaOprema> RezervacijaProdajaOprema { get; set; }
        public List<RezervacijaServis> RezervacijaServis { get; set; }

        public string ImePrezime { get => Ime + " " + Prezime; }

    }
    public enum StanjeRezervacije
    {
        Čekanje_uplate, U_obradi, Odbijena, Završena,
        Otkazana
    }
}
