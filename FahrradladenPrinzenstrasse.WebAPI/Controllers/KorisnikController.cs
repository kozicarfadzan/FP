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
    public class KorisnikController : ControllerBase
    {
        private readonly IKorisnikService _service;

        public KorisnikController(IKorisnikService service)
        {
            _service = service;
        }

        [HttpGet("{Id}")]
        public Model.Korisnik GetById(int Id)
        {
            return _service.GetById(Id);
        }

        [HttpPost]
        public Model.Korisnik Insert([FromBody] Model.Requests.KorisnikInsertRequest request)
        {
            return _service.Insert(request);
        }

        [Authorize]
        [HttpPut("{Id}")]
        public Model.Korisnik Update(int Id, [FromBody] Model.Requests.KorisnikUpdateRequest request)
        {
            return _service.Update(Id, request);
        }

        [HttpGet("MyProfile")]
        [Authorize]
        public Model.Korisnik MyProfile()
        {
            return _service.MyProfile();
        }

    }
}