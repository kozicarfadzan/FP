using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstraße.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstraße.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Klijent")]
    public class OpremaController : ControllerBase
    {
        private readonly IOpremaService _service;

        public OpremaController(IOpremaService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Oprema> Get([FromQuery] Model.Requests.OpremaSearchRequest request)
        {
            return _service.Get(request);
        }

        [HttpGet("{Id}")]
        public Model.Oprema GetById(int Id)
        {
            return _service.GetById(Id);
        }

    }
}