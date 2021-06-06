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
    [Authorize(Roles = "Klijent")]
    public class DioController : ControllerBase
    {
        private readonly IDioService _service;

        public DioController(IDioService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.Dio> Get([FromQuery] Model.Requests.DioSearchRequest request)
        {
            return _service.Get(request);
        }

        [HttpGet("{Id}")]
        public Model.Dio GetById(int Id)
        {
            return _service.GetById(Id);
        }

    }
}