using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class DodajOpremuVM
    {
        public int OpremaId { get; set; }
        [Required(ErrorMessage = "Polje Naziv je obavezno.")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Polje Opis je obavezno.")]
        [StringLength(maximumLength: 1000, MinimumLength = 10, ErrorMessage = "Minimalno 10 karaktera.")]
        public string Opis { get; set; }
        [Range(0.5, double.MaxValue, ErrorMessage = "Cijena mora biti unesena ispravno.")]
        public double Cijena { get; set; }
        [DisplayName("Proizvođač")]
        [Required(ErrorMessage = "Polje Proizvođač je obavezno.")]
        public int ProizvodjacId { get; set; }

        public byte[] Slika { get; set; }

        public List<string> OpremaStanja_Sifre { get; set; }
        public List<int> OpremaStanja_Lokacije { get; set; }
        public List<OpremaStanje> OpremaStanje { get; set; }

    }
}
