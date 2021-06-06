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
    public class NacinOtpremeController : ControllerBase
    {
        private readonly INacinOtpremeService _service;

        public NacinOtpremeController(INacinOtpremeService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<Model.NacinOtpreme> Get()
        {
            return _service.Get();
        }

        [HttpGet("{Id}")]
        public Model.NacinOtpreme GetById(int Id)
        {
            return _service.GetById(Id);
        }

        [HttpPost]
        [Authorize(Roles ="Administrator")]
        public Model.NacinOtpreme Insert([FromBody] Model.Requests.NacinOtpremeInsertRequest request)
        {
            return _service.Insert(request);
        }

        [HttpPut("{Id}")]
        [Authorize(Roles ="Administrator")]
        public Model.NacinOtpreme Update(int Id, [FromBody] Model.Requests.NacinOtpremeInsertRequest request)
        {
            return _service.Update(Id, request);
        }

        [HttpDelete("{Id}")]
        [Authorize(Roles ="Administrator")]
        public bool Delete(int Id)
        {
            return _service.Delete(Id);
        }
    }
}