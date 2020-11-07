using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.WebAPI.Services
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
