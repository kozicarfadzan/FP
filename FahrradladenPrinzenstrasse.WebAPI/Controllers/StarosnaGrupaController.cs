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
    public class StarosnaGrupaController : ControllerBase
    {
        private readonly IStarosnaGrupaService _service;

        public StarosnaGrupaController(IStarosnaGrupaService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.StarosnaGrupa> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        public Model.StarosnaGrupa GetById(int Id)
        {
            return _service.GetById(Id);
        }

    }
}