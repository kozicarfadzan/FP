using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FahrradladenPrinzenstrasse.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FahrradladenPrinzenstrasse.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Klijent")]
    public class BiciklController : ControllerBase
    {
        private readonly IBiciklService _service;

        public BiciklController(IBiciklService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Bicikl> Get([FromQuery] Model.Requests.BiciklSearchRequest request)
        {
            return _service.Get(request);
        }

        [HttpGet("{Id}")]
        public Model.Bicikl GetById(int Id)
        {
            return _service.GetById(Id);
        }
        [HttpGet("GetDaneBezDostupnihTermina/{Id}/{Kolicina}")]
        public List<DateTime> GetDaneBezDostupnihTermina(int Id, int Kolicina)
        {
            return _service.GetDaneBezDostupnihTermina(Id, Kolicina);
        }
        [HttpGet("SaveGPSLocation/{Id}")]
        [AllowAnonymous]
        public IActionResult SaveGPSLocation(int Id, [FromQuery] string lat, [FromQuery] string lon)
        {
            return _service.SaveGPSLocation(Id, lat, lon);
        }

        [HttpPost("OdaberiTermin")]
        public bool OdaberiTermin([FromBody] Model.Requests.BiciklOdaberiTerminRequest request)
        {
            return _service.OdaberiTermin(request);
        }

    }
}