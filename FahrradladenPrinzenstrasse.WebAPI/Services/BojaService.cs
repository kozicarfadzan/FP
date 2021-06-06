using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class BojaService : IBojaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public BojaService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<Boja> Get()
        {
            var list = _context.Boja
                .Where(x => x.IsDeleted == false)
                .ToList();

            return _mapper.Map<List<Model.Boja>>(list);
        }

        public Boja GetById(int id)
        {
            var entity = _context.Boja.Find(id);
            return _mapper.Map<Model.Boja>(entity);
        }

    }
}
