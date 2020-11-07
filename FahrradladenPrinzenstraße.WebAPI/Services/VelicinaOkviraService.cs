using AutoMapper;
using FahrradladenPrinzenstraße.Data;
using FahrradladenPrinzenstraße.Model;
using FahrradladenPrinzenstraße.Model.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FahrradladenPrinzenstraße.WebAPI.Services
{
    public class VelicinaOkviraService : IVelicinaOkviraService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public VelicinaOkviraService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<VelicinaOkvira> Get()
        {
            var list = _context.VelicinaOkvira.ToList();

            return _mapper.Map<List<Model.VelicinaOkvira>>(list);
        }

        public VelicinaOkvira GetById(int id)
        {
            var entity = _context.VelicinaOkvira.Find(id);
            return _mapper.Map<Model.VelicinaOkvira>(entity);
        }

    }
}
