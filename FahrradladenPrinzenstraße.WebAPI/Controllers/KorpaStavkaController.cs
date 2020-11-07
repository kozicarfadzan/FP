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
    public class KorpaStavkaController : ControllerBase
    {
        private readonly IKorpaStavkaService _service;

        public KorpaStavkaController(IKorpaStavkaService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.KorpaStavka> Get()
        {
            return _service.Get();
        }
        
        [HttpGet("GetBrojStavki")]
        public int GetBrojStavki()
        {
            return _service.GetBrojStavki();
        }

        [HttpGet("{Id}")]
        public Model.KorpaStavka GetById(int Id)
        {
            return _service.GetById(Id);
        }

        [HttpPost]
        public Model.KorpaStavka Insert([FromBody] Model.Requests.KorpaStavkaInsertRequest request)
        {
            return _service.Insert(request);
        }

        [HttpPut("{Id}")]
        public Model.KorpaStavka Update(int Id, [FromBody] Model.Requests.KorpaStavkaInsertRequest request)
        {
            return _service.Update(Id, request);
        }

        [HttpDelete("{Id}")]
        public bool Delete(int Id)
        {
            return _service.Delete(Id);
        }
    }
}