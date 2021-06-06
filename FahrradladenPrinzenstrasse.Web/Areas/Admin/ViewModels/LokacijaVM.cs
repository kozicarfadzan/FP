using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.Web.Areas.Admin.ViewModels
{
    public class LokacijaVM
    {
        public class Row
        {
            public string Naziv { get; set; }

        }
        public List<Row> rows { get; set; }
    }
}
