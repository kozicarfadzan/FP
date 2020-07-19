using FahrradladenPrinzenstraße.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Klijent.ViewModels
{
    public class RezervacijePretragaVM
    {
        [DataType(DataType.Date)]
        public DateTime? DatumOd { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumDo { get; set; }
        public VrstaRezervacije? VrstaRezervacije { get; set; }
        public List<Rezervacija> Rezervacije{ get; set; }
    }

    public enum VrstaRezervacije
    {
        Kupovina, Servis, Iznajmljivanje
    }
}
