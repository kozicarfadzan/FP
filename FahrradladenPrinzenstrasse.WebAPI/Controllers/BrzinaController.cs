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
    public class BrzinaController : ControllerBase
    {
        private readonly IBrzinaService _service;

        public BrzinaController(IBrzinaService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<int> Get([FromQuery] Model.Requests.BrzinaSearchRequest request)
        {
            return _service.Get(request);
        }

    }
}