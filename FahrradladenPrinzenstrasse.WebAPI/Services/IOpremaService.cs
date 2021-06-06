using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IOpremaService
    {
        List<Model.Oprema> Get(Model.Requests.OpremaSearchRequest request);
        Model.Oprema GetById(int id);
    }
}
