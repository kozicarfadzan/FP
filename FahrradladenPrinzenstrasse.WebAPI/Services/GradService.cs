using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class GradService : IGradService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public GradService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<Grad> Get()
        {
            var list = _context.Grad
                .Where(x => x.IsDeleted == false)
                .ToList();

            return _mapper.Map<List<Model.Grad>>(list);
        }

        public Grad GetById(int id)
        {
            var entity = _context.Grad.Find(id);
            return _mapper.Map<Model.Grad>(entity);
        }

    }
}
