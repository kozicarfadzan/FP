using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public interface IStarosnaGrupaService
    {
        List<StarosnaGrupa> Get();
        StarosnaGrupa GetById(int id);
    }
}
