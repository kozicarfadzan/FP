using FahrradladenPrinzenstraße.Data;
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
    public class UrediOpremuVM
    {
        public int OpremaId { get; set; }
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Polje Opis je obavezno.")]
        [StringLength(maximumLength: 1000, MinimumLength = 10, ErrorMessage = "Minimalno 10 karaktera.")]
        public string Opis { get; set; }
        public double Cijena { get; set; }
        [Required]
        [DisplayName("Proizvođač")]
        public int ProizvodjacId { get; set; }

        public byte[] Slika { get; set; }

        public List<string> OpremaStanja_Sifre { get; set; }
        public List<int> OpremaStanja_Lokacije { get; set; }
        public List<OpremaStanje> OpremaStanje { get; set; }


    }
}