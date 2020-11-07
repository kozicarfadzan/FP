using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FahrradladenPrinzenstraße.Model.Requests
{
    public class BiciklSearchRequest
    {
        public int ModelId { get; set; }
        public string VelicinaOkvira { get; set; }
        public bool SamoIznajmljivanje { get; set; }

        // radio btn
        public int? NoznaKocnica { get; set; }
        public Suspenzija? Suspenzija { get; set; }

        // checkbox
        public List<Stanje> Stanje { get; set; }
        public List<int> ProizvodjacId { get; set; }
        public List<SpolBicikl> SpolBicikla { get; set; }
        public List<int> VelicinaOkviraId { get; set; }
        public List<int> StarosnaGrupaId { get; set; }
        public List<int> BojaId { get; set; }
        public List<int> Brzina { get; set; }
    }
}
