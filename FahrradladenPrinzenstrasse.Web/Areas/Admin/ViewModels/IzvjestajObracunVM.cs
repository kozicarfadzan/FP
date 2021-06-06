using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Reporting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class IzvjestajObracunVM
    {
        public List<RezervacijaProdajaBicikla> Bicikli { get; set; }

        public List<SelectListItem> ReportTypes { get; set; }
        public ObracunReportType? ReportType { get; set; }

        public List<string> ChartLabels { get; set; }
        public Dictionary<VrstaProizvoda, List<double>> Data { get; set; }
        public double Maximum { get; set; }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public List<SelectListItem> Months { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTime { get; set; }
        public List<RezervacijaProdajaDio> Dijelovi { get; set; }
        public List<RezervacijaProdajaOprema> Oprema { get; set; }
        public List<RezervacijaServis> Servisi { get; set; }
        public List<RezervacijaIznajmljenaBicikla> Iznajmljivanje { get; set; }
        public List<ProdaniProizvod> NajprodavanijiProizvodi { get; set; }

        public enum ObracunReportType
        {
            Mjesečno, Dnevno, Sedmično, Godišnje
        }
        public enum VrstaProizvoda
        {
            Biciklo, Oprema, Dio, Servis, Iznajmljivanje
        }

        public class ZaradaResult
        {
            public DateTime DateTime { get; set; }
            public double Sum { get; set; }
            public int Hour { get; set; }
            public int Month { get; internal set; }
        }

        public class ProdaniProizvod
        {
            public string Naziv { get; set; }
            public int Kolicina { get; set; }
            public double JedCijena { get; set; }
            public double Zarada { get; set; }
            public VrstaProizvoda VrstaProizvoda { get; set; }
        }
    }
}
