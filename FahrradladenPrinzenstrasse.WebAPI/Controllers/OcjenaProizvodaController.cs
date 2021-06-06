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
    public class OcjenaProizvodaController : ControllerBase
    {
        private readonly IOcjenaProizvodaService _service;

        public OcjenaProizvodaController(IOcjenaProizvodaService service)
        {
            _service = service;
        }

        [HttpPost("OcijeniProizvod")]
        public bool OcijeniProizvod([FromBody]Model.Requests.OcjenaKorisnikaInsertRequest request)
        {
            return _service.OcijeniProizvod(request);
        }

    }
}