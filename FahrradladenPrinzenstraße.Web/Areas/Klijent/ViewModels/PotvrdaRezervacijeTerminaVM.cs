using FahrradladenPrinzenstraße.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Klijent.ViewModels
{
    public class PotvrdaRezervacijeTerminaVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Kolicina nije uspravno unesena.")]
        public int Kolicina { get; set; }
        [Required]
        public DateTime Datum { get; set; }

        public double UkupniIznos { get; internal set; }
        public ServisDetalji [] DetaljiServisa { get; set; }
        public int BrojServisStavki { get; set; }


        public class ServisDetalji
        {
            [Required]
            public string Proizvodjac { get; set; }
            [Required]
            public string Model { get; set; }
            [Required]
            public string Boja { get; set; }
            [Required]
            public string Opis { get; set; }
            [Required]
            public Tip Tip { get; set; }
            [Required]
            public int DodatniTroskovi { get; set; }
        }
    }
}
