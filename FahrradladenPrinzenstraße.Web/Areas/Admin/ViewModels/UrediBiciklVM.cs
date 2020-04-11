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
    public class UrediBiciklVM
    {

        public int BiciklId { get; set; }
        [DisplayName("Model")]
        public int ModelId { get; set; }
        [DisplayName("Godina proizvodnje")]
        public short GodinaProizvodnje { get; set; }
        public Stanje Stanje { get; set; }
        public byte[] Slika { get; set; }
        [DisplayName("Cijena po danu")]
        public double? CijenaPoDanu { get; set; }
        public double? Cijena { get; set; }
        [DisplayName("Boja")]
        public int BojaId { get; set; }
        [DisplayName("Nožna kočnica")]
        public bool NoznaKocnica { get; set; }

        public List<string> BiciklStanja_Sifre { get; set; }
        public List<int> BiciklStanja_Lokacije { get; set; }
        public List<BiciklStanje> BiciklStanje { get; set; }
    }
}
