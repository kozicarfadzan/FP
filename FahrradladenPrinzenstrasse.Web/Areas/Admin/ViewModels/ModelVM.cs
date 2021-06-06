using FahrradladenPrinzenstrasse.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class ModelVM
    {


        public class Row
        {

            public string Naziv { get; set; }

            public Proizvodjac Proizvodjac { get; set; }


            public int Brzina { get; set; }
            public Suspenzija Suspenzija { get; set; }
            public SpolBicikl SpolBicikl { get; set; }
            public Tip Tip { get; set; }

            public MaterijalOkvira MaterijalOkvira { get; set; }

            
            public StarosnaGrupa StarosnaGrupa { get; set; }

        
            public VelicinaOkvira VelicinaOkvira { get; set; }
        }
        public List<Row> rows { get; set; }
    }
}
