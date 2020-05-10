using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FahrradladenPrinzenstraße.Data.EntityModels
{
  public  class Bicikl
    {
        public Bicikl()
        {
            BiciklStanje = new List<BiciklStanje>();
        }

        public int BiciklId { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public string PuniNaziv =>  Model.Proizvodjac?.Naziv + " " + Model.Naziv;
        public short GodinaProizvodnje { get; set; }
        public Stanje Stanje { get; set; }
        public byte[] Slika { get; set; }
        public double? CijenaPoDanu { get; set; }
        public double? Cijena { get; set; }
        public int BojaId { get; set; }
        public Boja Boja { get; set; }
        public bool NoznaKocnica { get; set; }
        [DefaultValue(true)]
        public bool Aktivan { get; set; } = true;

        public IEnumerable<BiciklStanje> BiciklStanje { get; set; }
    }
}
