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
    [Authorize]
    public class VelicinaOkviraController : ControllerBase
    {
        private readonly IVelicinaOkviraService _service;

        public VelicinaOkviraController(IVelicinaOkviraService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.VelicinaOkvira> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        public Model.VelicinaOkvira GetById(int Id)
        {
            return _service.GetById(Id);
        }

    }
}