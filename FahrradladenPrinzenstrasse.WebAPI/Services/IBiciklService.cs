using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IBiciklService
    {
        List<Model.Bicikl> Get(Model.Requests.BiciklSearchRequest request);
        Model.Bicikl GetById(int id);
        List<DateTime> GetDaneBezDostupnihTermina(int id, int kolicina);
        bool OdaberiTermin(BiciklOdaberiTerminRequest request);
        IActionResult SaveGPSLocation(int Id, string lat, string lon);
    }
}
