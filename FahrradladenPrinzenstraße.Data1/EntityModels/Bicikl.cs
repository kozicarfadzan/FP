using System;
using System.Collections.Generic;
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
        public string PuniNaziv =>  Model.Tip + " " + Model.Naziv;
        public short GodinaProizvodnje { get; set; }
        public Stanje Stanje { get; set; }
        public string Slika { get; set; }
        public double? CijenaPoDanu { get; set; }
        public double? Cijena { get; set; }
        public int BojaId { get; set; }
        public Boja Boja { get; set; }
        public bool NoznaKocnica { get; set; }

        public IEnumerable<BiciklStanje> BiciklStanje { get; set; }
    }
}
