using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class IzvjestajInventarVM
    {
        public List<SelectListItem> Lokacije { get; set; }
        public int LokacijaId { get; set; }
        public List<string> OdabraneOpcije { get; set; }
        public List<SelectListItem> DostupneOpcije { get; set; }
        public List<SelectListItem> NaciniPrikaza { get; set; }
        public int NacinPrikaza { get; set; }

    }
}
