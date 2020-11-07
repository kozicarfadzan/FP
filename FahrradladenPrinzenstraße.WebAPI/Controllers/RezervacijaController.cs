using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace FahrradladenPrinzenstraße.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RezervacijaController : ControllerBase
    {
        private readonly IRezervacijaService _service;

        public RezervacijaController(IRezervacijaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public List<Model.Rezervacija> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        [Authorize]
        public Model.Rezervacija GetById(int Id)
        {
            return _service.GetById(Id);
        }

        [HttpPost]
        [Authorize(Roles = "Klijent")]
        public Model.Rezervacija Insert([FromBody] Model.Requests.RezervacijaInsertRequest request)
        {
            return _service.Insert(request);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Klijent,Administrator")]
        public Model.Rezervacija Update(int Id, [FromBody] Model.Requests.RezervacijaInsertRequest request)
        {
            return _service.Update(Id, request);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles = "Administrator")]
        public bool Delete(int Id)
        {
            return _service.Delete(Id);
        }

        [HttpPost("PotvrdiUplatu")]
        [AllowAnonymous]
        public ActionResult PotvrdiUplatu(Model.Requests.PaymentIntentConfirmRequest request)
        {
            return _service.PotvrdiUplatu(request);
        }
    }
}