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
    [Authorize(Roles ="Klijent")]
    public class TerminStavkaController : ControllerBase
    {
        private readonly ITerminStavkaService _service;

        public TerminStavkaController(ITerminStavkaService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.TerminStavka> Get()
        {
            return _service.Get();
        }
        
        [HttpGet("GetBrojStavki")]
        public int GetBrojStavki()
        {
            return _service.GetBrojStavki();
        }

        [HttpGet("{Id}")]
        public Model.TerminStavka GetById(int Id)
        {
            return _service.GetById(Id);
        }

        [HttpDelete("{Id}")]
        public bool Delete(int Id)
        {
            return _service.Delete(Id);
        }
    }
}