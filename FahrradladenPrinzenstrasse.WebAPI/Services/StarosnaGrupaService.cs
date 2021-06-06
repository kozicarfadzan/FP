using AutoMapper;
using FahrradladenPrinzenstrasse.Data;
using FahrradladenPrinzenstrasse.Model;
using FahrradladenPrinzenstrasse.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstrasse.WebAPI.Services
{
    public class StarosnaGrupaService : IStarosnaGrupaService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public StarosnaGrupaService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<StarosnaGrupa> Get()
        {
            var list = _context.StarosnaGrupa
                .Where(x => x.IsDeleted == false).ToList();

            return _mapper.Map<List<Model.StarosnaGrupa>>(list);
        }

        public StarosnaGrupa GetById(int id)
        {
            var entity = _context.StarosnaGrupa.Find(id);
            return _mapper.Map<Model.StarosnaGrupa>(entity);
        }

    }
}
