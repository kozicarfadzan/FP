using FahrradladenPrinzenstraße.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class PrikaziServiseVM
    {
        public string Pretraga { get; set; }
        public List<Servis> Servisi { get; set; }

  
    }
}
