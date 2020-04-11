using FahrradladenPrinzenstraße.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class UrediOpremuVM
    {
        public UrediOpremuVM()
        {
        }

        public UrediOpremuVM(MyContext db, Data.EntityModels.Oprema oprema)
        {

            Proizvodjaci = db.Proizvodjac.Select(g => new SelectListItem()
            {
                Value = g.ProizvodjacId.ToString(),
                Text = g.Naziv
            }).ToList();

            

            OpremaId = oprema.OpremaId;
            Naziv = oprema.Naziv;
            Opis = oprema.Opis;
            Cijena = oprema.Cijena;
        
            ProizvodjacId = oprema.ProizvodjacID;
        }
        public int OpremaId { get; set; }
        [Required]
        public string Naziv { get; set; }

        [Required]
        public string Opis { get; set; }

        [Required]
        public double Cijena { get; set; }

        
        public int ProizvodjacId { get; set; }
        public List<SelectListItem> Proizvodjaci { get; set; }


    }
}
