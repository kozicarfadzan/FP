using FahrradladenPrinzenstrasse.Data.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Zaposlenik.ViewModels
{
    public class PrikaziBiciklGPSLokacijeVM
    {
        public List<BiciklGPSLokacije> GPSLokacije  { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Datum { get; set; }

        public List<SelectListItem> Bicikla { get; set; }
        public int BiciklId { get; set; }
    }
}
