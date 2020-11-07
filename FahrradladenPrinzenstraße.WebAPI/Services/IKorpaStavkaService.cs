using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.WebAPI.Services
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
