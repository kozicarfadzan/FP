using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class UrediDioVM
    {
        public int DioId { get; set; }
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Polje Opis je obavezno.")]
        [StringLength(maximumLength: 1000, MinimumLength = 10, ErrorMessage = "Minimalno 10 karaktera.")]
        public string Opis { get; set; }
        public double Cijena { get; set; }
        [Required(ErrorMessage = "Polje Proizviđač je obavezno.")]
        [DisplayName("Proizvođač")]
        public int ProizvodjacId { get; set; }


        public byte[] Slika { get; set; }

        public List<string> DioStanja_Sifre { get; set; }
        public List<int> DioStanja_Lokacije { get; set; }
        public List<DioStanje> DioStanje { get; set; }


    }
}