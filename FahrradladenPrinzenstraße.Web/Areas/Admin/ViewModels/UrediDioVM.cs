using FahrradladenPrinzenstraße.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.Web.Areas.Admin.ViewModels
{
    public class UrediDioVM
    {
        public UrediDioVM()
        {
        }

        public UrediDioVM(MyContext db, Data.EntityModels.Dio dio)
        {

            Proizvodjaci = db.Proizvodjac.Select(g => new SelectListItem()
            {
                Value = g.ProizvodjacId.ToString(),
                Text = g.Naziv
            }).ToList();



            DioId = dio.DioId;
            Naziv = dio.Naziv;
            Opis = dio.Opis;
            Cijena = dio.Cijena;

            ProizvodjacId = dio.ProizvodjacID;
        }
        public int DioId { get; set; }

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