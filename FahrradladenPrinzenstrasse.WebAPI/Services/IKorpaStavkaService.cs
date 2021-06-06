using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IKorpaStavkaService
    {
        List<KorpaStavka> Get();
        KorpaStavka GetById(int id);
        KorpaStavka Insert(KorpaStavkaInsertRequest request);
        KorpaStavka Update(int id, KorpaStavkaInsertRequest request);
        bool Delete(int id);
        int GetBrojStavki();
    }
}
