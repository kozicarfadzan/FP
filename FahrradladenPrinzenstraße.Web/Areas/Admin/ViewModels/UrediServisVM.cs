using FahrradladenPrinzenstraße.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class UrediServisVM
    {

        public UrediServisVM()
        {
        }
        public UrediServisVM(MyContext db, Data.EntityModels.Servis servis)
        {

            ServisId = servis.ServisId;
            Naziv = servis.Naziv;
            Opis = servis.Opis;
            Cijena = servis.Cijena;
         
        }
        public int ServisId { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public double Cijena { get; set; }
    }
}
