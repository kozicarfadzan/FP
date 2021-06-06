using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IServisService
    {
        List<Model.Servis> Get();
        Model.Servis GetById(int id);
        List<string> DostupniTermini(ServisOdaberiTerminRequest request);
        bool OdaberiTermin(ServisOdaberiTerminRequest request);
        object PotvrdaRezervacijeTermina(ServisPotvrdaRezervacijeTerminaRequest request);
    }
}
