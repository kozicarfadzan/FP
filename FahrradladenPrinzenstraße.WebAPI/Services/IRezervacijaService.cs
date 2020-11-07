using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public interface IRezervacijaService
    {
        List<Rezervacija> Get();
        Rezervacija GetById(int id);
        Rezervacija Insert(RezervacijaInsertRequest request);
        Rezervacija Update(int id, RezervacijaInsertRequest request);
        bool Delete(int id);
        ActionResult PotvrdiUplatu(PaymentIntentConfirmRequest request);
    }
}
