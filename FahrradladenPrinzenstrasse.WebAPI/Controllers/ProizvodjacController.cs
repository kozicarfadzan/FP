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
    [Authorize]
    public class ProizvodjacController : ControllerBase
    {
        private readonly IProizvodjacService _service;

        public ProizvodjacController(IProizvodjacService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Proizvodjac> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        public Model.Proizvodjac GetById(int Id)
        {
            return _service.GetById(Id);
        }

    }
}