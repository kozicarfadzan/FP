using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class ProizvodjacService : IProizvodjacService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public ProizvodjacService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<Proizvodjac> Get()
        {
            var list = _context.Proizvodjac
                .Where(x => x.IsDeleted == false).ToList();

            return _mapper.Map<List<Model.Proizvodjac>>(list);
        }

        public Proizvodjac GetById(int id)
        {
            var entity = _context.Proizvodjac.Find(id);
            return _mapper.Map<Model.Proizvodjac>(entity);
        }

    }
}
