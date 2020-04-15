using FahrradladenPrinzenstraße.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.ViewModels
{
    public class PrikaziBiciklVM
    {
        public string Pretraga { get; set; }
        public int? Poredak { get; set; }
        public bool PrikaziKaoListu { get; set; }

        public List<Row> Bicikla { get; set; }

        /* checkbox db object */
        public List<Proizvodjac> Proizvodjaci { get; set; }
        public List<int> ProizvodjacId { get; set; }

        public List<VelicinaOkvira> VelicineOkvira { get; set; }
        public List<int> VelicinaOkviraId { get; set; }
        public List<StarosnaGrupa> StarosneGrupe { get; set; }
        public List<int> StarosnaGrupaId { get; set; }
        public List<MaterijalOkvira> MaterijaliOkvira { get; set; }
        public List<int> MaterijalOkviraId { get; set; }
        public List<Boja> Boje { get; set; }
        public List<int> BojaId { get; set; }

        /* checkbox enum */
        public List<Stanje> Stanja { get; set; }
        public List<string> Stanje { get; set; }
        public List<Suspenzija> Suspenzije { get; set; }
        public List<SpolBicikl> SpoloviBicikla { get; set; }
        public List<string> SpolBicikla { get; set; }
        public List<Tip> Tipovi { get; set; }
        public List<string> Tip { get; set; }

        /* radio */
        public int? NoznaKocnica { get; set; }

        public int? Suspenzija { get; set; }

        /* dropdown */
        public List<int> Brzine { get; set; }
        public int? Brzina { get; set; }


        public class Row
        {
            public int BiciklId { get; set; }
            public Model Model { get; set; }
            public string PuniNaziv => Model.Tip + " " + Model.Naziv;
            public short GodinaProizvodnje { get; set; }
            public Stanje Stanje { get; set; }
            public byte[] Slika { get; set; }
            public double? CijenaPoDanu { get; set; }
            public double? Cijena { get; set; }
            public Boja Boja { get; set; }
            public bool NoznaKocnica { get; set; }
            public int Kolicina { get; internal set; }
            public bool Aktivan { get; set; }
        }
    }
}
