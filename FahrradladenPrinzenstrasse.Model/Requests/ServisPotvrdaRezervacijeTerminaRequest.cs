using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstrasse.Model.Requests
{
    public class ServisPotvrdaRezervacijeTerminaRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Kolicina nije uspravno unesena.")]
        public int Kolicina { get; set; }
        [Required]
        public DateTime Datum { get; set; }
        public TimeSpan Satnica { get; set; }
        public double UkupniIznos { get; set; }
        public ServisDetalji[] DetaljiServisa { get; set; }
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
            public int DodatniTroskovi { get; set; }
        }
    }
}
