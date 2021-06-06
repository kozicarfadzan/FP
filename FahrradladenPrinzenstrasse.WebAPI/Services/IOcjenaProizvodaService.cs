using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IOcjenaProizvodaService
    {
        bool OcijeniProizvod(OcjenaKorisnikaInsertRequest request);
    }
}
