using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class RezervacijaInsertRequest
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

        public double Postarina { get; set; }
        public int NacinOtpremeId { get; set; }
        public string NacinPlacanja { get; set; }

        public bool ObradiIznajmljivanje { get; set; }
        public List<RezervacijaServis> RezervacijaServis { get; set; }
    }
}
