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
    public class GradController : ControllerBase
    {
        private readonly IGradService _service;

        public GradController(IGradService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Grad> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        public Model.Grad GetById(int Id)
        {
            return _service.GetById(Id);
        }

    }
}