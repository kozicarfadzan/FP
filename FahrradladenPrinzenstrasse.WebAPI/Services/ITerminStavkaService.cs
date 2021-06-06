using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface ITerminStavkaService
    {
        List<TerminStavka> Get();
        TerminStavka GetById(int id);
        bool Delete(int id);
        int GetBrojStavki();
    }
}
