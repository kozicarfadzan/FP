using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstraße.WebAPI.Services
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
            var list = _context.StarosnaGrupa.ToList();

            return _mapper.Map<List<Model.StarosnaGrupa>>(list);
        }

        public StarosnaGrupa GetById(int id)
        {
            var entity = _context.StarosnaGrupa.Find(id);
            return _mapper.Map<Model.StarosnaGrupa>(entity);
        }

    }
}
