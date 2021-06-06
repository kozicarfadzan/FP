using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public interface IRezervacijaService
    {
        List<Rezervacija> Get(RezervacijaSearchRequest request);
        RezervacijaDetalji GetById(int id);
        Rezervacija Insert(RezervacijaInsertRequest request);
        Rezervacija Update(int id, RezervacijaInsertRequest request);
        ActionResult PotvrdiUplatu(PaymentIntentConfirmRequest request);
        bool OtkaziRezervaciju(int id);
    }
}
