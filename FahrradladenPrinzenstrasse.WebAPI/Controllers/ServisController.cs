using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.Model.Requests;
using FahrradladenPrinzenstrasse.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstrasse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Klijent")]
    public class ServisController : ControllerBase
    {
        private readonly IServisService _service;

        public ServisController(IServisService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Servis> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        public Model.Servis GetById(int Id)
        {
            return _service.GetById(Id);
        }

        [HttpPost("DostupniTermini")]
        public List<string> DostupniTermini([FromBody]ServisOdaberiTerminRequest request)
        {
            return _service.DostupniTermini(request);
        }

        [HttpPost("OdaberiTermin")]
        public bool OdaberiTermin([FromBody]ServisOdaberiTerminRequest request)
        {
            return _service.OdaberiTermin(request);
        }

        [HttpGet("PotvrdaRezervacijeTermina")]
        public ServisPotvrdaRezervacijeTerminaRequest PotvrdaRezervacijeTerminaGet([FromQuery] ServisPotvrdaRezervacijeTerminaRequest request)
        {
            return (ServisPotvrdaRezervacijeTerminaRequest) _service.PotvrdaRezervacijeTermina(request);
        }

        [HttpPost("PotvrdaRezervacijeTermina")]
        public bool PotvrdaRezervacijeTerminaPost([FromBody]ServisPotvrdaRezervacijeTerminaRequest request)
        {
            return (bool)_service.PotvrdaRezervacijeTermina(request);
        }

    }
}