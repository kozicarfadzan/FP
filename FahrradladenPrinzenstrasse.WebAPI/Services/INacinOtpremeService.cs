using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface INacinOtpremeService
    {
        List<NacinOtpreme> Get();
        NacinOtpreme GetById(int id);
        NacinOtpreme Insert(NacinOtpremeInsertRequest request);
        NacinOtpreme Update(int id, NacinOtpremeInsertRequest request);
        bool Delete(int id);
    }
}
